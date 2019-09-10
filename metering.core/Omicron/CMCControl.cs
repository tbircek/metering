using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using metering.core.Resources;
using OMICRON.CMEngAL;

namespace metering.core
{
    public class CMCControl // : IOmicron

    {
        #region Private variables

        /// <summary>
        /// Omicron CM Engine
        /// </summary>
        private CMEngine engine;

        /// <summary>
        ///  Omicron Test Set string commands.
        /// </summary>
        private StringCommands omicronStringCommands;

        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private const double maxVoltageMagnitude = 8.0f;

        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private const double maxCurrentMagnitude = 2.0f;

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

        #endregion

        #region Public Properties

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

        /// <summary>
        /// Holds minimum value for test register.
        /// </summary>
        public int MinTestValue { get; set; }

        /// <summary>
        /// Holds minimum value for test register.
        /// </summary>
        public int MaxTestValue { get; set; }

        /// <summary>
        /// Hold progress information per test register.
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// Associated Omicron Test Set ID. Assigned by CM Engine.
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// Omicron Test Set debugging log levels.
        /// </summary>
        public enum LogLevels : short { None, Level1, Level2, Level3, };

        #endregion

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
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Unable to find any device attach to this computer\n";
                return false;
            }

            // log Omicron Test Set debug information.
            CMEngine.LogNew(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cmc.log");
            CMEngine.LogSetLevel((short)LogLevels.Level3);
            Debug.WriteLine($"Found device: {deviceList}", "info");
            Debug.WriteLine($"Error text: {CMEngine.GetExtError()}");

            DeviceID = Convert.ToInt32(extract.Parameters(1, deviceList));
            CMEngine.DevLock(DeviceID);

            IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Connecting device: {extract.Parameters(2, deviceList)}\n";

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

                // change power mode.
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
            catch (Exception ex)
            {
                Debug.WriteLine($"initial setup::Exception InnerException is : {ex.Message}");
                IoC.Communication.Log += $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tinitial setup::Exception InnerException is : {ex.Message}\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }
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
            catch (Exception ex)
            {
                Debug.WriteLine($"release Omicron::Exception InnerException is : {ex.Message}");
                IoC.Communication.Log += $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\trelease Omicron: error detected\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }
            }
        }

        /// <summary>
        /// Turns off outputs of Omicron Test Set and release it.
        /// </summary>
        public void TurnOffCMC()
        {
            try
            {
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.out_analog_outputOff);
                var t = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(5.0));
                });
                t.Wait();
                ReleaseOmicron();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"turnOffCMC setup::Exception InnerException is : {ex.Message}");
                IoC.Communication.Log += $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tturnOffCMC setup: error detected\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }
            }
        }

        /// <summary>
        /// Turns on outputs of Omicron Test Set.
        /// </summary>
        public void TurnOnCMC()
        {
            try
            {
                // Send command to Turn On Analog Outputs
                OmicronStringCommands.SendStringCommand(CMEngine, DeviceID, OmicronStringCmd.out_analog_outputOn);
                Debug.WriteLine($"{DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}: turnOnCMC setup: started\t");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"turnONCMC setup::Exception InnerException is : {ex.Message}");
                IoC.Communication.Log += $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tturnOnCMC setup: error detected\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }
            }
        }

        /// <summary>
        /// Presentation quick fix. Would have to be in a separate class.
        /// </summary>
        /// <param name="Register">Modbus register to monitor.</param>
        /// <param name="From">Test start point.</param>
        /// <param name="To">Test stop point.</param>
        /// <param name="Delta">Test interval magnitude point.</param>
        /// <param name="DwellTime">Test interval time point.</param>
        /// <param name="MeasurementDuration">Overall test time for this particular register.</param>
        /// <param name="StartDelayTime">Wait time to stay in chamber room before a test magnitude applied.</param>
        /// <param name="MeasurementInterval">Specifies register reading interval.</param>
        /// <param name="StartMeasurementDelay">Wait time after a test magnitude applied.</param>
        /// <param name="message">Test message to pass to the Log textbox</param>
        public async Task TestSample(int Register, double From, double To, double Delta, double DwellTime, double MeasurementDuration, double StartDelayTime, double MeasurementInterval, double StartMeasurementDelay, string message)
        {

            try
            {
                Debug.WriteLine($"{DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tTest parameters:");
                Debug.Indent();
                Debug.WriteLine($"From: {From:F3}\tTo: {To}\t\tDelta: {Delta:F3}\t\t\t\t\tDwell time: {DwellTime}sec\r\tStart delay time: {StartDelayTime}min\tMeasurement interval: {MeasurementInterval}mSec\tStart measurement delay: {StartMeasurementDelay}sec\n");
                Debug.Unindent();

                // Wait StartDelayTime to start Modbus communication
                var delay = Task.Run(async delegate
                {
                    // wait for the user specified "Start Delay Time"
                    await Task.Delay(TimeSpan.FromMinutes(StartDelayTime));

                    // Progress bar is visible
                    IoC.Commands.IsConnectionCompleted = IoC.Commands.IsConnecting = IoC.Communication.EAModbusClient.Connected;

                    // change color of Cancel Command button to Green
                    IoC.Commands.CancelForegroundColor = "00ff00";

                });

                // wait for modbus connection
                delay.Wait();

                // update test progress
                int progressStep = 1;

                // report file id to distinguish between test results 
                string fileId = $"{DateTime.Now.ToLocalTime():MM_dd_yy_hh_mm}";

                // set maximum value for the progress bar
                IoC.Commands.MaximumTestCount = Math.Ceiling((Math.Abs(To - From) / Delta) + 1);

                // Process test steps
                for (double testStartValue = From; testStartValue <= To; testStartValue += Delta)
                {
                    // set timer to read modbus register per the user specified time.
                    Timer mdbusTimer = new Timer(TimerTick, Register, TimeSpan.FromSeconds(StartMeasurementDelay), TimeSpan.FromMilliseconds(MeasurementInterval));

                    Debug.WriteLine($"{DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tRegister: {Register}\tTest value: {testStartValue:F3}");
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Register: {Register} --- Test value: {testStartValue:F3}\n";

                    // set voltage amplifiers default values.
                    // Analog signal: Voltage Output 1:
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:1", testStartValue, phase, nominalFrequency);

                    // Analog signal: Voltage Output 2:
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:2", 0, 0, nominalFrequency);

                    // Analog signal: Voltage Output 3:
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:3", 0, 0, nominalFrequency);

                    // set current amplifiers default values.
                    // Analog signal: Current Output 1:
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:1", 0, 0, nominalFrequency);

                    // Analog signal: Current Output 2:
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:2", 0, 0, nominalFrequency);

                    // Analog signal: Current Output 3:
                    OmicronStringCommands.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:3", 0, 0, nominalFrequency);

                    // Turn On Omicron Analog Outputs per the user input
                    TurnOnCMC();

                    // Start reading the user specified Register
                    var t = Task.Run(async delegate
                    {
                        // wait until the user specified "Dwell Time" expires.
                        await Task.Delay(TimeSpan.FromSeconds(DwellTime));

                        // terminate reading modbus register because "Dwell Time" is over.
                        mdbusTimer.Dispose();

                        // Remember first test case and Add +1.
                        Debug.WriteLine($"{Convert.ToDouble(progressStep) / Math.Ceiling(Math.Abs(To - From) / Delta) + 1}");

                        // increment progress percentage
                        Progress = Convert.ToDouble(progressStep) / Math.Ceiling((Math.Abs(To - From) / Delta) + 1);

                        // Progress cannot be larger than 100%
                        if (Progress <= 1.00)
                        {
                            // update multiple texts to present new progress percentage to the user/developer.
                            Debug.WriteLine($"\t\t\t\t\t\t\t\tMin value: {MinTestValue}\t\tMax value: {MaxTestValue}\tProgress: {Progress * 100:F2}% completed.\n");
                            IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Min value: {MinTestValue} Max value: {MaxTestValue}\n";
                            message += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff},{Register},{testStartValue},{MinTestValue},{MaxTestValue}";
                        }

                        // log the test step result to a ".csv" format file
                        LogTestResults(message, Register, From, To, fileId);

                        // reset min test value for the next test range
                        MinTestValue = 0;

                        // reset max test value for the next test range
                        MaxTestValue = 0;

                        // clear message for the next test values
                        message = string.Empty;

                        // increment progress
                        progressStep++;

                        // increment progress bar strip on the "Button"
                        IoC.Commands.TestProgress = Convert.ToDouble(progressStep);
                    });

                    // wait for the timer to expire
                    t.Wait();
                }

                // update developer "Test completed"
                Debug.WriteLine($"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}\tTest completed for register: {Register}\n");

                // update the user "Test completed"
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Test completed for register: {Register}.\n";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}\tException: {ex.Message}\n");

                // TODO: show error once and terminate connection
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Test failed: {ex.Message}.\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }
            }
            finally
            {
                // Trying to stop the app gracefully.
                await Task.Run(() => ProcessErrors());
            }
        }

        /// <summary>
        /// Handles errors and stops the app gracefully.
        /// </summary>
        private void ProcessErrors()
        {
            // Turn off outputs of Omicron Test Set and release it.
            TurnOffCMC();

            // Disconnect Modbus Communication
            IoC.Communication.EAModbusClient.Disconnect();

            // Progress bar is invisible
            IoC.Commands.IsConnectionCompleted = IoC.Commands.IsConnecting = IoC.Communication.EAModbusClient.Connected;
            // mdbus.GetConnected();

            // change color of Cancel Command button to Red
            IoC.Commands.CancelForegroundColor = "ff0000";
        }

        /// <summary>
        /// Logs test results
        /// </summary>
        /// <param name="message">test values to write in to a file</param>
        /// <param name="Register">the Register number read during the test</param>
        /// <param name="From">the start value of the test step</param>
        /// <param name="To">the end value of the test step</param>
        /// <param name="fileId">the test start time to add the end of the report file name</param>
        private void LogTestResults(string message, int Register, double From, double To, string fileId)
        {
            // specify a "metering" that under the current user's "MyDocuments" folder
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "metering");

            // generate the folder
            Directory.CreateDirectory(directory);

            // generate a file and a fileStream to write the test results
            using (var fileStream = new StreamWriter(File.OpenWrite(Path.Combine(directory, $"{Register}_{From:F3}-{To:F3}_{fileId}.csv"))))
            {
                // find the end of the file
                fileStream.BaseStream.Seek(0, SeekOrigin.End);

                // add the message at the of the file
                fileStream.WriteLineAsync(message);
            }
        }

        /// <summary>
        /// Allows to read test register values specified by "Measurement Interval".
        /// </summary>
        private void TimerTick(object Register)
        {
            try
            {
                int register = Convert.ToInt32(Register);
                int[] serverResponse = Task.Factory.StartNew(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(register, 1)).Result;

                if (serverResponse.Length > 0)
                {
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
                else
                {
                    // server failed to respond. Ignoring it until find a better option.
                }

            }
            catch (Exception ex)
            {
                // TODO: show error once and terminate connection
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                   IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }
            }
        }
    }
}