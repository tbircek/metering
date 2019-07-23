using System.Windows.Input;

namespace metering.viewModel
{
    public class CommandsViewModel: ViewModelBase
    {
        public CommandsViewModel(ICommand newTestCommand)
        {            
            AddNewTest = newTestCommand;
        }

        public ICommand AddNewTest { get; private set; }    
    }
}
