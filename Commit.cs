using System.Collections.Generic;

namespace MiniatureGit
{
    public class Commit
    {
        public string CommitMessage { get; set; }
        public DateTime CommittedAt { get; set; }
        public string Parent { get; set; }
        public Dictionary<string, string> FileNameFileShaDictionary { get; set; }

        public Commit()
        {
            CommitMessage = "Initial Commit";
            CommittedAt = DateTime.Now;
            Parent = string.Empty;
            FileNameFileShaDictionary = new Dictionary<string, string>();
        }
        
    }
}