using System;
using System.Diagnostics;
using System.Threading.Tasks;
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

        /// <summary>
        /// ConnectCommand button content
        /// </summary>
        public string ConnectCommandContent { get; set; } = "Connect";

        /// <summary>
        /// a flag indicating Omicron Test Set connected and running
        /// </summary>
        public bool OmicronIsConnected { get; set; } = false;

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
            ConnectCommand = new RelayParameterizedCommand(async(parameter) => await ConnectOmicronAndUnit(parameter));

        }

        #endregion

        #region Helper Method

        /// <summary>
        /// connects to omicron and test unit.
        /// </summary>
        /// <param name="parameter">Attached self IsChecked property in the view</param>
        [DebuggerStepThrough]
        private async Task ConnectOmicronAndUnit(object parameter)
        {
            await RunCommand(() => OmicronIsConnected, async () =>
            {
                // waiting for the connections.
                // TODO: remove me later
                // await Task.Delay(5000);

                // Change Content of the button per isChecked parameter
                if ((bool)parameter)
                {
                    // get instance of Omicron Test Set
                    CMCControl cMCControl = new CMCControl();

                    // await omicron connection
                    bool isOmicronConnected = await Task.Factory.StartNew(()=> cMCControl.FindCMC());

                    // The user click on the button 
                    // TODO: Handle Omicron open connection here.
                    Debug.WriteLine($"TODO: Connect Omicron Test Set ... success?: {isOmicronConnected}");
                    Log += $"{DateTime.Now.ToLocalTime()}:\nConnecting Omicron Test Set was {(isOmicronConnected ? " successful" : " failed")}\n";

                    // TODO: Handle ConnectCommand Button checked
                    Debug.WriteLine($"TODO: Connect thru modbus protocol to {IpAddress}:{Port}");

                    // Change ConnectCommand Button content to "Disconnect"
                    ConnectCommandContent = isOmicronConnected?"Disconnect": "Connect";
                }
                else
                {
                    // The user wants to disconnect.
                    // TODO: Handle Omicron close connection here.
                    Debug.WriteLine("TODO: Disconnect Omicron Test Set ...");

                    // TODO: Handle ConnectCommand Button checked
                    Debug.WriteLine($"TODO: Disconnect modbus communication to {IpAddress}:{Port}");

                    // TODO: Verify disconnect was successful.

                    // Change ConnectCommand Button content to "Disconnect"
                    ConnectCommandContent = "Connect";
                }
            });
        }

        #endregion

    }
}
