using metering.viewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace metering.view
{
    /// <summary>
    /// Interaction logic for CommandsView.xaml
    /// </summary>
    public partial class CommandsView : UserControl
    {
        //private NominalValuesViewModel nominalValues;
        public CommandsView()
        {
            //nominalValues = new NominalValuesViewModel();
            InitializeComponent();
        }

        //private void NavigationService_Loading(object sender, NavigationEventArgs e)
        //{
        //    Debug.WriteLine($"NavigationService_Loading -- NavigationEventArgs: {e.ExtraData.ToString()}");
        //}

        //private void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        //{
        //    string str = (string)e.ExtraData;
        //    // deltaV1.Text = str;
        //    Debug.WriteLine($"NavigationService_LoadCompleted -- NavigationEventArgs: {e.ExtraData.ToString()}");

        //}


        // TODO: Move in to MVVM framework
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var navigationService =  NavigationService.GetNavigationService(this);

            // Force WPF to download this page
            //if (navigationService != null)
            if (navigationService.CanGoBack)
            {
                Debug.WriteLine("Can go forward");
                navigationService.Navigate(new Uri("\\view\\NominalValuesView.xaml", UriKind.Relative));
            }
        }

        // it is not pure mvvm but it works.
        private void AddNewTest_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);

            // Force WPF to download this page
            if (navigationService != null)
            {
                Debug.WriteLine("Can move backward");                
               
                navigationService.Navigate(new Uri("\\view\\TestDetailsView.xaml", UriKind.Relative));
                // navigationService.AddBackEntry(state);
            }
        }
    }
}
