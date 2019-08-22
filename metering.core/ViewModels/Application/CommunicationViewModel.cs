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
        // TODO: Add a model for "Connect" button 
        // TODO: squirrel.windows update tool
        // TODO: Remove ModbusClient to its own project
        // TODO: Remove CMCControl to its own project
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

            Log += $"{DateTime.Now.ToLocalTime()}: App Starts\n";
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Starts a test with the values specified in Nominal Values page and
        /// Communication page.
        /// </summary>
        public async Task StartCommunicationAsync()
        {

            Log += $"{DateTime.Now.ToLocalTime()}: Communication starts\n";

            await RunCommand(() => IsUnitUnderTestConnected, async () =>
            {
                try
                {
                    // get new construct of CMCControl
                    CMCControl = new CMCControl
                    {
                        CMEngine = new CMEngine()
                    };

                    // find any CMCEngine attached to this computer
                    if (CMCControl.FindCMC())
                    {

                        // perform initial set up on CMCEngine
                        CMCControl.InitialSetup();

                        if (CMCControl.DeviceID > 0)
                        {                           
                            await CMCControl.TestSample(
                                         Register: Convert.ToInt32(IoC.TestDetails.Register),
                                         From: Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].From),
                                         To: Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].To),
                                         Delta: Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Delta),
                                         DwellTime: Convert.ToDouble(IoC.TestDetails.DwellTime),
                                         MeasurementDuration: 0d,
                                         StartDelayTime: Convert.ToDouble(IoC.TestDetails.StartDelayTime),
                                         MeasurementInterval: Convert.ToDouble(IoC.TestDetails.MeasurementInterval),
                                         StartMeasurementDelay: Convert.ToDouble(IoC.TestDetails.StartMeasurementDelay)
                            );

                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        Debug.WriteLine("Find no Omicron");
                    }


                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.Message);
                }

                //// open communication channel
                //// TODO: ModbusClient moves to its own project
                //if (ModbusClient == null)
                //    ModbusClient = new ModbusClient
                //    {
                //        IpAddress = this.IpAddress,
                //        Port = Convert.ToInt32(this.Port),
                //        ConnectionTimeout = 20000,
                //    };

                //if (!ModbusClient.GetConnected())
                //{
                //    try
                //    {
                //        ModbusClient.Connect();

                //        // await if the server is connected
                //        bool isUUTConnected = await Task.Factory.StartNew(() => ModbusClient.GetConnected());

                //        int[] response = ModbusClient.ReadHoldingRegisters(Convert.ToInt32(IoC.TestDetails.Register), 1);

                //        for (int i = 0; i < response.Length; i++)
                //        {
                //            Debug.WriteLine($"Start Test is running: Register: {Convert.ToInt32(IoC.TestDetails.Register) + i} reads {response[i]}");
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        // update modbus connection status
                //        IsUnitUnderTestConnected = false;

                //        // Change Start Test Button color
                //        IoC.Commands.StartTestForegroundColor = "00ff00";

                //        Log += $"{DateTime.Now} - Modbus error: {ex.Message}";
                //    }
                //    finally
                //    {
                //        // update modbus connection status
                //        IsUnitUnderTestConnected = false;

                //        // Disconnect from Modbus server
                //        ModbusClient.Disconnect();
                //    }
                //}
                //else
                //{
                //    // Disconnect from Modbus server
                //    ModbusClient.Disconnect();

                //    // Change Start Test Button color
                //    IoC.Commands.StartTestForegroundColor = "00ff00";

                //    Debug.WriteLine("Communication terminated.");
                //}

            });
        }
        #endregion

        #region Public Commands

        #endregion

    }
}
