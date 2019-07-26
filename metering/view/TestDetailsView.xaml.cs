using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace metering.view
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestDetailsView : UserControl
    {
        public TestDetailsView()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);

            // Force WPF to download this page
            if (navigationService.CanGoBack) // != null)
            {
                Debug.WriteLine("Can go forward...");
                navigationService.GoBack(); // (new Uri("\\MainWindow.xaml", UriKind.Relative));
            }
        }
    }
}
