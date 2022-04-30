namespace MiniatureGit
{
    public class StagingArea
    {
        public Dictionary<string, string> FilesStagedForAddition { get; set; }
        public Dictionary<string, string> FilesStagedForRemoval { get; set; }

        public StagingArea()
        {
            FilesStagedForAddition = new Dictionary<string, string>();
            FilesStagedForRemoval = new Dictionary<string, string>();
        }

        public void AddFile(string filePath, string fileContentSha)
        {
            FilesStagedForAddition[filePath] = fileContentSha;
        }

        

        public void ClearStagingArea()
        {
            FilesStagedForAddition = new Dictionary<string, string>();
            FilesStagedForRemoval = new Dictionary<string, string>();
        }
    }
}