using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class NominalValuesPage : UserControl
    {
        public NominalValuesPage()
        {
            InitializeComponent();
            DataContext = new NominalValuesViewModel();
        }
    }
}
