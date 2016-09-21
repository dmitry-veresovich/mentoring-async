using System.Windows;
using Downloader.Infrastructure;
using Downloader.ViewModels;
using System.Windows.Forms;

namespace Downloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DownloadViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = Factory.Resolve<DownloadViewModel>();
            DataContext = _viewModel;
        }

        private void OnChooseSaveFolderButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _viewModel.SaveFolderPath = dialog.SelectedPath;
            }
        }
    }
}
