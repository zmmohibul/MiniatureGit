namespace MiniatureGit
{
    public class ErrorLog
    {
        public static void CheckNoFileExistError(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"No file {filePath} exists in present working directory");
                Environment.Exit(1);
            }
        }
    }
}