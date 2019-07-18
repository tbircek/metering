using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using metering.model;

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
    }
}
