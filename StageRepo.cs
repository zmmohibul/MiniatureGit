namespace MiniatureGit
{
    public class StageRepo
    {
        public static async Task AddFileToStagingArea(string filePath)
        {
            await Setup(filePath);

            Commit headCommit = await Repository.GetHeadCommit();

            var fileSha = await Utils.GetSha1OfFileFromPathAsync(filePath);

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

        public static async Task RemoveFileFromStagingArea(string filePath)
        {
            await Setup(filePath);
            Repository.StagingArea.RemoveFile(filePath);
            await Repository.SaveStagingArea();
        }

        private static async Task Setup(string filePath)
        {
            ErrorLog.CheckNoFileExistError(filePath);
            await Repository.SetupStagingArea();
        }
    }
}