using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Downloader.ViewModels
{
    public class SourceViewModel : INotifyPropertyChanged
    {
        private string _url;
        private string _message;

        public string Url
        {
            get { return _url; }
            set
            {
                if (_url == value) return;
                _url = value;
                OnPropertyChanged();
            }
        }

        public bool HasUrl => !string.IsNullOrWhiteSpace(Url);

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message == value) return;
                _message = value;
                OnPropertyChanged();
            }
        }

        public DownloadStatus Status { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
