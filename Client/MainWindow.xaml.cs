using System.Windows;
using System.Windows.Navigation;

namespace Client
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService;
            navigationService?.Navigate(new UploadPage());
        }
    }
}