using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        #endregion

        #region Public Properties

        /// <summary>
        /// Timer ticks used to read ModbusClient
        /// </summary>
        public Timer MdbusTimer { get; set; }

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
        /// holds information about the test completion.
        /// if true test ran and completed without any interruption
        /// else the user interrupted the test
        /// </summary>
        public bool IsTestRunning { get; set; }

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
        /// Runs Test Steps
        /// </summary>
        /// <returns>Returns nothing</returns>
        public async Task TestAsync(StringBuilder Message)
        {
            try
            {

                // update test progress
                int progressStep = default(int);

                // Wait StartDelayTime to start Modbus communication
                IoC.Task.Run(async () =>
                {
                    // lock the task
                    await AsyncAwaiter.AwaitAsync(nameof(TestAsync), async () =>
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

                // loads TestSignal information as a Tuple
                var (SignalName, From, To, Delta, Phase, Frequency) = testSignal.GetRampingSignal();

                // initialize new testStartValue
                double testStartValue = default(double);

                // verify we have a ramping signal
                if (string.IsNullOrWhiteSpace(SignalName))
                {
                    // inform the user there is no test case
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: There is no ramping signal.Please check your entries.";

                    // return from this task.
                    return;
                }

                // create a TestResultLogger to generate a test report in .csv format.
                var testResultLog = new TestResultLogger
                    (
                        // set file path and name
                        filePath: Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "metering"), $"{IoC.TestDetails.Register}_{From:F3}-{To:F3}_{fileId}.csv"),
                        // no need to save time
                        logTime: false
                    );

                // inform the developer of test SignalName
                IoC.Logger.Log($"Test signal name: {SignalName}", LogLevel.Informative);

                // set maximum value for the progress bar
                IoC.Commands.MaximumTestCount = Math.Ceiling((Math.Abs(To - From) / Delta)) + 1;

                // inform the developer MaximumTestCount
                IoC.Logger.Log($"Maximum test count: {IoC.Commands.MaximumTestCount}", LogLevel.Informative);

                // Process test steps
                // due to nature of double calculations use this formula "testStartValue <= (To + Delta * 1 / 1000)" to make sure the last test step always runs
                for (testStartValue = From; testStartValue <= (To + Delta * 1 / 1000); testStartValue += Delta)
                {
                    // check if the user canceled the tests.
                    if (!IoC.Commands.Token.IsCancellationRequested)
                    {

                        // set timer to read modbus register per the user specified time.
                        MdbusTimer = new Timer(MeasurementIntervalCallbackAsync, IoC.TestDetails.Register, TimeSpan.FromSeconds(Convert.ToDouble(IoC.TestDetails.StartMeasurementDelay)), TimeSpan.FromMilliseconds(Convert.ToDouble(IoC.TestDetails.MeasurementInterval)));

                        // inform the developer about test register and start value.
                        IoC.Logger.Log($"Register: {IoC.TestDetails.Register}\tTest value: {testStartValue:F3} started", LogLevel.Informative);

                        // inform the user about test register and start value.
                        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Register: {IoC.TestDetails.Register} --- Test value: {testStartValue:F3} started";

                        // send string commands to Omicron
                        await IoC.Task.Run(() => SendOmicronCommands(SignalName, testStartValue));

                        // Turn On Omicron Analog Outputs per the user input
                        await IoC.Task.Run(() => IoC.PowerOptions.TurnOnCMC());

                        // Start reading the user specified Register
                        IoC.Task.Run(async () =>
                        {

                            // lock the task
                            await AsyncAwaiter.AwaitAsync(nameof(TestAsync), async () =>
                            {

                                // wait until the user specified "Dwell Time" expires.
                                await Task.Delay(TimeSpan.FromSeconds(Convert.ToDouble(IoC.TestDetails.DwellTime)));
                            });

                            // terminate reading modbus register because "Dwell Time" is over.
                            MdbusTimer.Dispose();

                            // inform the user about test results.
                            IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Min value: {MinTestValue} Max value: {MaxTestValue}";

                            // generate a string to inform the user about test results.
                            Message.Append($"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff},{IoC.TestDetails.Register},{testStartValue:F3},{MinTestValue:F3},{MaxTestValue:F3}");

                            // wait task to be over with
                        }, IoC.Commands.Token).Wait();

                        // update test result report
                        testResultLog.Log(Message.ToString(), LogLevel.Informative);

                        // increment progress bar strip on the "Button"
                        IoC.Commands.TestProgress = Convert.ToDouble(progressStep);

                        // increment progress
                        progressStep++;

                        // inform the developer about test register and start value.
                        IoC.Logger.Log($"Register: {IoC.TestDetails.Register} --- Test value: {testStartValue:F3} completed", LogLevel.Informative);

                        // inform the user about test register and start value.
                        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Register: {IoC.TestDetails.Register} --- Test value: {testStartValue:F3} completed.";

                        // inform the developer
                        IoC.Logger.Log($"Test {progressStep} of {IoC.Commands.MaximumTestCount} completed", LogLevel.Informative);

                        // increment progress percentage
                        Progress = progressStep / IoC.Commands.MaximumTestCount;

                        // update the developer about progress
                        IoC.Logger.Log($"Min value: {MinTestValue} --- Max value: {MaxTestValue} --- Progress : {Progress * 100d:F2}% completed", LogLevel.Informative);

                        // reset min test value for the next test range
                        MinTestValue = 0;

                        // reset max test value for the next test range
                        MaxTestValue = 0;

                        // reset message for the next test step
                        Message.Clear();

                    }
                    else
                    {
                        // if timer is initialized
                        if (!MdbusTimer.Equals(null))
                            // terminate reading modbus register because the user canceled the test.
                            MdbusTimer.Dispose();

                        // exit from this task
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

                // inform developer
                IoC.Logger.Log($"Exception: {ex.Message}");

                // update the user about failed test.
                IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Test failed: {ex.Message}.";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the user about more details about error.
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.";
                }

                // Trying to stop the app gracefully.
                await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrors());

                // exit from this task
                return;
            }
            finally
            {
                // Trying to stop the app gracefully.
                await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrors(false));
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Send string commands to Omicron Test Set
        /// </summary>
        /// <param name="testSignalName">the signal that From and To values are not equal</param>
        /// <param name="testStartValue">From value that test starts and increments per <see cref="AnalogSignalListItemViewModel.Delta"/></param>
        private void SendOmicronCommands(string testSignalName, double testStartValue)
        {
            // TODO: Move this methods to its own class and automate

            // inform developer
            IoC.Logger.Log($"SendOmicronCommands started :  ramping signal: {testSignalName} -- test value: {testStartValue}", LogLevel.Informative);

            // set voltage amplifiers default values.
            // Analog signal: Voltage Output 1:
            IoC.StringCommands.SendOutAnaAsync
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.v,
                // generator 
                generatorNumber: "1:1",
                // Signal Amplitude
                amplitude: (string.Equals("v1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Magnitude),
                // Signal Phase
                phase: (string.Equals("v1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Phase),
                // Signal Frequency
                frequency: (string.Equals("v1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Frequency)
                );

            // Analog signal: Voltage Output 2:
            IoC.StringCommands.SendOutAnaAsync
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.v,
                // generator 
                generatorNumber: "1:2",
                 // Signal Amplitude
                 amplitude: (string.Equals("v2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[1].Magnitude),
                // Signal Phase
                phase: (string.Equals("v2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[1].Phase),
                // Signal Frequency
                frequency: (string.Equals("v2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[1].Frequency)
                );

            // Analog signal: Voltage Output 3:
            IoC.StringCommands.SendOutAnaAsync
                (

                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.v,
                // generator 
                generatorNumber: "1:3",
                // Signal Amplitude
                amplitude: (string.Equals("v3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[2].Magnitude),
                // Signal Phase
                phase: (string.Equals("v3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[2].Phase),
                // Signal Frequency
                frequency: (string.Equals("v3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[2].Frequency)
                );

            // set current amplifiers default values.
            // Analog signal: Current Output 1:
            IoC.StringCommands.SendOutAnaAsync
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.i,
                // generator 
                generatorNumber: "1:1",
                // Signal Amplitude
                amplitude: (string.Equals("i1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[3].Magnitude),
                // Signal Phase
                phase: (string.Equals("i1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[3].Phase),
                // Signal Frequency
                frequency: (string.Equals("i1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[3].Frequency)
                );

            // Analog signal: Current Output 2:
            IoC.StringCommands.SendOutAnaAsync
                (
               // Omicron Test Set internal generator name
               generator: (int)StringCommands.GeneratorList.i,
               // generator 
               generatorNumber: "1:2",
               // Signal Amplitude
               amplitude: (string.Equals("i2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[4].Magnitude),
                // Signal Phase
                phase: (string.Equals("i2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[4].Phase),
                // Signal Frequency
                frequency: (string.Equals("i2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[4].Frequency)
                );

            // Analog signal: Current Output 3:
            IoC.StringCommands.SendOutAnaAsync
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.i,
                // generator 
                generatorNumber: "1:3",
                // Signal Amplitude
                amplitude: (string.Equals("i3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[5].Magnitude),
                // Signal Phase
                phase: (string.Equals("i3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[5].Phase),
                // Signal Frequency
                frequency: (string.Equals("i3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[5].Frequency)
                );

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
                if (register >= 0 && register <= 65535)
                {

                    // start a task to read register address specified by the user.
                    await IoC.Task.Run(async () =>
                    {
                        // start a task to read holding register (Function 0x03)
                        int[] serverResponse = await IoC.Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(register - 1, 1), IoC.Commands.Token);

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
                    throw new ArgumentOutOfRangeException($"Register: {register} is out of range");

                    // await IoC.Task.Run(() => ProcessErrors(false));
                }

            }
            catch (Exception ex)
            {
                // inform the developer about error
                IoC.Logger.Log($"Exception is : {ex.Message}");

                // update the user about the error.
                IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the developer about error
                    IoC.Logger.Log($"InnerException is : {ex.Message}");

                    // update the user.
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.";
                }

                // Trying to stop the app gracefully.
                await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrors());
            }
        }

        #endregion
    }
}