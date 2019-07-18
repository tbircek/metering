using OMICRON.CMEngAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using metering.Resources;
using System.Threading.Tasks;
using System.Threading;

namespace metering
{
    public class CMCControl
    {
        /// <summary>
        /// Refers to Omicron CM Engine.
        /// </summary>
        CMEngine engine = new CMEngine();
        /// <summary>
        /// Implemented Omicron Test Set string commands.
        /// </summary>
        StringCommands omicron = new StringCommands();
        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private const double maxVoltageMagnitude = 8.0f;
        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private const double maxCurrentMagnitude = 2.0f;
        //protected CMCControl() { }

        //protected CMCControl(CMEngine engine, StringCommands omicron, int deviceID)
        //{
        //    this.engine = engine ?? throw new ArgumentNullException(nameof(engine));
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
        public enum LogLevels : short { None, Level1, Level2, };

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
            engine.DevScanForNew();
            string deviceList = "";
            ExtractParameters extract = new ExtractParameters();
            deviceList = engine.DevGetList(ListSelectType.lsUnlockedAssociated);

            if (string.IsNullOrWhiteSpace(deviceList))
            {
                Debug.WriteLine("Unable to find any device attach to this computer");
                return false;
            }

            // log Omicron Test Set debug information.
            engine.LogNew(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\cmc.log");
            engine.LogSetLevel((short)LogLevels.Level2);
            Debug.WriteLine(string.Format("Found device: {0}", deviceList), "info");
            Debug.WriteLine(string.Format("Error text: {0}", engine.GetExtError()));

            DeviceID = Convert.ToInt32(extract.Parameters(1, deviceList));
            engine.DevLock(DeviceID);

            // Searches for external Omicron amplifiers and returns a list of IDs.
            // Future use.
            //omicron.SendStringCommand(engine, DeviceID, OmicronStringCmd.amp_scan);

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
                omicron.SendStringCommand(engine, DeviceID, OmicronStringCmd.amp_route_init);
                omicron.SendStringCommand(engine, DeviceID, OmicronStringCmd.amp_def_init);
                omicron.SendStringCommand(engine, DeviceID, OmicronStringCmd.amp_route_voltage);
                omicron.SendStringCommand(engine, DeviceID, OmicronStringCmd.amp_route_current);

                // update ranges.
                omicron.SendStringCommand(engine, DeviceID, string.Format(OmicronStringCmd.amp_range_voltage, maxVoltageMagnitude));
                omicron.SendStringCommand(engine, DeviceID, string.Format(OmicronStringCmd.amp_range_current, maxCurrentMagnitude));

                // change pmode.
                omicron.SendStringCommand(engine, DeviceID, OmicronStringCmd.out_analog_pmode);

                //// set voltage amplifiers default values.
                //omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.v, "1:1", nominalVoltage, phase, nominalFrequency);
                //omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.v, "1:2", nominalVoltage, phase, nominalFrequency);
                //omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.v, "1:3", nominalVoltage, phase, nominalFrequency);

                //// set current amplifiers default values.
                //omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.i, "1:1", nominalCurrent, phase, nominalFrequency);
                //omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.i, "1:2", nominalCurrent, phase, nominalFrequency);
                //omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.i, "1:3", nominalCurrent, phase, nominalFrequency);

            }
            catch (Exception err)
            {
                Debug.WriteLine(String.Format("initial setup::Exception InnerException is : {0}", err.Message));
            }
        }

        /// <summary>
        /// Disconnects and releases associated Omicron Test Set.
        /// </summary>
        public void ReleaseOmicron()
        {
            try
            {
                omicron = null;
                engine.DevUnlock(DeviceID);
                engine = null;
            }
            catch (Exception err)
            {
                Debug.WriteLine(string.Format("release Omicron::Exception InnerException is : {0}", err.Message));
            }
        }

        /// <summary>
        /// Turns off outputs of Omicron Test Set.
        /// </summary>
        public void TurnOffCMC()
        {
            try
            {
                omicron.SendStringCommand(engine, DeviceID, OmicronStringCmd.out_analog_outputOff);
                var t = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(2.0));
                });
                t.Wait();
                ReleaseOmicron();
            }
            catch (Exception err)
            {
                Debug.WriteLine(String.Format("turnOffCMC setup::Exception InnerException is : {0}", err.Message));
            }
        }

        /// <summary>
        /// Turns on outputs of Omicron Test Set.
        /// </summary>
        public void TurnOnCMC()
        {
            try
            {
                omicron.SendStringCommand(engine, DeviceID, OmicronStringCmd.out_analog_outputOn);
            }
            catch (Exception err)
            {
                Debug.WriteLine(String.Format("turnONCMC setup::Exception InnerException is : {0}", err.Message));
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
        public void TestSample(int Register, double From, double To, double Delta, double DwellTime, double MeasurementDuration, double StartDelayTime, double MeasurementInterval, double StartMeasurementDelay)
        {

            Debug.WriteLine($"Time: {DateTime.Now.ToString("hh:mm:ss.fff")}\tTest parameters:");
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

                Debug.WriteLine($"Time: {DateTime.Now.ToString("hh:mm:ss.fff")}\tRegister: {Register}\tTest value: {testStartValue:F3}");

                // set voltage amplifiers default values.
                omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.v, "1:1", testStartValue, phase, nominalFrequency);
                omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.v, "1:2", 0, 0, nominalFrequency);
                omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.v, "1:3", 0, 0, nominalFrequency);

                // set current amplifiers default values.
                omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.i, "1:1", 0, 0, nominalFrequency);
                omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.i, "1:2", 0, 0, nominalFrequency);
                omicron.SendOutAna(engine, DeviceID, (int)StringCommands.GeneratorList.i, "1:3", 0, 0, nominalFrequency);

                TurnOnCMC();

                var t = Task.Run(async delegate
                {
                    await Task.Delay(TimeSpan.FromSeconds(DwellTime));
                    mdbusTimer.Dispose();

                    // Remember first test case and Add +1.
                    Progress = Convert.ToDouble(progressStep) / Math.Ceiling((Math.Abs(To - From) / Delta) + 1);
                    
                    // Progress cannot be larger than 100%
                    if (Progress <= 1.00)
                    {
                        Debug.WriteLine($"\t\t\t\t\t\t\t\tMin value: {MinTestValue}\t\tMax value: {MaxTestValue}\tProgress: {Progress * 100:F2}% completed.\n");
                    }
                    MinTestValue = 0;
                    MaxTestValue = 0;
                    progressStep++;
                });
                t.Wait();
            }
            Debug.WriteLine($"Time: {DateTime.Now.ToString("hh:mm:ss.fff")}\tTest completed for register: {Register}\n");
           
            TurnOffCMC();
            mdbus.Disconnect();
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