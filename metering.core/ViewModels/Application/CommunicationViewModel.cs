using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace metering.core
{
    public class CommunicationViewModel : BaseViewModel
    {
        // TODO: Add a model for "Connect" button 
        // TODO: squirrel.windows update tool

        #region Public Properties

        /// <summary>
        /// Modbus Client for all modbus protocol communication
        /// </summary>
        public ModbusClient ModbusClient { get; set; }
        
        /// <summary>
        /// Omicron CMC Engine
        /// </summary>
        public CMCControl CMCControl { get; set; }
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

        #endregion

        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public CommunicationViewModel()
        {

        }

        #endregion

        #region Helper Method

        /// <summary>
        /// Starts a test with the values specified in Nominal Values page and
        /// Communication page.
        /// </summary>
        /// <param name="parameter">holding register starting address</param>
        public async Task StartCommunicationAsync(object parameter)
        {
            if (Convert.ToInt32(IoC.TestDetails.Register) == 0)
            {
                await Task.Run(() => MessageBox.Show("Invalid Register"));
                IsUnitUnderTestConnected = true;

                // Change Start Test Button color
                IoC.Commands.StartTestForegroundColor = "00ff00";
            }

            await RunCommand(() => IsUnitUnderTestConnected, async () =>
            {

                // open communication channel
                if (ModbusClient == null)
                    ModbusClient = new ModbusClient
                    {
                        IpAddress = this.IpAddress,
                        Port = Convert.ToInt32(this.Port),
                        ConnectionTimeout = 20000,
                    };

                if (!ModbusClient.GetConnected())
                {
                    try
                    {
                        ModbusClient.Connect();

                        // await if the server is connected
                        bool isUUTConnected = await Task.Factory.StartNew(() => ModbusClient.GetConnected());

                        int[] response = ModbusClient.ReadHoldingRegisters(Convert.ToInt32(IoC.TestDetails.Register), 1);

                        for (int i = 0; i < response.Length; i++)
                        {
                            Debug.WriteLine($"Start Test is running: Register: {Convert.ToInt32(IoC.TestDetails.Register) + i} reads {response[i]}");
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
                    ModbusClient.Disconnect();

                    // Change Start Test Button color
                    IoC.Commands.StartTestForegroundColor = "00ff00";

                    Debug.WriteLine("Communication terminated.");
                }

            });
        }
        #endregion

    }
}
