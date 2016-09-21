using System;
using System.IO;

namespace Downloader.Services
{
    public class ValidationService : IValidationService
    {
        public bool IsUrlValid(string url)
        {
            Uri uriResult;
            var result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return result;
        }

        public bool IsFolderPathValid(string path)
        {
            return Directory.Exists(path);
        }
    }
}
