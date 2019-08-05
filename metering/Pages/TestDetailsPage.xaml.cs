using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// Interaction logic for TestDetailsPage.xaml
    /// </summary>
    public partial class TestDetailsPage : UserControl
    {
        public TestDetailsPage()
        {
            InitializeComponent();
                      
            DataContext = new TestDetailsViewModel();
        }
    }
}
