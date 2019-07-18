using metering.pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace metering.Commands
{
    public class AddNewTestCommand : ICommand
    {
        //NavigateService navigateService = new NavigateService();

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            // throw new NotImplementedException();
            // TODO: if anything would cause this command to be false
            return true;
        }

        public void Execute(object parameter)
        {
            // throw new NotImplementedException();
            Debug.WriteLine("AddNewTestCommand: params", parameter);
            //NavigateService.NavigateTo(typeof(VoltageTestPage));
        }
    }
}
