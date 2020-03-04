using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                // if engine is null return a new engine
                if (engine == null)
                    return new CMEngine.CMEngine();

                // return engine
                return engine;
            }
            // set engine
            set => engine = value;
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
        public int[] MinValues { get; set; }

        /// <summary>
        /// Holds maximum value for test register.
        /// </summary>
        public int[] MaxValues { get; set; }

        /// <summary>
        /// Holds total value for test register.
        /// </summary>
        public int[] Totals { get; set; }

        /// <summary>
        /// Holds average value for test register.
        /// </summary>
        public double[] Averages { get; set; }

        /// <summary>
        /// Holds standard deviation value for test register.
        /// </summary>
        public double[] StandardDeviations { get; set; }

        /// <summary>
        /// Holds every legit read values for the registers
        /// </summary>
        public List<StringBuilder> IndivudalRegisters { get; set; }

        /// <summary>
        /// Holds every legit read values for the registers with time stamp.
        /// </summary>
        public List<SortedDictionary<DateTime, int>> RegisterReadingsWithTime { get; set; }

        /// <summary>
        /// Holds successful reading number of the test register.
        /// </summary>
        public int[] SuccessCounters { get; set; }

        /// <summary>
        /// Hold progress information per test register.
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// Associated Omicron Test Set ID. Assigned by CM Engine.
        /// </summary>
        public int DeviceID { get; set; }

        /// <summary>
        /// Associated Omicron Test Set Information. Assigned by Omicron.
        /// </summary>
        public string DeviceInfo { get; set; }

        /// <summary>
        /// Omicron Test Set debugging log levels.
        /// </summary>
        public enum OmicronLoggingLevels : short { None, Level1, Level2, Level3, };

        /// <summary>
        /// Holds the test folder location
        /// </summary>
        public string TestsFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "metering\\tests"));

        /// <summary>
        /// Holds the results folder location
        /// </summary>
        public string ResultsFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "metering\\results"));

        /// <summary>
        /// Holds the logs folder location.
        /// Omicron logs and app logs.
        /// </summary>
        public string LogsFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "metering\\logs"));
        #endregion

        #region Public Methods


        /// <summary>
        /// Runs Test Steps
        /// </summary>
        /// <returns>Returns nothing</returns>
        public async Task TestAsync(CancellationToken cancellationToken)
        {

            try
            {

                // check if the user trying to run a non-saved test?
                if (1 > IoC.Communication.TestFileListItems.Count)
                {
                    // generate a dummy test file, save and load it 
                    await IoC.Commander.SaveDummyTestFileAsync();
                }

                // scan multiple tests input for the test
                for (int testFileNumber = 0; testFileNumber < IoC.Communication.TestFileListItems.Count(); testFileNumber++)
                {
                    // retrieve file name with extension without file location
                    IoC.Communication.TestFileListItems[testFileNumber].TestFileNameWithExtension = Path.GetFileName(IoC.Communication.TestFileListItems[testFileNumber].FullFileName);

                    // load the test file into memory
                    IoC.Commander.LoadTestFile(testFileNumber);

                    // update In Progress test file
                    IoC.Communication.CurrentTestFileListItem = IoC.Communication.TestFileListItems[testFileNumber];

                    // modify physical appearance of the Test File list and tool tip
                    IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.InProgress);

                    // update test progress
                    int progressStep = default;

                    // check if the test is harmonic
                    // yes => include harmonics portion of the test
                    // no => move to core metering testing
                    // loads TestSignal information as a Tuple
                    var (HarmonicStart, HarmonicEnd, HarmonicOrders, TotalHarmonicTestCount) = new TestHarmonics().GetHarmonicOrder();

                    // decides which signal is our ramping signal by comparing the mismatch of any "From" and "To" values.
                    // after the user clicked "Go" button
                    // loads TestSignal information as a Tuple
                    var (SignalName, From, To, Delta, Phase, Frequency, Precision) = new TestSignal().GetRampingSignal();

                    // initialize new testStartValue
                    double testStartValue = default;

                    //// precision format text to format strings
                    //string precision = $"F{Precision}";

                    // verify we have a ramping signal
                    if (string.IsNullOrWhiteSpace(SignalName))
                    {
                        // inform the user there is no test case
                        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: There is no ramping signal.Please check your entries.";

                        // return from this task.
                        return;
                    }

                    // inform the developer of test SignalName
                    await Task.Run(() => IoC.Logger.Log($"Test signal name: {SignalName}", LogLevel.Informative));

                    // set maximum value for the progress bar.
                    IoC.Commands.MaximumTestCount = TotalHarmonicTestCount * (Math.Ceiling((Math.Abs(To - From) / Delta)) + 1);

                    // overall every harmonic test
                    foreach (var harmonicTest in HarmonicOrders)
                    {

                        // if there is no harmonics test this should run once.
                        // if there is harmonics test should run From "HarmonicStart" To "HarmonicEnd"
                        for (int orderNumber = HarmonicStart[HarmonicOrders.IndexOf(harmonicTest)]; orderNumber < HarmonicEnd[HarmonicOrders.IndexOf(harmonicTest)] + 1; orderNumber++)
                        {

                            // report file id to distinguish between test results 
                            string fileId = $"{DateTime.Now.ToLocalTime():yyyy_MM_dd_HH_mm}";

                            // inform the developer MaximumTestCount
                            await Task.Run(() => IoC.Logger.Log($"Maximum test count: {IoC.Commands.MaximumTestCount}", LogLevel.Informative));

                            // Wait StartDelayTime to start Modbus communication
                            await IoC.Task.Run(async () =>
                            {
                                // obtain time delay from UI.
                                TimeSpan delayTime = TimeSpan.FromMinutes(Convert.ToDouble(IoC.TestDetails.StartDelayTime));

                                // wait with cancellation token
                                await Task.Delay(delay: delayTime, cancellationToken: cancellationToken);
                            });

                            // set Progress bar is visible
                            IoC.Commands.IsConnectionCompleted = IoC.Commands.IsConnecting = IoC.Communication.EAModbusClient.Connected;

                            // change color of Cancel Command button to Green
                            IoC.Commands.CancelForegroundColor = "00ff00";

                            // new test starts if it is not harmonic
                            if (2 >= TotalHarmonicTestCount)
                            {
                                progressStep = 0;
                            }

                            // update testing harmonic number for file naming
                            IoC.Communication.TestingHarmonicOrder = orderNumber;

                            // generate fixed portion of header information for reporting.
                            StringBuilder @string = new StringBuilder("Time, Test Value");

                            // generate variable portion of header information based on Register entry.
                            IEnumerable<int> registerValues = Enumerable.Range(0, IoC.TestDetails.Register.ToString().Split(',').Length).Select(i => i);

                            // every register holder initialization
                            IndivudalRegisters = new List<StringBuilder>();
                            // generate headers for the reporting.
                            foreach (int registerValue in registerValues)
                            {
                                // append new variable header information to previous string generated.
                                @string.Append($" ,Register {registerValue + 1}, Minimum Value {registerValue + 1}, Maximum Value {registerValue + 1}, Average Value {registerValue + 1}, StDev Value {registerValue + 1},Good Reading Value {registerValue + 1}");

                                // add register holders
                                IndivudalRegisters.Add(new StringBuilder());
                            }

                            // add new line to generated string for the test values.
                            @string.AppendLine();

                            // Process test steps
                            // due to nature of double calculations use this formula "testStartValue <= (To + Delta * 1 / 1000)" to make sure the last test step always runs
                            for (testStartValue = From; testStartValue <= (To + Delta * Math.Pow(10, -Precision)); testStartValue += Delta)
                            {

                                // inform the developer about test register and start values.
                                await Task.Run(() => IoC.Logger.Log($"Register: {IoC.TestDetails.Register}\tTest value: {testStartValue:F6} started", LogLevel.Informative));

                                // inform the user about test register and start values.
                                IoC.Communication.Log = await Task.Run(() => $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Register: {IoC.TestDetails.Register} --- Test value: {testStartValue:F6} started");

                                // initialize Minimum Value List with the highest int32 to capture every minimum value
                                MinValues = Enumerable.Repeat(int.MaxValue, IoC.TestDetails.Register.ToString().Split(',').Length).ToArray();

                                // initialize Maximum Value List with the lowest int32 to capture every maximum value
                                MaxValues = Enumerable.Repeat(int.MinValue, IoC.TestDetails.Register.ToString().Split(',').Length).ToArray();

                                // initialize totals with 0.
                                Totals = Enumerable.Repeat(0, IoC.TestDetails.Register.ToString().Split(',').Length).ToArray();

                                // initialize average with 0.
                                Averages = Enumerable.Repeat(0.0, IoC.TestDetails.Register.ToString().Split(',').Length).ToArray();

                                // initial successful reading with 0.
                                SuccessCounters = Enumerable.Repeat(0, IoC.TestDetails.Register.ToString().Split(',').Length).ToArray();

                                // initial successful reading with 0.
                                StandardDeviations = Enumerable.Repeat(0.0, IoC.TestDetails.Register.ToString().Split(',').Length).ToArray();

                                // initialize RegisterReadingsWithTime with null values.
                                RegisterReadingsWithTime = new List<SortedDictionary<DateTime, int>>();                                
                                // add RegisterReadings the user specified register space holders
                                for (int i = 0; i < IoC.TestDetails.Register.ToString().Split(',').Length; i++)
                                {
                                    // initialize a blank register space holders.
                                    SortedDictionary<DateTime, int> registerReadings = new SortedDictionary<DateTime, int>();

                                    // add the register holder
                                    RegisterReadingsWithTime.Add(registerReadings);
                                }

                                // send string commands to Omicron
                                await IoC.Task.Run(() => IoC.SetOmicron.SendOmicronCommands(SignalName, testStartValue));

                                // delay little bit Omicron CMC power up
                                await Task.Delay(1000);

                                // Turn On Omicron Analog Outputs per the user input
                                await IoC.Task.Run(async () => await IoC.PowerOptions.TurnOnCMCAsync());

                                // set timer to read modbus register per the user specified time.
                                MdbusTimer = await Task.Run(() =>
                                                new Timer(
                                                    // reads the user specified modbus register(s).
                                                    callback: IoC.Modbus.MeasurementIntervalCallback,
                                                    // pass the use specified modbus register(s) to callback.
                                                    state: IoC.TestDetails.Register,
                                                    // the time delay before modbus register(s) read start. 
                                                    dueTime: TimeSpan.FromSeconds(Convert.ToDouble(IoC.TestDetails.StartMeasurementDelay)),
                                                    // the interval between reading of the modbus register(s).
                                                    period: TimeSpan.FromMilliseconds(Convert.ToDouble(IoC.TestDetails.MeasurementInterval))), cancellationToken);

                                // wait until the user specified "Dwell Time" expires.
                                await Task.Delay(TimeSpan.FromSeconds(Convert.ToDouble(IoC.TestDetails.DwellTime) + Convert.ToDouble(IoC.TestDetails.StartMeasurementDelay)), cancellationToken);

                                // terminate reading modbus register because "Dwell Time" is over.
                                MdbusTimer?.Dispose();

                                // Time Test Value Register 1  Min Value 1 Max Value 1 Register 2  Min Value 2 Max Value 2 Register 3  Min Value 3 Max Value 3 ... etc.
                                await Task.Run(() => @string.Append(value: $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff},{testStartValue:F6}"));

                                // calculate Std. Deviation
                                new CalculateStdDeviation().CalculateStdDeviationWithTime(RegisterReadingsWithTime, testStartValue);

                                // generate variable portion of header information based on Register entry.
                                foreach (int i in registerValues)
                                {
                                    // append new variable header information to previous string generated.
                                    await Task.Run(() => @string.Append(value: $" ,{IoC.TestDetails.Register.ToString().Split(',').GetValue(i)} ,{MinValues[i]:F6} , {MaxValues[i]:F6}, {Averages[i]:F6}, {StandardDeviations[i]:F6}, {SuccessCounters[i]}"));
                                }

                                // update test result report
                                await IoC.Task.Run(() =>
                                {
                                    // initialize test details string.
                                    string testDetailsFileName = string.Empty;

                                    if (string.IsNullOrWhiteSpace(IoC.Communication.CurrentTestFileListItem.TestFileNameWithExtension))
                                    {
                                        // test result file name contains Register, From, To, and test start time values
                                        testDetailsFileName = $"{(IoC.TestDetails.IsHarmonics ? $"[{IoC.Communication.TestingHarmonicOrder.ToString()}]" : string.Empty)}{IoC.TestDetails.Register}_{From:F6}-{To:F6}_{fileId}";
                                    }
                                    else
                                    {
                                        // test result file name contains "Test File Name" per the user input.
                                        // file name might contain multiple "."
                                        testDetailsFileName = $"{(IoC.TestDetails.IsHarmonics ? $"[{IoC.Communication.TestingHarmonicOrder.ToString()}]" : string.Empty)}{IoC.Communication.CurrentTestFileListItem.ShortTestFileName}_{fileId}";
                                    }

                                    // create a TestResultLogger to generate a test report in .csv format.
                                    new TestResultLogger
                                                 (
                                                     // set file path and name
                                                     filePath: Path.Combine(ResultsFolder, $"{testDetailsFileName}.csv"),
                                                     // no need to save time
                                                     logTime: false
                                                 ).Log(@string.ToString(), LogLevel.Informative);
                                });

                                // increment progress bar strip on the "Button"
                                IoC.Commands.TestProgress = Convert.ToDouble(progressStep);

                                // increment progress
                                progressStep++;

                                // inform the developer about test register and start value.
                                await Task.Run(() => IoC.Logger.Log($"Register: {IoC.TestDetails.Register} --- Test value: {testStartValue:F6} completed", LogLevel.Informative));

                                // inform the user about test register and start value.
                                IoC.Communication.Log = await Task.Run(() => $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Register: {IoC.TestDetails.Register} --- Test value: {testStartValue:F6} completed.");

                                // inform the developer
                                await Task.Run(() => IoC.Logger.Log($"Test {progressStep} of {IoC.Commands.MaximumTestCount} completed", LogLevel.Informative));

                                // increment progress percentage
                                Progress = await Task.Run(() => progressStep / IoC.Commands.MaximumTestCount);

                                // reset message for the next test step
                                @string.Clear();

                                // delay little bit for communication to clear up
                                await Task.Delay(Convert.ToInt32(IoC.TestDetails.MeasurementInterval) * 2);
                            }
                        }
                    }

                    // Turn off Omicron Test set temporarily.
                    await IoC.Task.Run(async () => await IoC.PowerOptions.TurnOffCMCAsync());

                    // modify physical appearance of the Test File list and tool tip
                    IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Completed);

                    // generate result files per register 
                    foreach (var item in IndivudalRegisters)
                    {
                        // check if the user wants to save modbus reading details.
                        if (IoC.Communication.IsSaveHoldingRegisterDetailsChecked)
                        {

                            // report file id to distinguish between test results 
                            string fileId = $"{DateTime.Now.ToLocalTime():yyyy_MM_dd_HH_mm}";

                            // initialize test details string.
                            string testDetailsFileName = string.Empty;

                            if (string.IsNullOrWhiteSpace(IoC.Communication.CurrentTestFileListItem.TestFileNameWithExtension))
                            {
                                // test result file name contains Register, From, To, and test start time values
                                testDetailsFileName = $"{(IoC.TestDetails.IsHarmonics ? $"[{IoC.Communication.TestingHarmonicOrder.ToString()}]" : string.Empty)}{IoC.TestDetails.Register}_{StandardDeviations:F6}-{StandardDeviations:F6}_{fileId}";
                            }
                            else
                            {
                                // test result file name contains "Test File Name" per the user input.
                                // file name might contain multiple "."
                                testDetailsFileName = $"{(IoC.TestDetails.IsHarmonics ? $"[{IoC.Communication.TestingHarmonicOrder.ToString()}]" : string.Empty)}{IoC.Communication.CurrentTestFileListItem.ShortTestFileName}_{IoC.TestDetails.Register.ToString().Split(',').GetValue(IndivudalRegisters.IndexOf(item))}_{fileId}";
                            }

                            // create a TestResultLogger to generate a test report in .csv format.
                            new TestResultLogger
                                         (
                                             // set file path and name
                                             filePath: Path.Combine(ResultsFolder, $"{testDetailsFileName}.csv"),
                                             // no need to save time
                                             logTime: false
                                         ).Log(item.ToString(), LogLevel.Informative);

                        } 
                    }

                    // destruct TestFileListItem
                    IoC.Communication.CurrentTestFileListItem = null;
                }
            }
            catch (OperationCanceledException)
            {
                // re-throw error
                throw;
            }
            catch (Exception)
            {
                // re-throw error
                throw;
            }

        }

        #endregion

        #region Private Methods

        #endregion
    }
}