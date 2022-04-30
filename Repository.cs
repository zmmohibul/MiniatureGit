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

        public static async Task Init()
        {
            MiniatureGit.Create();
            Files.Create();
            Commits.Create();
            Branches.Create();

            var initialCommt = new Commit();
            var initialCommtHash = await Utils.WriteObjectAndGetObjectHashAsync<Commit>(initialCommt, Commits.FullName);

            await File.WriteAllTextAsync(Master, initialCommtHash);
            await File.WriteAllTextAsync(Head, Master);
        }
    }
}