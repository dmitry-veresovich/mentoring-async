namespace Downloader.Services
{
    public static class Constants
    {
        public static class ValidationErrors
        {
            public const string UrlNotValid = "Url is not valid";
            public const string FolderPathNotValid = "Folder path is not valid";
        }

        public static class Messages
        {
            public const string Downloading = "Downloading...";
            public const string Saving = "Saving...";
            public const string SucessfullyDownloadedAndSavedFormat = "Successfuly downloaded and saved, file name: {0}";
            public const string NotStarted = "Download didn't start and was cancelled";
            public const string Cancelled = "Download started but was cancelled";
        }
    }
}
