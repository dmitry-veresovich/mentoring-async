using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Downloader.Services
{
    public class PageSaver : IPageSaver
    {
        public async Task<string> SavePageAsync(string folderPath, string page, CancellationTokenSource cancellationTokenSource)
        {
            var fileName = Path.ChangeExtension(Path.GetRandomFileName(), "html");
            var fullPath = Path.Combine(folderPath, fileName);
            var bytes = Encoding.UTF8.GetBytes(page);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await stream.WriteAsync(bytes, 0, bytes.Length, cancellationTokenSource.Token);
            }

            return fileName;
        }
    }
}
