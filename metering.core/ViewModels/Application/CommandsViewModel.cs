using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;

namespace metering.core
{
    /// <summary>
    /// Handles Commands page.
    /// </summary>
    public class CommandsViewModel : BaseViewModel
    {
        #region Private Members

        #endregion

        #region Public Properties

        /// <summary>
        /// Cancellation token source for Omicron Test Set to stop and power down Omicron Test Set.
        /// </summary>
        public CancellationTokenSource TokenSource { get; private set; }

        /// <summary>
        /// Cancellation token for Omicron Test Set to stop and power down Omicron Test Set.
        /// </summary>
        public CancellationToken Token { get; set; }

        /// <summary>
        /// True if the user hit "+" button
        /// </summary>
        public bool NewTestAvailable { get; set; } = false;

        /// <summary>
        /// Holds Foreground color information for the Start Test Command button
        /// </summary>
        public string StartTestForegroundColor { get; set; }

        /// <summary>
        /// Holds Foreground color information for the Cancel Command button
        /// </summary>
        public string CancelForegroundColor { get; set; }

        /// <summary>
        /// Set ProgressAssist IsIndicatorVisible on the floating StartTestCommand button
        /// </summary>
        public bool IsConnecting { get; set; }

        /// <summary>
        /// To change icon on the floating StartTestCommand button
        /// </summary>
        public bool IsConnectionCompleted { get; set; }
        /// <summary>
        /// Progress percentage of the test completion
        /// </summary>
        public double TestProgress { get; set; }

