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
            if (navigationService != null)
            {
                Debug.WriteLine("Can go forward");
                navigationService.Navigate(new Uri("\\view\\NominalValuesView.xaml", UriKind.Relative));
            }
        }

        //private void AddNewTest_Click(object sender, RoutedEventArgs e)
        //{
        //    var navigationService = NavigationService.GetNavigationService(this);

        //    // Force WPF to download this page
        //    if (navigationService != null)
        //    {
        //        Debug.WriteLine("Can move backward");
        //        var test = new TestDetailsViewModel().Test;

        //        // NominalValuesViewModel nominalValues = new NominalValuesViewModel();

        //        // TODO: This value would be retrieve from Omicron Connection.
        //        int omicronVoltageOutputNumber = 4;
        //        for (int i = 1; i <= omicronVoltageOutputNumber; i++)
        //        {
        //            test = new model.TestDetail(signalName: "v" + i,
        //                                        from: nominalValues.Voltage,
        //                                        to: nominalValues.Voltage,
        //                                        delta: nominalValues.Delta,
        //                                        phase: "0.00",
        //                                        frequency: nominalValues.Frequency);
                    
        //            // testView.S("v" + i, nominalValues.Voltage, nominalValues.Voltage, nominalValues.Delta, "0.00", nominalValues.Frequency);
        //            Debug.WriteLine($"testDetail: signalName: {test.SignalName}\tfrom: {test.From}\tto: {test.To}\tdelta: {test.Delta}\tphase: {test.Phase}\tfrequency: {test.Frequency}");
        //        }


        //        navigationService.Navigate(new Uri("\\view\\TestDetailsView.xaml", UriKind.Relative));
        //    }
        //}
    }
}
