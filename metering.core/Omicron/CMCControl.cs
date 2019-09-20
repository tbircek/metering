using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// Handles Omicron Test Set operations.
    /// </summary>
    public class CMCControl

    {

        #region Private Members

        /// <summary>
        /// Omicron CM Engine
        /// </summary>
        private CMEngine.CMEngine engine;

        /// <summary>
        /// a thread lock object for this class
        /// </summary>
        private readonly object mThreadLock = new object();

        /// <summary>
        /// Timer ticks used to read ModbusClient
        /// </summary>
        private Timer MdbusTimer { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Omicron CM Engine
        /// </summary>
        public CMEngine.CMEngine CMEngine
        {
            get
            {
                if (engine == null)
                    return new CMEngine.CMEngine();

                return engine;
            }
            set
            {
                engine = value;
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
        public enum OmicronLoggingLevels : short { None, Level1, Level2, Level3, };

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Disconnects and releases associated Omicron Test Set.
        /// </summary>
        public void ReleaseOmicron()
        {
            try
            {
                // lock the thread
                lock (mThreadLock)
                {

                    // inform the developer
                    IoC.Logger.Log($"ReleaseOmicron: started",LogLevel.Informative);

                    // unlock attached Omicron Test Set                    
                    CMEngine.DevUnlock(DeviceID);

                    // Destruct Omicron Test set
                    CMEngine = null;

                }
            }
            catch (Exception ex)
            {
                // inform the developer about error
                IoC.Logger.Log($"InnerException is : {ex.Message}");

                // inform the user about error
                IoC.Communication.Log += $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\trelease Omicron: error detected\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the user about more details about error.
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
                // lock the thread
                lock (mThreadLock)
                {
                    // Send command to Turn On Analog Outputs
                    IoC.StringCommands.SendStringCommand(OmicronStringCmd.out_analog_outputOn);

                    // update the developer
                    IoC.Logger.Log($"turnOnCMC setup: started",LogLevel.Informative);
                }
            }
            catch (Exception ex)
            {
                // inform the developer about error.
                IoC.Logger.Log($"InnerException: {ex.Message}");

                // inform the user about error.
                IoC.Communication.Log += $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tturnOnCMC setup: error detected\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the user about more details about error.
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }
            }
        }

        /// <summary>
        /// Handles errors and stops the app gracefully.
        /// </summary>
        /// <param name="userRequest">false if test interrupt requested by the user
        /// true if test completed itself</param>
        public void ProcessErrors(bool userRequest)
        {
            // lock the thread
            lock (mThreadLock)
            {
                if (!userRequest)
                {
                    // update developer "Test interrupted"
                    IoC.Logger.Log($"Test interrupted",LogLevel.Informative);

                    // update the user "Test interrupted"
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Test interrupted by the user.\n";
                }
                else
                {

                    // update the user "Test interrupted"
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Test completed.\n";
                }

                // Turn off outputs of Omicron Test Set and release it.
                IoC.PowerOptions.TurnOffCMC();

                // Disconnect Modbus Communication
                IoC.Communication.EAModbusClient.Disconnect();

                // Progress bar is invisible
                IoC.Commands.IsConnectionCompleted = IoC.Commands.IsConnecting = IoC.Communication.EAModbusClient.Connected;

                // change color of Cancel Command button to Red
                IoC.Commands.CancelForegroundColor = "ff0000";
            }
        }

        /// <summary>
        /// Runs Test Steps
        /// </summary>
        /// <returns></returns>
        public async Task TestAsync(string Message)
        {
            try
            {

                // update test progress
                int progressStep = default(int);

                // Wait StartDelayTime to start Modbus communication
                Task.Run(async delegate
                {
                    // lock the task
                    await AsyncAwaiter.AwaitAsync(nameof(Test), async () =>
                    {
                        // wait for the user specified "Start Delay Time"
                        await Task.Delay(TimeSpan.FromMinutes(Convert.ToDouble(IoC.TestDetails.StartDelayTime)), IoC.Commands.Token);
                    });

                    // Progress bar is visible
                    IoC.Commands.IsConnectionCompleted = IoC.Commands.IsConnecting = IoC.Communication.EAModbusClient.Connected;

                    // change color of Cancel Command button to Green
                    IoC.Commands.CancelForegroundColor = "00ff00";

                    // test starts 
                    progressStep = 0;

                }, IoC.Commands.Token).Wait();

                // report file id to distinguish between test results 
                string fileId = $"{DateTime.Now.ToLocalTime():yyyy_MM_dd_HH_mm}";

                // decides which signal is our ramping signal by comparing the mismatch of any "From" and "To" values.
                // after the user clicked "Go" button
                TestSignal testSignal = new TestSignal();

                // verify we have a ramping signal
                if (string.IsNullOrWhiteSpace(testSignal.SignalName))
                {
                    // inform the user there is no test case
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: There is no ramping signal.Please check your entries.\n";

                    // return from this task.
                    return;
                }

                // inform the developer of test SignalName
                IoC.Logger.Log($"Test signal name: {testSignal.SignalName}",LogLevel.Informative);

                // set maximum value for the progress bar
                IoC.Commands.MaximumTestCount = Math.Ceiling((Math.Abs(testSignal.To - testSignal.From) / testSignal.Delta)) + 1;

                // inform the developer MaximumTestCount
                IoC.Logger.Log($"Maximum test count: {IoC.Commands.MaximumTestCount}",LogLevel.Informative);

                // initialize new testStartValue
                double testStartValue;

                // Process test steps
                for (testStartValue = testSignal.From; testStartValue <= (testSignal.To + testSignal.Delta * 1 / 1000); testStartValue += testSignal.Delta)
                {
                    // check if the user canceled the tests.
                    if (!IoC.Commands.Token.IsCancellationRequested)
                    {

                        // set timer to read modbus register per the user specified time.
                        MdbusTimer = new Timer(MeasurementIntervalCallbackAsync, IoC.TestDetails.Register, TimeSpan.FromSeconds(Convert.ToDouble(IoC.TestDetails.StartMeasurementDelay)), TimeSpan.FromMilliseconds(Convert.ToDouble(IoC.TestDetails.MeasurementInterval)));

                        // inform the developer about test register and start value.
                        IoC.Logger.Log($"Register: {IoC.TestDetails.Register}\tTest value: {testStartValue:F3} started",LogLevel.Informative);

                        // inform the user about test register and start value.
                        IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Register: {IoC.TestDetails.Register} --- Test value: {testStartValue:F3} started\n";

                        // send string commands to Omicron
                        SendOmicronCommands(testSignal, testStartValue);

                        // Turn On Omicron Analog Outputs per the user input
                        TurnOnCMC();

                        // Start reading the user specified Register
                        Task.Run(async delegate
                        {

                            // lock the task
                            await AsyncAwaiter.AwaitAsync(nameof(Test), async () =>
                            {

                                // wait until the user specified "Dwell Time" expires.
                                await Task.Delay(TimeSpan.FromSeconds(Convert.ToDouble(IoC.TestDetails.DwellTime)));
                            });

                            // terminate reading modbus register because "Dwell Time" is over.
                            MdbusTimer.Dispose();

                            // inform the developer about test progress.
                            //IoC.Logger.Log($"Min value: {MinTestValue} --- Max value: {MaxTestValue} --- Progress: {Progress * 100.0d:F2}% completed", LogLevel.Informative);

                            // inform the user about test results.
                            IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Min value: {MinTestValue} Max value: {MaxTestValue}\n";

                            // generate a string to inform the user about test results.
                            Message += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff},{IoC.TestDetails.Register},{testStartValue:F3},{MinTestValue:F3},{MaxTestValue:F3}";
                            
                        // wait task to be over with
                        }, IoC.Commands.Token).Wait();

                        // log the test step result to a ".csv" format file
                        LogTestResults(Message, Convert.ToInt32(IoC.TestDetails.Register), testSignal.From, testSignal.To, fileId);

                        // increment progress bar strip on the "Button"
                        IoC.Commands.TestProgress = Convert.ToDouble(progressStep);
                        
                        // increment progress
                        progressStep++;

                        // inform the developer about test register and start value.
                        IoC.Logger.Log($"Register: {IoC.TestDetails.Register} --- Test value: {testStartValue:F3} completed", LogLevel.Informative);

                        // inform the user about test register and start value.
                        IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Register: {IoC.TestDetails.Register} --- Test value: {testStartValue:F3} completed.\n";

                        // inform the developer
                        IoC.Logger.Log($"Test {progressStep} of {IoC.Commands.MaximumTestCount} completed",LogLevel.Informative);

                        // increment progress percentage
                        Progress = progressStep / IoC.Commands.MaximumTestCount;

                        // update the developer about progress
                        IoC.Logger.Log($"Min value: {MinTestValue} --- Max value: {MaxTestValue} --- Progress : {Progress * 100d:F2}% completed", LogLevel.Informative);

                        // reset min test value for the next test range
                        MinTestValue = 0;

                        // reset max test value for the next test range
                        MaxTestValue = 0;

                        // reset message for the next test step
                        Message = default(string);

                    }
                    else
                    {
                        // if timer is initialized
                        if (!MdbusTimer.Equals(null))
                            // terminate reading modbus register because the user canceled the test.
                            MdbusTimer.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {

                // inform developer
                IoC.Logger.Log($"Exception: {ex.Message}");

                // update the user about failed test.
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Test failed: {ex.Message}.\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the user about more details about error.
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }

                // Trying to stop the app gracefully.
                await Task.Run(() => ProcessErrors(false));
            }
            finally
            {
                // Trying to stop the app gracefully.
                await Task.Run(() => ProcessErrors(true));
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Send string commands to Omicron Test Set
        /// </summary>
        /// <param name="testSignal">a signal that From and To values are not equal</param>
        /// <param name="testStartValue">From value that test starts and increments per <see cref="AnalogSignalListItemViewModel.Delta"/></param>
        private void SendOmicronCommands(TestSignal testSignal, double testStartValue)
        {
            // TODO: Move this methods to its own class and automate

            // inform developer
            IoC.Logger.Log($"SendOmicronCommands started :  ramping signal: {testSignal.SignalName} -- test value: {testStartValue}",LogLevel.Informative);

            // set voltage amplifiers default values.
            // Analog signal: Voltage Output 1:
            IoC.StringCommands.SendOutAna
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.v,
                // generator 
                generatorNumber: "1:1",
                // Signal Amplitude
                amplitude: string.Equals("v1", testSignal.SignalName) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].From),
                // Signal Phase
                phase: string.Equals("v1", testSignal.SignalName) ? testSignal.Phase : Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Phase),
                // Signal Frequency
                frequency: string.Equals("v1", testSignal.SignalName) ? testSignal.Frequency : Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Frequency)
                );

            // Analog signal: Voltage Output 2:
            IoC.StringCommands.SendOutAna
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.v,
                // generator 
                generatorNumber: "1:2",
                 // Signal Amplitude
                 amplitude: string.Equals("v2", testSignal.SignalName) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[1].From),
               // Signal Phase
               phase: string.Equals("v2", testSignal.SignalName) ? testSignal.Phase : Convert.ToDouble(IoC.TestDetails.AnalogSignals[1].Phase),
                // Signal Frequency
                frequency: string.Equals("v2", testSignal.SignalName) ? testSignal.Frequency : Convert.ToDouble(IoC.TestDetails.AnalogSignals[1].Frequency));

            // Analog signal: Voltage Output 3:
            IoC.StringCommands.SendOutAna
                (
                   // Omicron Test Set internal generator name
                   generator: (int)StringCommands.GeneratorList.v,
                   // generator 
                   generatorNumber: "1:3",
                     // Signal Amplitude
                     amplitude: string.Equals("v3", testSignal.SignalName) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[2].From),
                   // Signal Phase
                   phase: string.Equals("v3", testSignal.SignalName) ? testSignal.Phase : Convert.ToDouble(IoC.TestDetails.AnalogSignals[2].Phase),
                    // Signal Frequency
                    frequency: string.Equals("v3", testSignal.SignalName) ? testSignal.Frequency : Convert.ToDouble(IoC.TestDetails.AnalogSignals[2].Frequency));

            // set current amplifiers default values.
            // Analog signal: Current Output 1:
            IoC.StringCommands.SendOutAna
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.i,
                // generator 
                generatorNumber: "1:1",
                // Signal Amplitude
                amplitude: string.Equals("i1", testSignal.SignalName) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[3].From),
               // Signal Phase
               phase: string.Equals("i1", testSignal.SignalName) ? testSignal.Phase : Convert.ToDouble(IoC.TestDetails.AnalogSignals[3].Phase),
                // Signal Frequency
                frequency: string.Equals("i1", testSignal.SignalName) ? testSignal.Frequency : Convert.ToDouble(IoC.TestDetails.AnalogSignals[3].Frequency));

            // Analog signal: Current Output 2:
            IoC.StringCommands.SendOutAna
                (
               // Omicron Test Set internal generator name
               generator: (int)StringCommands.GeneratorList.i,
               // generator 
               generatorNumber: "1:2",
                // Signal Amplitude
                amplitude: string.Equals("i2", testSignal.SignalName) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[4].From),
              // Signal Phase
              phase: string.Equals("i2", testSignal.SignalName) ? testSignal.Phase : Convert.ToDouble(IoC.TestDetails.AnalogSignals[4].Phase),
               // Signal Frequency
               frequency: string.Equals("i2", testSignal.SignalName) ? testSignal.Frequency : Convert.ToDouble(IoC.TestDetails.AnalogSignals[4].Frequency));

            // Analog signal: Current Output 3:
            IoC.StringCommands.SendOutAna
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.i,
                // generator 
                generatorNumber: "1:3",
                // Signal Amplitude
                amplitude: string.Equals("i3", testSignal.SignalName) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[5].From),
                // Signal Phase
                phase: string.Equals("i3", testSignal.SignalName) ? testSignal.Phase : Convert.ToDouble(IoC.TestDetails.AnalogSignals[5].Phase),
              // Signal Frequency
              frequency: string.Equals("i3", testSignal.SignalName) ? testSignal.Frequency : Convert.ToDouble(IoC.TestDetails.AnalogSignals[5].Frequency));
            
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
        private async void MeasurementIntervalCallbackAsync(object Register)
        {
            // read Modbus register in interval that specified by the user 
            try
            {

                // if a cancellation requested stop reading register
                if (IoC.Commands.Token.IsCancellationRequested)
                    return;

                // convert register string to integer.
                int register = Convert.ToInt32(Register);

                // verify the register is a legit
                if (register >= 0 && register <= 65536)
                {

                    // start a task to read register address specified by the user.
                    await Task.Run(async () =>
                    {
                        // start a task to read holding register (Function 0x03)
                        int[] serverResponse = await Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(register - 1, 1), IoC.Commands.Token);

                        // decide if serverResponse is acceptable only criteria is the length of the response.
                        if (serverResponse.Length > 0)
                        {
                            // establish minimum and maximum values.
                            for (int i = 0; i < serverResponse.Length; i++)
                            {
                                // update minimum value with new value if new value is less or minimum value was 0
                                if (MinTestValue > serverResponse[i] || MinTestValue == 0)
                                {
                                    // update minimum value
                                    MinTestValue = serverResponse[i];
                                }

                                // update maximum value with new value if new value is less or maximum value was 0
                                if (MaxTestValue < serverResponse[i] || MaxTestValue == 0)
                                {
                                    // update maximum value
                                    MaxTestValue = serverResponse[i];
                                }
                            }
                        }
                        else
                        {
                            //TODO: server failed to respond. Ignoring it until find a better option.
                        }

                    }, IoC.Commands.Token);
                }
                else
                {
                    // illegal register address
                    // Trying to stop the app gracefully.
                    await Task.Run(() => ProcessErrors(false));
                }

            }
            catch (Exception ex)
            {
                // update the user about the error.
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // update the user.
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }

                // Trying to stop the app gracefully.
                await Task.Run(() => ProcessErrors(false));
            }
        }

        #endregion
    }
}