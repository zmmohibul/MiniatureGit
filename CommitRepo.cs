using System.Text;

namespace MiniatureGit
{
    public class CommitRepo
    {
        public static async Task MakeCommit(string message)
        {
            var headSha = await File.ReadAllTextAsync(Repository.Head);
            var headCommit = await Utils.ReadObjectAsync<Commit>(Path.Join(Repository.Commits.FullName, headSha));

            var commit = Utils.CloneObject<Commit>(headCommit);
            commit.CommitMessage = message;
            commit.CommittedAt = DateTime.Now;
            commit.Parent = headSha;

            var hasFileChanged = false;

            foreach (var (filePath, fileSha) in Repository.StagingArea.FilesStagedForAddition)
            {
                if (File.Exists(filePath))
                {
                    var fileInPWDSha = await Utils.GetSha1OfFileFromPathAsync(filePath);
                    if (!fileSha.Equals(fileInPWDSha))
                    {
                        ErrorLog.FileChangedError(filePath);
                    }
                    
                    if (!Repository.StagingArea.FilesStagedForRemoval.ContainsKey(filePath))
                    {
                        commit.AddFile(filePath, fileSha);
                        hasFileChanged = true;
                    }
                    else
                    {
                        commit.RemoveFile(filePath);
                        hasFileChanged = true;
                    }
                }
            }

            foreach (var key in Repository.StagingArea.FilesStagedForRemoval.Keys)
            {
                commit.RemoveFile(key);
                File.Delete(key);
                hasFileChanged = true;
            }

            if (hasFileChanged)
            {
                await Repository.WriteFilesInStagingArea();
                await Repository.ClearAndSaveStagingArea();
                var commitSha = await Utils.WriteObjectAndGetObjectHashAsync<Commit>(Repository.Commits.FullName, commit);

                await Repository.ChangeCurrentBranchPointer(commitSha);
                await Repository.ChangeHeadPointer(commitSha);
            }
            else
            {
                Console.WriteLine("No file has been modified, added or removed since last commit.");
                Environment.Exit(1);
            }
        }

        public static async Task Checkout(string commitIdToCheckout)
        {
            var commits = Directory.GetFiles(Repository.Commits.FullName);
            var commitIdToCheckoutExists = false;
            
            foreach(var commit in commits)
            {
                var currCommitId = Path.GetRelativePath(Repository.Commits.FullName, commit);
                if (currCommitId.Equals(commitIdToCheckout))
                {
                    commitIdToCheckoutExists = true;
                    break;
                }
            }

            if (commitIdToCheckoutExists)
            {
                System.Console.WriteLine(commitIdToCheckout);

                var commitToCheckout = await Utils.ReadObjectAsync<Commit>(Path.Join(Repository.Commits.FullName, commitIdToCheckout));
                ClearPWD();
                

                foreach (var (file, fileSha) in commitToCheckout.FileNameFileShaDictionary)
                {
                    CreateDirectoriesForFile(file);
                    await File.WriteAllBytesAsync(file, await File.ReadAllBytesAsync(Path.Join(Repository.Files.FullName, fileSha)));
                }
            }
            else
            {
                Console.WriteLine($"No commit with the id {commitIdToCheckout} found.");
                Environment.Exit(1);
            }
        }

        private static void ClearPWD()
        {
            var directories = Directory.GetFiles(Repository.PWD.FullName, "*", SearchOption.AllDirectories).Where(d => !d.StartsWith(Path.Join(Repository.PWD.FullName, "MiniatureGit")) && !d.StartsWith(Path.Join(Repository.PWD.FullName, ".")));
            foreach(var directory in directories)
            {
                File.Delete(directory);
            }
        }

        private static void CreateDirectoriesForFile(string file)
        {
            var directoriesToCreateForFile = file.Split("/");
            if (directoriesToCreateForFile.Length > 1)
            {
                var path = new StringBuilder();
                for (int i = 0; i < directoriesToCreateForFile.Length - 1; i++)
                {
                    path.Append(directoriesToCreateForFile[i]);
                    path.Append("/");
                    if (!Directory.Exists(path.ToString()))
                    {
                        Directory.CreateDirectory(path.ToString());
                    }
                }
            }
        }
    }
}