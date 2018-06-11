using System.Windows;


namespace Client
{
    public partial class UploadPage
    {
        public UploadPage()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            var navigationService = NavigationService;
            navigationService?.Navigate(new TabResult());
        }
    }
}
