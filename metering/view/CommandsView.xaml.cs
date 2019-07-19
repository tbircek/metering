using metering.model;
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
        
        public CommandsView()
        {
            InitializeComponent();
        }

        private void NavigationService_Loading(object sender, NavigationEventArgs e)
        {
            Debug.WriteLine($"NavigationService_Loading -- NavigationEventArgs: {e.ExtraData.ToString()}");
        }

        private void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string str = (string)e.ExtraData;
            // deltaV1.Text = str;
            Debug.WriteLine($"NavigationService_LoadCompleted -- NavigationEventArgs: {e.ExtraData.ToString()}");

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var navigationService =  NavigationService.GetNavigationService(this);

            // Force WPF to download this page
            if (navigationService != null)
            {
                Debug.WriteLine("Can go forward");
                navigationService.Navigate(new Uri("\\view\\NominalValuesView.xaml", UriKind.Relative));
            }
        }

        private void AddNewTest_Click(object sender, RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);

            // Force WPF to download this page
            if (navigationService != null)
            {
                Debug.WriteLine("Can move backward");
                TestDetailsViewModel testView = new TestDetailsViewModel();
                NominalValues nominalValues = new NominalValues();
                TestDetail testDetail;

                // TODO: This value would be retrieve from Omicron Connection.
                int omicronVoltageOutputNumber = 4;
                for (int i = 1; i <= omicronVoltageOutputNumber; i++)
                {
                    testDetail = new TestDetail(signalName: "v" + i,
                                                from: nominalValues.NominalVoltage,
                                                to: nominalValues.NominalVoltage,
                                                delta: nominalValues.NominalDelta,
                                                phase: "0.00",
                                                frequency: nominalValues.NominalFrequency);
                    Debug.WriteLine($"testDetail: signalName: {testDetail.SignalName}\tfrom: {testDetail.From}\tto: {testDetail.To}\tdelta: {testDetail.Delta}\tphase: {testDetail.Phase}\tfrequency: {testDetail.Frequency}");
                }


                navigationService.Navigate(new Uri("\\view\\TestDetailsView.xaml", UriKind.Relative));
            }
        }
    }
}
