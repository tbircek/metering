using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace metering.pages
{
    /// <summary>
    /// Interaction logic for VoltageTestPage.xaml
    /// </summary>
    public partial class VoltageTestPage : Page
    {
        public VoltageTestPage()
        {
            InitializeComponent();

            //if (NavigationService != null)
            //{
            // Can only access the NavigationService when the page has been loaded
            //NavigationService.LoadCompleted += NavigationService_LoadCompleted;
            //    //Unloaded += new RoutedEventHandler(TestPage_Unloaded);
            //}
            //else
            //{
            //    Debug.WriteLine("NavigationService --- null ");
            //}

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
            // Force WPF to download this page
            if (NavigationService != null)
            {
                Debug.WriteLine("Can go back");
                NavigationService.Navigate(new Uri("\\pages\\MainPage.xaml", UriKind.Relative));
            }            
        }
    }
}
