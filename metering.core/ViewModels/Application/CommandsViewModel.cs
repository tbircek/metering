﻿using System;
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
        /// The command handles cancelling New Test addition view and returns default view
        /// </summary>
        public ICommand CancelNewTestCommand { get; set; }

        /// <summary>
        /// The command to handle connecting associated Omicron Test Set
        /// and communication to the UUT
        /// </summary>
        public ICommand StartTestCommand { get; set; }

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

            // reset maximum value for progress bar for the next run
            MaximumTestCount = 0d;

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
            // Run test command
            await Task.Run(() => IoC.TestDetails.ConnectCommand.Execute(IoC.TestDetails));
        }
        #endregion
    }
}
