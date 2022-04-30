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
            else
            {
                Console.WriteLine($"No command '{firstArgument}' exists.");
                Environment.Exit(1);
            }
        }
        
    }
}