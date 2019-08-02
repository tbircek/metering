using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// Interaction logic for CommunicationView.xaml
    /// </summary>
    public partial class CommunicationView : UserControl
    {
        public CommunicationView()
        {
            InitializeComponent();
            DataContext = new CommunicationViewModel();
        }
    }
}
