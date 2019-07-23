using System.Diagnostics;
using System.Windows.Input;
using metering.model;

namespace metering.viewModel
{
    public class CommunicationViewModel : ViewModelBase
    {
        // TODO: Add a model for "Connect" button 
        // TODO: squirrel.windows update tool

        private static CommunicationModel model = new CommunicationModel();
        DelegateCommand connectCommand;


        public string IpAddress
        {
            get => model.IpAddress;
            set
            {
                if (SetProperty(model.IpAddress, value))
                {
                    model.IpAddress = value;
                }
            }
        }
               
        public string Port
        {
            get => model.Port;
            set
            {
                if (SetProperty(model.Port, value))
                {
                    model.Port = value;
                }
            }
        }
                
        public string Log
        {
            get => model.Log;
            set
            {
                if (SetProperty(model.Log, value))
                {
                    model.Log = value;
                }
            }
        }

        public ICommand ConnectCommand
        {
            get
            {
                if (connectCommand == null)
                {
                    connectCommand = new DelegateCommand(
                        param => ConnectOmicronAndUnit(),
                        param => true
                        );
                }
                return connectCommand;
            }
        }

        private void ConnectOmicronAndUnit()
        {
            //throw new NotImplementedException();
            Debug.WriteLine("TODO: Connect Omicron Test Set ...");
            Debug.WriteLine($"TODO: Connect thru modbus protocol to {model.IpAddress}:{model.Port}");
        }
    }
}
