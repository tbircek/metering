using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace metering.core
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
        public bool IsOmicronConnected { get; set; } = false;

        /// <summary>
        /// a flag indication UUT is connected and responding
        /// </summary>
        public bool IsUnitUnderTestConnected { get; set; } = false;

        /// <summary>
        /// a property to change Icon colors
        /// </summary>
        public string IconForeground { get; set; } = "Black";

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
            ConnectCommand = new RelayParameterizedCommand(async(parameter) => await StartCommunicationAsync(parameter));

        }

        #endregion

        #region Helper Method

        ModbusClient modbusClient;

        /// <summary>
        /// Starts a test with the values specified in Nominal Values page and
        /// Communication page.
        /// </summary>
        /// <param name="parameter">holding register starting address</param>
        public async Task StartCommunicationAsync(object parameter)
        {
            await RunCommand(() => IsUnitUnderTestConnected, async () =>
            {
                // open communication channel
                if (modbusClient == null)
                    modbusClient = new ModbusClient
                    {
                        IpAddress = this.IpAddress,
                        Port = Convert.ToInt32(this.Port),
                        ConnectionTimeout = 20000,
                    };

                if (!modbusClient.GetConnected())
                {
                    try
                    {
                        modbusClient.Connect();

                        // await if the server is connected
                        bool isUUTConnected = await Task.Factory.StartNew(() => modbusClient.GetConnected());

                        int[] response = modbusClient.ReadHoldingRegisters(Convert.ToInt32(parameter), 1);

                        for (int i = 0; i < response.Length; i++)
                        {
                            Debug.WriteLine($"Start Test is running: Register: {Convert.ToInt32(parameter) + i} reads {response[i]}");
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                else
                {
                    // TODO: Check if any 
                    modbusClient.Disconnect();

                    // Change Start Test Button color
                    IoC.Commands.StartTestForegroundColor = "00ff00";
                    Debug.WriteLine("Communication terminated.");
                }

            });
        }
        #endregion

    }
}
