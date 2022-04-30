using System;
namespace MiniatureGit
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please enter an argument");
                Environment.Exit(1);
            }

            var firstArgument = args[0].ToLower();
            
            if (!firstArgument.Equals("init") && !Directory.Exists(".minigit"))
            {
                Console.WriteLine("This is not an initialized MiniatureGit repository.");
                Environment.Exit(1);
            }
            
            if (Directory.Exists(Repository.MiniatureGit.FullName))
            {
                await Repository.SetupStagingArea();
            }

            if (firstArgument.Equals("init"))
            {
                await Repository.Init();
            }
            else if (firstArgument.Equals("add"))
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Please enter a file name for staging files.");
                    Environment.Exit(1);
                }

                var filePath = args[1];
                await StageRepo.AddFileToStagingArea(filePath);
            }
            else if (firstArgument.Equals("commit"))
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Please enter a commit message.");
                    Environment.Exit(1);
                }

                await CommitRepo.MakeCommit(args[1]);
            }
            else if (firstArgument.Equals("rm"))
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Please enter a file name for removing files.");
                    Environment.Exit(1);
                }

                await StageRepo.RemoveFileFromStagingArea(args[1]);
            }
            else if (firstArgument.Equals("checkout"))
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Please enter a commit id to checkout.");
                    Environment.Exit(1);
                }

                await CommitRepo.Checkout(args[1]);
            }
            else
            {
                Console.WriteLine($"No command '{firstArgument}' exists.");
                Environment.Exit(1);
            }
        }
        
    }
}