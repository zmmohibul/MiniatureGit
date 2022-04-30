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

        public static void FileChangedError(string filePath)
        {
            Console.WriteLine($"The file {filePath} has been modified since last staged or committed.");
            Console.WriteLine("Please stage it before making commit");
            Environment.Exit(1);
        }
    }
}