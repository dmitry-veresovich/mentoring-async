namespace Downloader.Services
{
    public interface IValidationService
    {
        bool IsUrlValid(string url);
        bool IsFolderPathValid(string path);
    }
}
