namespace API.Helpers
{
    public class FileRepoSettings
    {
        public FileRepoSettings() {
            
        }
        public FileRepoSettings(string repoDirectory, string photosDirectory, string documentsDirectory, string userFilesDirectory)
        {
            this.RepoDirectory = repoDirectory;
            this.PhotosDirectory = photosDirectory;
            this.DocumentsDirectory = documentsDirectory;
            this.UserFilesDirectory = userFilesDirectory;

        }
        public string RepoDirectory { get; set; }
        public string PhotosDirectory { get; set; }
        public string DocumentsDirectory { get; set; }
        public string UserFilesDirectory { get; set; }
    }
}