using System.Diagnostics;
using System.Windows.Input;
using PropertyChanged;

namespace metering
{
    [AddINotifyPropertyChangedInterface]
    public class CommunicationViewModel : BaseViewModel
    {
        // TODO: Add a model for "Connect" button 
        // TODO: squirrel.windows update tool

        #region Public Properties


        /// <summary>
        /// ipaddress of the test unit.
        /// </summary>
        public string IpAddress { get; set; } = "192.168.0.122";

        /// <summary>
        /// port number of communication port
        /// </summary>
        public string Port { get; set; } = "502";

        /// <summary>
        /// Holds info about ipaddress:port, connected Omicron Serial # and etc.
        /// </summary>
        public string Log { get; set; } = "";

        #endregion

        #region Public Commands

        /// <summary>
        /// command to provide connection to both Attached Omicron and 
        /// specified Test Unit ipaddress and port.
        /// </summary>
        public ICommand ConnectCommand { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public CommunicationViewModel( ) //string ipAddress, string port, string log, string omicron)
        {
            // create the command.
            ConnectCommand = new RelayCommand(ConnectOmicronAndUnit);

            // set ipaddress, port and log
            //IpAddress = ipAddress;
            //Port = port;
            //Log = log;

            // TODO: Handle Omicron connection here.
        }

        #endregion

        #region Helper Method

        /// <summary>
        /// connects to omicron and test unit.
        /// </summary>
        private void ConnectOmicronAndUnit()
        {
            //throw new NotImplementedException();
            Debug.WriteLine("TODO: Connect Omicron Test Set ...");
            Debug.WriteLine($"TODO: Connect thru modbus protocol to {IpAddress}:{Port}");
        }

        #endregion

    }
}