        /// <summary>
        /// Sets maximum value for the progress bar around StartTestCommand button
        /// </summary>
        public double MaximumTestCount { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to handle change view to test plan detail view
        /// and populate items with nominal values
        /// </summary>
        public ICommand AddNewTestCommand { get; set; }

        /// <summary>
        /// The command handles canceling New Test addition view and returns default view
        /// </summary>
        public ICommand CancelNewTestCommand { get; set; }

        /// <summary>
        /// The command to handle connecting associated Omicron Test Set
        /// and communication to the UUT
        /// </summary>
        public ICommand StartTestCommand { get; set; }

        /// <summary>
        /// The command to handle saving test step to the user specified location
        /// default location interface opens at "\\my documents\\metering\\tests"
        /// </summary>
        public ICommand SaveNewTestCommand { get; set; }

        /// <summary>
        /// The command to handle loading test step(s) from the user specified location
        /// default location interface opens at "\\my documents\\metering\\tests"
        /// </summary>
        public ICommand LoadTestsCommand { get; set; }

        /// <summary>
        /// The command to handle removing test step(s) from the test strip in the <see cref="CommunicationViewModel"/>
        /// this command will not delete any test step from folder that located at "\\my documents\\metering\\tests"
        /// </summary>
        public ICommand DeleteSelectedTestCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandsViewModel()
        {
            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            // Show a new Test details page populated with the user specified/accepted values
            AddNewTestCommand = new RelayCommand(() => IoC.NominalValues.CopyNominalValues());

            // starts the test specified by the user.
            StartTestCommand = new RelayCommand(async () => await ConnectOmicronAndUnitAsync());

            // navigate back to nominal values page.
            CancelNewTestCommand = new RelayCommand(() => CancelTestDetailsPageShowing());

            // save the test step to the user specified location.
            SaveNewTestCommand = new RelayCommand(async () => await SaveTestStepsAsync());

            // load the test step(s) from the user specified location.
            LoadTestsCommand = new RelayCommand(async () => await LoadTestsAsync());

            //// remove the test step(s) from the test strip.
            //DeleteSelectedTestCommand = new RelayCommand(async () => await DeleteSelectedTestAsync());

            // default StartTestForegroundColor is Green
            StartTestForegroundColor = "00ff00";

            // default CancelForegroundColor is Green
            CancelForegroundColor = "00ff00";
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Load a saved <see cref="TestDetailsViewModel"/> to the screen.
        /// </summary>
        private async Task LoadTestsAsync()
        {
            // retrieve file name
            string fileName = await IoC.Commander.LoadNewTestsAsync();

            // the user press OK button
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                {
                    // initialize a new TestDetailsViewModel
                    TestDetailsViewModel test = new TestDetailsViewModel();

                    // de-serialize a JSON file to a TestDetailsViewModel to show it the user 
                    using (StreamReader file = File.OpenText(fileName))
                    {
                        // initialize JsonSerializer to de-serialize directly from the file
                        JsonSerializer serializer = new JsonSerializer();

                        // convert a de-serialize json to TestDetailsViewModel
                        test = (TestDetailsViewModel)serializer.Deserialize(file, typeof(TestDetailsViewModel));
                    }

                    // Update values in the single instance of TestDetailsViewModel
                    // update AnalogSignals
                    IoC.TestDetails.AnalogSignals = test.AnalogSignals;

                    // update Register
                    IoC.TestDetails.Register = test.Register;

                    // update DwellTime
                    IoC.TestDetails.DwellTime = test.DwellTime;

                    // update StartDelayTime
                    IoC.TestDetails.StartDelayTime = test.StartDelayTime;

                    // update MeasurementInterval
                    IoC.TestDetails.MeasurementInterval = test.MeasurementInterval;

                    // update StartMeasurementDelay
                    IoC.TestDetails.StartMeasurementDelay = test.StartMeasurementDelay;

                    // update SelectedRampingSignal
                    IoC.TestDetails.SelectedRampingSignal = test.SelectedRampingSignal;

                    // Show TestDetails page
                    IoC.Application.GoToPage(ApplicationPage.TestDetails, IoC.TestDetails);
                }
            }
        }

    /// <summary>
    /// Save current <see cref="TestDetailsViewModel"/> to the user specified location
    /// </summary>
    private async Task SaveTestStepsAsync()
    {
        // retrieve file name
        string fileName = await IoC.Commander.SaveNewTestAsync();

        // the user press OK button
        if (!string.IsNullOrWhiteSpace(fileName))
        {
            // generate a new Test Steps Logger
            new TestStepsLogger(
                // file location and name as specified by the user
                filePath: fileName,
                // don't need to save time
                logTime: false,
                // test details need to be saved
                test: IoC.TestDetails
                );
        }
        // the user press Cancel button.
        else
            IoC.Logger.Log($"the user press Cancel button.");

    }

    /// <summary>
    /// Navigate backwards to main view / shows default nominal values
    /// resets values specified in test step view to nominal values
    /// </summary>
    private void CancelTestDetailsPageShowing()
    {

        // reset maximum value for progress bar for the next run
        MaximumTestCount = 0d;

        // change CancelForegroundColor to Red
        CancelForegroundColor = "00ff00";

        // set visibility of the Command Buttons
        NewTestAvailable = false;

        // clear Test details view model
        IoC.Application.CurrentPageViewModel = null;

        // reset test progress to show test canceled.
        TestProgress = 0d;

        // reset StartTestCommand button icon
        IsConnectionCompleted = false;

        // Update NominalValues RadioButtons to run a PropertyUpdate event
        IoC.NominalValues.SelectedVoltagePhase = "AllZero";
        IoC.NominalValues.SelectedCurrentPhase = "AllZero";

        // Show NominalValues page
        IoC.Application.GoToPage(ApplicationPage.NominalValues);

        // if the test completed normally no need to cancel token as it is canceled already
        if (IoC.CMCControl.IsTestRunning)
        {
            // check if Omicron Test Set is running
            // if the user never started test and press "Back" button 
            // DeviceID would be a zero
            if (IoC.CMCControl.DeviceID > 0)
            {
                // try to cancel thread running Omicron Test Set
                TokenSource.Cancel();

                // try to stop Omicron Test Set gracefully
                IoC.ReleaseOmicron.ProcessErrors(true);
            }
        }
    }

    /// <summary>
    /// connects to omicron and test unit.
    /// </summary>    
    private async Task ConnectOmicronAndUnitAsync()
    {
        // there is a test set attached so run specified tests.
        // lock the task
        await AsyncAwaiter.AwaitAsync(nameof(ConnectOmicronAndUnitAsync), async () =>
        {

                // define the cancellation token source.
                TokenSource = new CancellationTokenSource();

                // define the cancellation token to use 
                // terminate tests prematurely.
                Token = TokenSource.Token;

                // Run test command
                await IoC.Task.Run(() => IoC.TestDetails.ConnectCommand.Execute(IoC.TestDetails), Token);
        });
    }

    #endregion
}
}
