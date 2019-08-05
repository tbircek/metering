using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// Interaction logic for CommandsView.xaml
    /// </summary>
    public partial class CommandsView : UserControl
    {
        public CommandsView()
        {
            InitializeComponent();
            DataContext = new CommandsViewModel();
        }
    }
}
