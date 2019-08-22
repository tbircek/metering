using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using metering.core.Resources;
using OMICRON.CMEngAL;

namespace metering.core
{
    public class CMCControl // : IOmicron

    {
        /// <summary>
        /// Omicron CM Engine
        /// </summary>
        private CMEngine engine;

        /// <summary>
        /// Omicron CM Engine
        /// </summary>
        public CMEngine CMEngine
        {
            get
            {
                if (engine == null)
                    return new CMEngine();

                return engine;
            }
            set
            {
                engine = value;
            }
        }

        /// <summary>
        ///  Omicron Test Set string commands.
        /// </summary>
        private StringCommands omicronStringCommands;

        /// <summary>
        /// Omicron Test Set string commands.
        /// </summary>
        public StringCommands OmicronStringCommands
        {
            get
            {
                if (omicronStringCommands == null)
                    return new StringCommands();

                return omicronStringCommands;
            }
            set
            {
                omicronStringCommands = value;
            }
        }
        // /// <summary>
        // /// Refers to Omicron CM Engine.
        // /// </summary>
        ////  CMEngine CMEngine = new CMEngine();

        ///// <summary>
        ///// Implemented Omicron Test Set string commands.
        ///// </summary>
        //StringCommands omicron = new StringCommands();


        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private const double maxVoltageMagnitude = 8.0f;
        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private const double maxCurrentMagnitude = 2.0f;
        //protected CMCControl() { }

        //protected CMCControl(CMEngine CMEngine, StringCommands omicron, int deviceID)
        //{
        //    this.CMEngine = CMEngine ?? throw new ArgumentNullException(nameof(CMEngine));
        //    this.omicron = omicron ?? throw new ArgumentNullException(nameof(omicron));
        //    DeviceID = deviceID;
        //}

        /// <summary>
        /// Associated Omicron Test Set ID. Assigned by CM Engine.
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// Omicron Test Set debuggin log levels.
        /// </summary>
        public enum LogLevels : short { None, Level1, Level2, Level3, };

        /// <summary>
        /// Default value of Voltage amplifiers while testing non-voltage values.
        /// </summary>
        const double nominalVoltage = 120.0f;
        /// <summary>
        /// Default value of Current amplifiers while testing non-current values.
        /// </summary>
        const double nominalCurrent = 0.02f;
        /// <summary>
        /// Default value of amplifiers phase while testing non-phase values.
        /// </summary>
        const double phase = 0.0f;
        /// <summary>
        /// Default value of Frequency amplifiers while testing non-frequency values. Always a non-zero value.
        /// </summary>
        const double nominalFrequency = 60.0f;

        /// <summary>
        /// Scans for Omicron CMC's that associated and NOT locked.
        /// </summary>
        public bool FindCMC()
        {
            CMEngine.DevScanForNew();
            string deviceList = "";
            ExtractParameters extract = new ExtractParameters();
            deviceList = CMEngine.DevGetList(ListSelectType.lsUnlockedAssociated);

            if (string.IsNullOrWhiteSpace(deviceList))
            {
                Debug.WriteLine("Unable to find any device attach to this computer");
                return false;
            }

            // log Omicron Test Set debug information.
            CMEngine.LogNew(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cmc.log");
            CMEngine.LogSetLevel((short)LogLevels.Level3);
            Debug.WriteLine($"Found device: {deviceList}", "info");
            Debug.WriteLine($"Error text: {CMEngine.GetExtError()}");

            DeviceID = Convert.ToInt32(extract.Parameters(1, deviceList));
            CMEngine.DevLock(DeviceID);

            // Searches for external Omicron amplifiers and returns a list of IDs.
            // Future use.
            //omicron.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.amp_scan);

            return true;
        }

        /// <summary>
        /// Sets Omicron Test Set default values and limits.
        /// </summary>
        public void InitialSetup()
        {
            try
            {
                // initialize routes.
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.amp_route_init);
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.amp_def_init);
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.amp_route_voltage);
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.amp_route_current);

