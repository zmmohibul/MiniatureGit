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
            await File.WriteAllTextAsync(Head, Master);
        }

        public static async Task<Commit> GetHeadCommit()
        {
            var head = await File.ReadAllTextAsync(Repository.Head);
            var headCommitSha = await File.ReadAllTextAsync(head);
            return await Utils.ReadObjectAsync<Commit>(Path.Join(Repository.Commits.FullName, headCommitSha));
        }

        public static async Task SetupStagingArea()
        {
            StagingArea = await GetStagingArea();
        }

        private static async Task<StagingArea> GetStagingArea()
        {
            return await Utils.ReadObjectAsync<StagingArea>(StagingAreaPath);
        }

        public static async Task SaveStagingArea()
        {
            await Utils.WriteObjectAndGetJson<StagingArea>(StagingAreaPath, StagingArea);
        }
    }
}