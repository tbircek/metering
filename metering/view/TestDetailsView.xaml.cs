using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace metering.view
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        public TestView()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);

            // Force WPF to download this page
            if (navigationService != null)
            {
                Debug.WriteLine("Can go back");
                navigationService.GoBack(); // (new Uri("\\MainWindow.xaml", UriKind.Relative));
            }
        }
    }
}
