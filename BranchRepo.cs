namespace MiniatureGit
{
    public class BranchRepo
    {
        public static async Task Checkout(string branchName)
        {
            if (BranchExists(branchName))
            {
                var branchCommitId = await File.ReadAllTextAsync(Path.Join(Repository.Branches.FullName, branchName));
                await CommitRepo.Checkout(branchCommitId);
            }
            else
            {
                Console.WriteLine($"No branch {branchName} exists.");
                Environment.Exit(1);
            }
        }

        public static bool BranchExists(string branchName)
        {
            var branches = Directory.GetFiles(Repository.Branches.FullName);
            foreach (var branch in branches)
            {
                if (branch.Equals(Path.Join(Repository.Branches.FullName, branchName)))
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<string> GetCurrentBranchPointer()
        {
            var currentBranch = await File.ReadAllTextAsync(Repository.CurrentBranch);
            return await File.ReadAllTextAsync(currentBranch);
        }

        public static async Task ChangeCurrentBranchPointer(string commitId)
        {
            var currentBranch = await File.ReadAllTextAsync(Repository.CurrentBranch);
            await File.WriteAllTextAsync(currentBranch, commitId);
        }
    }
}