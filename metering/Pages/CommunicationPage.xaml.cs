using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// Interaction logic for CommunicationPage.xaml
    /// </summary>
    public partial class CommunicationPage : Page
    {
        public CommunicationPage()
        {
            InitializeComponent();
            DataContext = new CommunicationViewModel();
        }
    }
}
