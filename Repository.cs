namespace MiniatureGit
{
    public class Repository
    {
        public static readonly DirectoryInfo PWD = new DirectoryInfo(".");
        public static readonly DirectoryInfo MiniatureGit = new DirectoryInfo(Path.Join(PWD.FullName, ".minigit"));
        public static readonly DirectoryInfo Files = new DirectoryInfo(Path.Join(MiniatureGit.FullName, "files"));
        public static readonly DirectoryInfo Commits = new DirectoryInfo(Path.Join(MiniatureGit.FullName, "commits"));
        public static readonly DirectoryInfo Branches = new DirectoryInfo(Path.Join(MiniatureGit.FullName, "branches"));
        public static readonly string Head = Path.Join(MiniatureGit.FullName, "HEAD");
        public static readonly string CurrentBranch = Path.Join(MiniatureGit.FullName, "CurrentBranch");
        public static readonly string Master = Path.Join(Branches.FullName, "master");
        public static readonly string StagingAreaPath = Path.Join(MiniatureGit.FullName, "StagingArea");
        public static StagingArea StagingArea;

        public static async Task Init()
        {
            if (Directory.Exists(MiniatureGit.FullName))
            {
                Console.WriteLine("This is an already initialized MiniatureGit repository.");
                Environment.Exit(1);
            }

            MiniatureGit.Create();
            Files.Create();
            Commits.Create();
            Branches.Create();

            var initialCommt = new Commit();
            var initialCommtHash = await Utils.WriteObjectAndGetObjectHashAsync<Commit>(Commits.FullName, initialCommt);

            StagingArea = new StagingArea();
            await Utils.WriteObjectAndGetJson<StagingArea>(StagingAreaPath, StagingArea);

            await File.WriteAllTextAsync(Master, initialCommtHash);
            await File.WriteAllTextAsync(Head, initialCommtHash);
            await File.WriteAllTextAsync(CurrentBranch, Master);
        }

        public static async Task<Commit> GetHeadCommit()
        {
            var headSha = await File.ReadAllTextAsync(Repository.Head);
            return await Utils.ReadObjectAsync<Commit>(Path.Join(Repository.Commits.FullName, headSha));
        }

        public static async Task SetupStagingArea()
        {
            StagingArea = await GetStagingArea();
        }

        private static async Task<StagingArea> GetStagingArea()
        {
            return await Utils.ReadObjectAsync<StagingArea>(StagingAreaPath);
        }

        public static async Task WriteFilesInStagingArea()
        {
            foreach (var (file, fileContentSha) in StagingArea.FilesStagedForAddition)
            {
                if (!StagingArea.FilesStagedForRemoval.ContainsKey(file))
                {
                    var fileBytes = await File.ReadAllBytesAsync(file);
                    var fileBytesSha = Utils.GetSha1(fileBytes);
                    await File.WriteAllBytesAsync(Path.Join(Files.FullName, fileBytesSha), fileBytes);
                }
            }
        }

        public static async Task ChangeHeadPointer(string commitId)
        {
            await File.WriteAllTextAsync(Head, commitId);
        }

        public static async Task ChangeCurrentBranchPointer(string commitId)
        {
            var currentBranch = await File.ReadAllTextAsync(CurrentBranch);
            await File.WriteAllTextAsync(currentBranch, commitId);
        }

        public static async Task SaveStagingArea()
        {
            await Utils.WriteObjectAndGetJson<StagingArea>(StagingAreaPath, StagingArea);
        }

        public static async Task ClearAndSaveStagingArea()
        {
            Repository.StagingArea.ClearStagingArea();
            await Utils.WriteObjectAndGetJson<StagingArea>(StagingAreaPath, StagingArea);
        }
    }
}