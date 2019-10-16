using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace metering.core
{
    /// <summary>
    /// Handles Communication page.
    /// </summary>
    public class CommunicationViewModel : BaseViewModel
    {
        // TODO: use dependency injection for EasyModbus library

        #region Private Variables

        /// <summary>
        /// Holds the user information
        /// </summary>
        private StringBuilder logBuilder = new StringBuilder();


        /// <summary>
        /// thread lock object
        /// </summary>
        private readonly object lockObject = new object();

        #endregion

        #region Public Properties

        /// <summary>
        /// ModbusClient for modbus protocol communication.
        /// </summary>
        public EasyModbus.ModbusClient EAModbusClient { get; set; }

        /// <summary>
        /// Hint value for IpAddress textbox
        /// </summary>
        public string IpAddressHint { get; set; } = Resources.Strings.tab_home_ipaddress;

        /// <summary>
        /// IpAddress of the test unit.
        /// </summary>
        public string IpAddress { get; set; } = "192.168.0.122";

        /// <summary>
        /// Hint value for Port textbox
        /// </summary>
        public string PortHint { get; set; } = Resources.Strings.tab_home_port;

        /// <summary>
        /// port number of communication port
        /// </summary>
        public string Port { get; set; } = "502";

        /// <summary>
        /// Holds info about IpAddress:port, connected Omicron Serial # and etc.
        /// </summary>
        public string Log
        {
            // return StringBuilder holds log information
            get
            {
                // lock the thread
                lock (lockObject)
                {
                    // return log builder's string
                    return logBuilder.ToString();
                }
            }

            // update the string with a line.
            set
            {
                // lock the thread
                lock (lockObject)
                {
                    // set log builder's text
                    logBuilder.AppendLine(value);
                }
            }
        }

        /// <summary>
        /// a flag indicating Omicron Test Set connected and running
        /// </summary>
        public bool IsOmicronConnected { get; set; } = false;

        /// <summary>
        /// a flag indication UUT is connected and responding
        /// </summary>
        public bool IsUnitUnderTestConnected { get; set; } = false;

        /// <summary>
        /// indicates if the current text double left clicked to highlight the text
        /// </summary>
        public bool Selected { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Selects all text on left clicked text box.
        /// </summary>
        public ICommand SelectAllTextCommand { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public CommunicationViewModel()
        {

            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            // inform the user Application started.
            Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Application Starts.";

            // create command
            SelectAllTextCommand = new RelayCommand(SelectAll);
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Selects all text on the text box
        /// </summary>
        public void SelectAll()
        {
            // simulate property change briefly to select all text in the text box
            // as selecting all text should be last until the user leaves the control or types something

            // Sets FocusAndSelectProperty to true
            Selected = true;

            // Sets FocusAndSelectProperty to false
            Selected = false;
        }

        /// <summary>
        /// Starts a test with the values specified in Nominal Values page and
        /// Communication page.
        /// </summary>
        public async Task StartCommunicationAsync()
        {
            // start point of all test steps with the first mouse click and it will ignore subsequent mouse clicks
            await AsyncAwaiter.AwaitAsync(nameof(StartCommunicationAsync), async () =>
            {
                try
                {
                    // update the user
                    Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Communication starts.";

                    // get new construct of ModbusClient
                    EAModbusClient = new EasyModbus.ModbusClient
                    {
                        IPAddress = IpAddress,
                        Port = Convert.ToInt32(Port),
                        ConnectionTimeout = 20000,
                        // LogFileFilename = @"C:\Users\TBircek\Documents\metering\modbus.log"
                    };

                    // Checks if the Server IPAddress is available
                    if (EAModbusClient.Available(20000))
                    {
                        // connect to the server
                        EAModbusClient.Connect();

                        // find any CMCEngine attached to this computer
                        if (IoC.FindCMC.Find())
                        {
                            // perform initial set up on CMCEngine
                            IoC.InitialCMCSetup.InitialSetup();

                            // Is there Omicron Test Set attached to this app?
                            if (IoC.CMCControl.DeviceID > 0)
                            {
                                // indicates the test is running.
                                IoC.CMCControl.IsTestRunning = true;

                                // there is a test set attached so run specified tests.
                                await IoC.CMCControl.TestAsync();
                            }
                            else
                            {
                                // inform the user 
                                Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Failed: Omicron Test Set ID is a zero.";
                            }
                        }
                        else
                        {
                            //// inform the developer
                            //IoC.Logger.Log("Find no Omicron");

                            // inform the user 
                            Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Failed: There is no attached Omicron Test Set. Please attached a Omicron Test Set before test.";
                        }
                    }
                    else
                    {
                        //// inform the developer
                        //IoC.Logger.Log($"The server {EAModbusClient.IPAddress} is not available.");

                        // inform the user 
                        Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Failed: The server is not available: {EAModbusClient.IPAddress}.";
                    }
                }
                catch (Exception ex)
                {
                    //// inform the developer about error.
                    //IoC.Logger.Log(ex.Message);

                    // inform the user about error.
                    Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Start Communication failed: {ex.Message}.";

                    // catch inner exceptions if exists
                    if (ex.InnerException != null)
                    {
                        // inform the user about more details about error.
                        Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.";
                    }
                }
            });
        }
        #endregion

    }
}
