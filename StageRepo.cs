namespace MiniatureGit
{
    public class StageRepo
    {
        public static async Task AddFileToStagingArea(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"No file {filePath} exists in present working directory");
                Environment.Exit(1);
            }

            var head = await File.ReadAllTextAsync(Repository.Head);
            var headCommitSha = await File.ReadAllTextAsync(head);
            
            Commit headCommit;

            await Repository.SetupStagingArea();

            try
            {
                headCommit = await Utils.ReadObjectAsync<Commit>(Path.Join(Repository.Commits.FullName, headCommitSha));
                var fileSha = await Utils.GetSha1OfFileFromPath(filePath);

                if (!headCommit.ContainsFile(filePath))
                {
                    Repository.StagingArea.AddFile(filePath, fileSha);
                }
                else
                {
                    var fileShaInLastCommit = headCommit.GetFileSha(filePath);
                    if (!fileShaInLastCommit.Equals(fileSha))
                    {
                       Repository.StagingArea.AddFile(filePath, fileSha);
                    }
                }
                await Repository.SaveStagingArea();
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
            }

            
        }
    }
}