                // update ranges.
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, string.Format(OmicronStringCmd.amp_range_voltage, maxVoltageMagnitude));
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, string.Format(OmicronStringCmd.amp_range_current, maxCurrentMagnitude));

                // change pmode.
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.out_analog_pmode);

                //// set voltage amplifiers default values.
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:1", nominalVoltage, phase, nominalFrequency);
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:2", nominalVoltage, phase, nominalFrequency);
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:3", nominalVoltage, phase, nominalFrequency);

                //// set current amplifiers default values.
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:1", nominalCurrent, phase, nominalFrequency);
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:2", nominalCurrent, phase, nominalFrequency);
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:3", nominalCurrent, phase, nominalFrequency);

            }
            catch (Exception err)
            {
                Debug.WriteLine($"initial setup::Exception InnerException is : {err.Message}");
            }
        }

        /// <summary>
        /// Disconnects and releases associated Omicron Test Set.
        /// </summary>
        public void ReleaseOmicron()
        {
            try
            {
                OmicronStringCommands = null;
                CMEngine.DevUnlock(DeviceID);
                CMEngine = null;
            }
            catch (Exception err)
            {
                Debug.WriteLine($"release Omicron::Exception InnerException is : {err.Message}");
            }
        }

        /// <summary>
        /// Turns off outputs of Omicron Test Set.
        /// </summary>
        public void TurnOffCMC()
        {
            try
            {
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.out_analog_outputOff);
                var t = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(2.0));
                });
                t.Wait();
                ReleaseOmicron();
            }
            catch (Exception err)
            {
                Debug.WriteLine($"turnOffCMC setup::Exception InnerException is : {err.Message}");
            }
        }

        /// <summary>
        /// Turns on outputs of Omicron Test Set.
        /// </summary>
        public void TurnOnCMC()
        {
            try
            {
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.out_analog_outputOn);
            }
            catch (Exception err)
            {
                Debug.WriteLine($"turnONCMC setup::Exception InnerException is : {err.Message}");
            }
        }

        /// <summary>
        /// Presentation quick fix. Would have to be in a separate class.
        /// </summary>
        /// <param name="Register">Modbus register to monitor.</param>
        /// <param name="From">Test start point.</param>
        /// <param name="To">Test stop point.</param>
        /// <param name="Delta">Test interval magnatitude point.</param>
        /// <param name="DwellTime">Test interval time point.</param>
        /// <param name="MeasurementDuration">Overall test time for this particular register.</param>
        /// <param name="StartDelayTime">Wait time to stay in chamber room before a test magnitude applied.</param>
        /// <param name="MeasurementInterval">Specifies register reading interval.</param>
        /// <param name="StartMeasurementDelay">Wait time after a test magnitude applied.</param>
        public async Task TestSample(int Register, double From, double To, double Delta, double DwellTime, double MeasurementDuration, double StartDelayTime, double MeasurementInterval, double StartMeasurementDelay)
        {

            try
            {
                Debug.WriteLine($"Time: {DateTime.Now.ToLocalTime():hh:mm:ss.fff}\tTest parameters:");
                Debug.Indent();
                Debug.WriteLine($"From: {From:F3}\tTo: {To}\t\tDelta: {Delta:F3}\t\t\t\t\tDwell time: {DwellTime}sec\r\tStart delay time: {StartDelayTime}sec\tMeasurement interval: {MeasurementInterval}mSec\tStart measurement delay: {StartMeasurementDelay}sec\n");
                Debug.Unindent();

                var delay = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(StartDelayTime));
                    mdbus.ConnectionTimeout = 5000;
                    mdbus.Connect("192.168.0.122", 502);
                });
                delay.Wait();

                int progressStep = 1;
                //TestStartValue = From;

                for (double testStartValue = From; testStartValue <= To; testStartValue += Delta)
                {
                    Timer mdbusTimer = new Timer(TimerTick, Register, TimeSpan.FromSeconds(StartMeasurementDelay), TimeSpan.FromMilliseconds(MeasurementInterval));

                    Debug.WriteLine($"Time: {DateTime.Now.ToLocalTime():hh:mm:ss.fff}\tRegister: {Register}\tTest value: {testStartValue:F3}");
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():hh:mm:ss.fff} - Register: {Register} --- Test value: {testStartValue:F3}\n";

                // set voltage amplifiers default values.
                OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:1", testStartValue, phase, nominalFrequency);
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:2", 0, 0, nominalFrequency);
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:3", 0, 0, nominalFrequency);

                    // set current amplifiers default values.
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:1", 0, 0, nominalFrequency);
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:2", 0, 0, nominalFrequency);
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:3", 0, 0, nominalFrequency);

                    TurnOnCMC();

                    var t = Task.Run(async delegate
                    {
                        await Task.Delay(TimeSpan.FromSeconds(DwellTime));
                        mdbusTimer.Dispose();

                        // Remember first test case and Add +1.
                        //Progress = Convert.ToDouble(progressStep) / Math.Ceiling((Math.Abs(To - From) / Delta) + 1);
                        IoC.Commands.TestProgress = Convert.ToDouble(progressStep) / Math.Ceiling((Math.Abs(To - From) / Delta) + 1);


                        // Progress cannot be larger than 100%
                        if (Progress <= 1.00)
                        {
                            Debug.WriteLine($"\t\t\t\t\t\t\t\tMin value: {MinTestValue}\t\tMax value: {MaxTestValue}\tProgress: {Progress * 100:F2}% completed.\n");
                            IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():hh:mm:ss.fff} - Min value: {MinTestValue} Max value: {MaxTestValue} Progress: {Progress * 100:F2}% completed.\n";
                        }
                        MinTestValue = 0;
                        MaxTestValue = 0;
                        progressStep++;
                    });
                    t.Wait();
                }

                Debug.WriteLine($"Time: {DateTime.Now.ToLocalTime():hh:mm:ss.fff}\tTest completed for register: {Register}\n");
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():hh:mm:ss.fff} - Test completed for register: {Register}.\n";

                TurnOffCMC();
                mdbus.Disconnect();
            }
            finally
            {
                TurnOffCMC();
                mdbus.Disconnect();
            }
        }
        /// <summary>
        /// Instance of Modbus Communication library.
        /// </summary>
        ModbusClient mdbus = new ModbusClient();
        /// <summary>
        /// Holds minimum value for test register.
        /// </summary>
        public int MinTestValue { get; set; }
        /// <summary>
        /// Holds minimum value for test register.
        /// </summary>
        public int MaxTestValue { get; set; }
        /// <summary>
        /// Allows to read test register values specified by "Measurement Interval".
        /// </summary>
        private void TimerTick(object Register)
        {
            int register = Convert.ToInt32(Register);
            int[] serverResponse = Task.Factory.StartNew(() => mdbus.ReadHoldingRegisters(register, 1)).Result;
            for (int i = 0; i < serverResponse.Length; i++)
            {
                if (MinTestValue > serverResponse[i] || MinTestValue == 0)
                {
                    MinTestValue = serverResponse[i];
                }
                if (MaxTestValue < serverResponse[i] || MaxTestValue == 0)
                {
                    MaxTestValue = serverResponse[i];
                }
            }
        }

        /// <summary>
        /// Hold progress information per test register.
        /// </summary>
        public double Progress { get; set; }

    }
}