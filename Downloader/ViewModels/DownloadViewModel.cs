using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Downloader.Infrastructure;
using Downloader.Services;

namespace Downloader.ViewModels
{
    public class DownloadViewModel : INotifyPropertyChanged
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isDownloading = false;
        private string _saveFolderPath;
        private string _validationError;
        private readonly IValidationService _validationService;
        private readonly IPageLoader _pageLoader;
        private readonly IPageSaver _pageSaver;

        public DownloadViewModel(
            IValidationService validationService,
            IPageLoader pageLoader,
            IPageSaver pageSaver)
        {
            _validationService = validationService;
            _pageLoader = pageLoader;
            _pageSaver = pageSaver;
            Sources = new ObservableCollection<SourceViewModel> {new SourceViewModel()};
            StartDownloadCommand = new Command(StartDownload);
            CancelDownloadCommand = new Command(CancelDownload);
            AddSourceCommand = new Command(AddSource);
        }

        public ObservableCollection<SourceViewModel> Sources { get; }

        public bool IsDownloading
        {
            get { return _isDownloading; }
            private set
            {
                if (_isDownloading == value) return;
                _isDownloading = value;
                OnPropertyChanged();
            }
        }

        public string SaveFolderPath
        {
            get { return _saveFolderPath; }
            set
            {
                if (_saveFolderPath == value) return;
                _saveFolderPath = value;
                OnPropertyChanged();
            }
        }

        public string ValidationError
        {
            get { return _validationError; }
            set
            {
                if (_validationError == value) return;
                _validationError = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartDownloadCommand { get; set; }
        public ICommand CancelDownloadCommand { get; set; }
        public ICommand AddSourceCommand { get; set; }

        private async void StartDownload()
        {
            ResetStatusesAndMessages();

            if (!_validationService.IsFolderPathValid(SaveFolderPath))
            {
                ValidationError = Constants.ValidationErrors.FolderPathNotValid;
                return;
            }

            IsDownloading = true;
            _cancellationTokenSource = new CancellationTokenSource();

            foreach (var source in Sources)
            {
                if (!source.HasUrl) continue;

                if (_validationService.IsUrlValid(source.Url))
                {
                    try
                    {
                        if (_cancellationTokenSource.IsCancellationRequested) continue;
                        SetSourceStatusAndMessage(source, DownloadStatus.Downloading, Constants.Messages.Downloading);
                        var result = await _pageLoader.LoadPageAsync(source.Url, _cancellationTokenSource);

                        if (_cancellationTokenSource.IsCancellationRequested) continue;
                        SetSourceStatusAndMessage(source, DownloadStatus.Saving, Constants.Messages.Saving);
                        var fileName = await _pageSaver.SavePageAsync(SaveFolderPath, result, _cancellationTokenSource);

                        SetSourceStatusAndMessage(source, DownloadStatus.Finished, string.Format(Constants.Messages.SucessfullyDownloadedAndSavedFormat, fileName));
                    }
                    catch (OperationCanceledException)
                    {
                        // Downloading was cancelled so nothing is required to do.
                    }
                    catch (Exception e)
                    {
                        source.Message = e.ToString();
                    }
                }
                else
                {
                    source.Message = Constants.ValidationErrors.UrlNotValid;
                }
            }

            IsDownloading = false;
        }

        private void SetSourceStatusAndMessage(SourceViewModel source, DownloadStatus status, string message)
        {
            source.Status = status;
            source.Message = message;
        }

        private void ResetStatusesAndMessages()
        {
            ValidationError = string.Empty;
            foreach (var source in Sources)
            {
                source.Status = DownloadStatus.NotStarted;
                source.Message = string.Empty;
            }
        }

        private void CancelDownload()
        {
            _cancellationTokenSource?.Cancel();
            foreach (var source in Sources.Where(s => s.HasUrl))
            {
                switch (source.Status)
                {
                    case DownloadStatus.NotStarted:
                        source.Message = Constants.Messages.NotStarted;
                        break;
                    case DownloadStatus.Downloading:
                    case DownloadStatus.Saving:
                        source.Message = Constants.Messages.Cancelled;
                        break;
                }
            }
            IsDownloading = false;
        }

        private void AddSource()
        {
            Sources.Add(new SourceViewModel());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
