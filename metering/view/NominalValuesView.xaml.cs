using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class NominalValuesView : UserControl
    {
        public NominalValuesView()
        {
            InitializeComponent();
            DataContext = new NominalValuesViewModel();
        }
    }
}
