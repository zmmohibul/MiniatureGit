namespace MiniatureGit
{
    public class StageRepo
    {
        public static async Task AddFileToStagingArea(string filePath)
        {
            await Setup(filePath);

            Commit headCommit = await CommitRepo.GetHeadCommit();

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
            await SaveStagingArea();
        }

        public static async Task RemoveFileFromStagingArea(string filePath)
        {
            await Setup(filePath);
            Repository.StagingArea.RemoveFile(filePath);
            await SaveStagingArea();
        }

        private static async Task Setup(string filePath)
        {
            ErrorLog.CheckNoFileExistError(filePath);
            await SetupStagingArea();
        }

        public static async Task SetupStagingArea()
        {
            Repository.StagingArea = await GetStagingArea();
        }

        private static async Task<StagingArea> GetStagingArea()
        {
            return await Utils.ReadObjectAsync<StagingArea>(Repository.StagingAreaPath);
        }

        public static async Task WriteFilesInStagingArea()
        {
            foreach (var (file, fileContentSha) in Repository.StagingArea.FilesStagedForAddition)
            {
                if (!Repository.StagingArea.FilesStagedForRemoval.ContainsKey(file))
                {
                    var fileBytes = await File.ReadAllBytesAsync(file);
                    var fileBytesSha = Utils.GetSha1(fileBytes);
                    await File.WriteAllBytesAsync(Path.Join(Repository.Files.FullName, fileBytesSha), fileBytes);
                }
            }
        }

        public static async Task SaveStagingArea()
        {
            await Utils.WriteObjectAndGetJson<StagingArea>(Repository.StagingAreaPath, Repository.StagingArea);
        }

            public static async Task ClearAndSaveStagingArea()
        {
            Repository.StagingArea.ClearStagingArea();
            await Utils.WriteObjectAndGetJson<StagingArea>(Repository.StagingAreaPath, Repository.StagingArea);
        }
    }
}