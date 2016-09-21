using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Downloader.Services
{
    public class PageLoader : IPageLoader
    {
        public async Task<string> LoadPageAsync(string url, CancellationTokenSource cancellationTokenSource)
        {
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url, cancellationTokenSource.Token);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
