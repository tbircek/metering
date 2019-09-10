using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using OMICRON.CMEngAL;

namespace metering.core
{
    public class CommunicationViewModel : BaseViewModel
    {
        // TODO: squirrel.windows update tool
        // TODO: Remove ModbusClient to its own project
        // TODO: Remove CMCControl to its own project
        #region Public Properties

        /// <summary>
        /// Modbus Client for all modbus protocol communication
        /// </summary>
        // public ModbusClient ModbusClient { get; set; }

        /// <summary>
        /// ModbusClient for modbus protocol communication.
        /// </summary>
        public EasyModbus.ModbusClient EAModbusClient { get; set; }

        /// <summary>
        /// Omicron CMC Engine
        /// </summary>
        public CMCControl CMCControl { get; set; }

        /// <summary>
        /// Hint value for Ip Address textbox
        /// </summary>
        public string IpAddressHint { get; set; } = Resources.Strings.tab_home_ipaddress;

        /// <summary>
        /// ipaddress of the test unit.
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
        /// Holds info about ipaddress:port, connected Omicron Serial # and etc.
        /// </summary>
        public string Log { get; set; } = "";

        /// <summary>
        /// a flag indicating Omicron Test Set connected and running
        /// </summary>
        public bool IsOmicronConnected { get; set; } = false;

        /// <summary>
        /// a flag indication UUT is connected and responding
        /// </summary>
        public bool IsUnitUnderTestConnected { get; set; } = false;

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

            Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Application Starts\n";
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Starts a test with the values specified in Nominal Values page and
        /// Communication page.
        /// </summary>
        public async Task StartCommunicationAsync()
        {            
            // start point of all test steps with the first mouse click and it will ignore subsequent mouse clicks
            await RunCommand(() => IsUnitUnderTestConnected, async () =>
            {
                try
                {
                    // update the user
                    Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Communication starts\n";

                    // get new construct of CMCControl
                    CMCControl = new CMCControl
                    {
                        // TODO: this code needs to move its own project and access through dependency injection
                        // generate new Omicron Test engine
                        CMEngine = new CMEngine()
                    };

                    // get new construct of ModbusClient
                    EAModbusClient = new EasyModbus.ModbusClient
                    {
                        IPAddress = IpAddress,
                        Port = Convert.ToInt32(Port),
                        ConnectionTimeout = 20000,
                        // LogFileFilename = @"C:\Users\TBircek\Documents\metering\modbus.log"
                    };

                    // ModbusClient.Connect();
                    if (EAModbusClient.Available(10000))
                    {
                        EAModbusClient.Connect();
                    }

                    // find any CMCEngine attached to this computer
                    if (CMCControl.FindCMC())
                    {

                        // perform initial set up on CMCEngine
                        CMCControl.InitialSetup();

                        // Is there Omicron Test Set attached to this app?
                        if (CMCControl.DeviceID > 0)
                        {
                            // Run the test schedule per the user input
                            await CMCControl.TestSample(
                                         // communication register to retrieve information from
                                         Register: Convert.ToInt32(IoC.TestDetails.Register),
                                         // start of the test steps value
                                         From: Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].From),
                                         // end of the test steps value
                                         To: Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].To),
                                         // increment of the steps
                                         Delta: Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Delta),
                                         // duration of the test steps
                                         DwellTime: Convert.ToDouble(IoC.TestDetails.DwellTime),
                                         // currently not used
                                         MeasurementDuration: 0d,
                                         StartDelayTime: Convert.ToDouble(IoC.TestDetails.StartDelayTime),
                                         // interval to read the register through communication protocol
                                         MeasurementInterval: Convert.ToDouble(IoC.TestDetails.MeasurementInterval),
                                         // Delay reading of the register to prevent out of range values due to ramp up
                                         StartMeasurementDelay: Convert.ToDouble(IoC.TestDetails.StartMeasurementDelay),
                                         // excel header values for reporting.
                                         message: $"Time,Register,Test Value,Min Value,Max Value\n"
                            );

                        }
                        else
                        {
                            Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Failed: Omicron Test Set ID is a zero\n";
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Find no Omicron");
                        Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Failed: There is no attached Omicron Test Set. Please attached a Omicron Test Set before test\n";
                    }


                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                    // TODO: show error once and terminate connection
                    Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Start Communication failed: {ex.Message}.\n";

                    // catch inner exceptions if exists
                    if (ex.InnerException != null)
                    {
                        Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                    }
                }
            });
        }
        #endregion

        #region Public Commands

        #endregion

    }
}
