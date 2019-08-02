using System.Diagnostics;
using System.Windows.Input;

namespace metering
{
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
        public CommunicationViewModel( )
        {
            // create the command.
            ConnectCommand = new RelayCommand(param => ConnectOmicronAndUnit());
        }

        #endregion

        #region Helper Method

        /// <summary>
        /// connects to omicron and test unit.
        /// </summary>
        private void ConnectOmicronAndUnit()
        {
            //throw new NotImplementedException();
            // TODO: Handle Omicron connection here.
            // TODO: Handle Button checked and unchecked
            Debug.WriteLine("TODO: Connect Omicron Test Set ...");
            Debug.WriteLine($"TODO: Connect thru modbus protocol to {IpAddress}:{Port}");
        }

        #endregion

    }
}
