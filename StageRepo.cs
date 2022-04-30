namespace MiniatureGit
{
    public class StageRepo
    {
        public static async Task AddFileToStagingArea(string filePath)
        {
            await Setup(filePath);
            Commit headCommit = await Repository.GetHeadCommit();


            try
            {
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

        public static async Task RemoveFileFromStagingArea(string filePath)
        {
            await Setup(filePath);
            Repository.StagingArea.RemoveFile(filePath);
        }

        private static async Task Setup(string filePath)
        {
            ErrorLog.CheckNoFileExistError(filePath);
            await Repository.SetupStagingArea();
        }


    }
}