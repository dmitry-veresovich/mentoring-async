using System.Threading;
using System.Threading.Tasks;

namespace Downloader.Services
{
    public interface IPageLoader
    {
        Task<string> LoadPageAsync(string url, CancellationTokenSource cancellationTokenSource);
    }
}
