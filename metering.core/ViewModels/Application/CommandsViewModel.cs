using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace metering.core
{
    /// <summary>
    /// Handles Commands page.
    /// </summary>
    public class CommandsViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// True if the user hit "+" button
        /// </summary>
        public bool NewTestAvailable { get; set; } = false;

        #endregion

        #region Public Commands

        /// <summary>
        /// The command to handle change view to test plan detail view
        /// and populate items with nominal values
        /// </summary>
        public ICommand AddNewTestCommand { get; set; }
                
        /// <summary>
        /// The command handles cancelling New Test addition view and returns default view
        /// </summary>
        public ICommand CancelNewTestCommand { get; set; }

        /// <summary>
        /// The command to handle connecting associated Omicron Test Set
        /// and communication to the UUT
        /// </summary>
        public ICommand StartTestCommand { get; set; }

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

            // create the command.
            StartTestCommand = new RelayCommand(async () => await ConnectOmicronAndUnit());
            
            // navigate back to nominal values page.
            CancelNewTestCommand = new RelayCommand(() => CancelTestDetailsPageShowing());

            // default StartTestForegroundColor is Green
            StartTestForegroundColor = "00ff00";

            // default CancelForegroundColor is Green
            CancelForegroundColor = "00ff00";
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Navigate backwards to main view / shows default nominal values
        /// resets values specified in test step view to nominal values
        /// </summary>
        private void CancelTestDetailsPageShowing()
        {
            // change CancelForegroundColor to Red
            CancelForegroundColor = "00ff00";

            // set visibility of the Command Buttons
            NewTestAvailable = false;

            // clear Test details view model
            IoC.Application.CurrentPageViewModel = null;

            TestProgress = 0d;
            IsConnectionCompleted = false;

            // Update NominalValues RadioButtons to run a PropertyUpdate event
            IoC.NominalValues.SelectedVoltagePhase = "AllZero";
            IoC.NominalValues.SelectedCurrentPhase = "AllZero";

            // Show NominalValues page
            IoC.Application.GoToPage(ApplicationPage.NominalValues);
        }


        /// <summary>
        /// connects to omicron and test unit.
        /// </summary>    
        private async Task ConnectOmicronAndUnit()
        {

            await Task.Run(() => IoC.TestDetails.ConnectCommand.Execute(IoC.TestDetails));

            //await RunCommand(() => IoC.Communication.IsOmicronConnected, async () =>
            //{

            //    Debug.WriteLine($"register: {IoC.TestDetails.Register}");
            //    Debug.WriteLine($"ipdadress: {IoC.Communication.IpAddress}");

            //    // Verify a new test available.
            //    if (NewTestAvailable)
            //    {

            //        // Progress bar is visible
            //        IsConnecting = true;
            //        IsConnectionCompleted = false;

            //        var started = DateTime.Now;

            //        new DispatcherTimer(
            //            TimeSpan.FromMilliseconds(50),
            //            DispatcherPriority.Normal,
            //            new EventHandler((o, e) =>
            //            {
            //                var totalDuration = started.AddSeconds(7).Ticks - started.Ticks;
            //                var currentProgress = DateTime.Now.Ticks - started.Ticks;
            //                var currentProgressPercent = 100.0 / totalDuration * currentProgress;

            //                TestProgress = currentProgressPercent;

            //                if (TestProgress >= 100)
            //                {
            //                    IsConnecting = true;
            //                    TestProgress = 0;
            //                    ((DispatcherTimer)o).Stop();
            //                }

            //            }), Dispatcher.CurrentDispatcher);


            //        // get instance of Omicron Test Set
            //        IoC.Communication.CMCControl = new CMCControl();

            //        // await omicron connection
            //        bool isOmicronConnected = await Task.Run(() => IoC.Communication.CMCControl.FindCMC());

            //        // The user click on the button 
            //        // TODO: Handle Omicron open connection here.
            //        Debug.WriteLine($"TODO: {DateTime.Now.ToLocalTime()}: Connect Omicron Test Set ... success?: {isOmicronConnected}");
            //        IoC.Communication.Log += $"{DateTime.Now.ToLocalTime()}: Connecting Omicron Test Set was {(isOmicronConnected ? " successful" : " failed")}\n";

            //        // TODO: Handle ConnectCommand Button checked
            //        Debug.WriteLine($"TODO: {DateTime.Now.ToLocalTime()}: Connect thru modbus protocol to {IoC.Communication.IpAddress}:{IoC.Communication.Port}");
            //        await Task.Run(() => IoC.TestDetails.ConnectCommand.Execute(IoC.TestDetails));

            //        IsConnectionCompleted = true;

            //        //// Pressing again same button will terminate the test
            //        //NewTestAvailable = false;

            //    }
            //    else
            //    {
            //        // The user wants to disconnect.
            //        // TODO: Handle Omicron close connection here.
            //        Debug.WriteLine($"TODO: {DateTime.Now.ToLocalTime()}: Disconnect Omicron Test Set ...");

            //        // TODO: Handle ConnectCommand Button checked
            //        Debug.WriteLine($"TODO: {DateTime.Now.ToLocalTime()}: Disconnect modbus communication to {IoC.Communication.IpAddress}:{IoC.Communication.Port}");

            //        // TODO: Verify disconnect was successful.


            //        // Progress is visible
            //        IsConnecting = false;
            //        IsConnectionCompleted = false;
            //        NewTestAvailable = true;
            //    }
            //});
        }
        #endregion
    }
}
