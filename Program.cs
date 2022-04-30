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

            var firstArgument = args[0];
            if (firstArgument.Equals("init"))
            {
                await Repository.Init();
            }
            else
            {
                Console.WriteLine($"No command '{firstArgument}' exists.");
                Environment.Exit(1);
           }
        }
        
    }
}