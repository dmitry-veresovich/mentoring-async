using System.Threading;
using System.Threading.Tasks;

namespace Downloader.Services
{
    public interface IPageSaver
    {
        Task<string> SavePageAsync(string folderPath, string page, CancellationTokenSource cancellationTokenSource);
    }
}
