using System.Windows.Input;

namespace metering.viewModel
{
    public class CommunicationViewModel : ViewModelBase
    {
        private string _ipAddress = "192.168.0.122";
        public string IpAddress
        {
            get => _ipAddress;
            set => SetProperty(ref _ipAddress, value);
        }

        private string _port = "502";
        public string Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        private string _log;
        public string Log
        {
            get => _log;
            set => SetProperty(ref _log, value);
        }

        private readonly DelegateCommand _connectCommand;
        public ICommand ConnectCommand => _connectCommand;

        public CommunicationViewModel()
        {
            _connectCommand = new DelegateCommand(OnConnect, CanConnect);
        }

        private bool CanConnect(object arg)
        {
            return IpAddress != "testing command";
        }

        private void OnConnect(object obj)
        {
            IpAddress = "testing command";
            _connectCommand.InvokeCanExecuteChanged();
        }
    }
}
