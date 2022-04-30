namespace MiniatureGit
{
    public class CommitRepo
    {
        public static async Task MakeCommit(string message)
        {
            var headCommit = await Repository.GetHeadCommit();
            var commit = Utils.CloneObject<Commit>(headCommit);
            commit.CommitMessage = message;
            commit.CommittedAt = DateTime.Now;

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
                else
                {
                    Repository.StagingArea.RemoveFile(filePath);
                    commit.RemoveFile(filePath);
                    hasFileChanged = true;      
                }
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
    }
}