using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
//using AdaptiveCards;
//using AdaptiveCards.Rendering.Wpf;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using metering.Resources;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace metering.pages
{
    /// <summary>
    /// Interaction logic for testPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();


            //// Can only access the NavigationService when the page has been loaded
            //Loaded += new RoutedEventHandler(CancelNavigationPage_Loaded);
            //Unloaded += new RoutedEventHandler(CancelNavigationPage_Unloaded);

            // Don't allow back navigation if no navigation service
            if (NavigationService != null)
            {
                AddNewTest.IsEnabled = false;

            }
        }
        
        private void AddNewTest_Click(object sender, RoutedEventArgs e)
        {
            
            // Force WPF to download this page
            Debug.WriteLine($"AddNewTest_Click -- RoutedEventArgs: {globalNominalDelta.Text}");
            NavigationService.Navigate(new Uri("\\pages\\VoltageTestPage.xaml", UriKind.Relative), globalNominalDelta.Text);
        }
    }
}
