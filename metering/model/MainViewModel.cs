using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;


namespace metering.model
{
    public class MainViewModel
    {
        // NavigationService navigationService = NavigationService.GetNavigationService(MainViewModel);
       public MainViewModel()
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

        //    CommandBinding customCommandBinding = new CommandBinding(
   // CustomRoutedCommand, ExecutedCustomCommand, CanExecuteCustomCommand);

            // attach CommandBinding to root window
     //       this.CommandBindings.Add(customCommandBinding);

            // CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding());

            //CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(Commands.BrowseBack, GoBackExecuted));

            //CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(Commands.GoForwardCommand, GoForwardExecuted));

            // CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(System.Windows.Input.Commands.BrowseForward, GoForwardExecuted));
        }

        private void GoForwardExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // Force WPF to download this page
            //Debug.WriteLine($"AddNewTest_Click -- RoutedEventArgs: {Voltage}");
            // NavigationService.Navigate(new Uri("\\pages\\VoltageTestPage.xaml", UriKind.Relative)); //, globalNominalDelta.Text);
            // NavigationService.Navigate(new Uri("\\pages\\VoltageTestPage.xaml", UriKind.Relative));
        }

        private void GoBackExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddNewTest(object sender, ExecutedRoutedEventArgs e)
        {
            Debug.WriteLine($"AddNewTest_Click -- RoutedEventArgs: ");
            // NavigationService.Navigate(new Uri("\\pages\\VoltageTestPage.xaml", UriKind.Relative)); //, globalNominalDelta.Text);
            
        }
    }
}
