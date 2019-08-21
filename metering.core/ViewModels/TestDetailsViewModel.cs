using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace metering.core
{
    /// <summary>
    /// Shows test details such as <see cref="Register"/>, <see cref="DwellTime"/> and such
    /// with the user specified nominal values in <see cref="NominalValuesViewModel"/> 
    /// </summary>
    public class TestDetailsViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Omicron Analog Output Signals.
        /// Depending on <see cref="AnalogSignalListItemViewModel.SignalName"/> 
        /// <see cref="NominalValuesViewModel.NominalVoltage"/> and <see cref="NominalValuesViewModel.NominalCurrent"/>
        /// would apply to <see cref="AnalogSignalListItemViewModel.From"/> and <see cref="AnalogSignalListItemViewModel.To"/> values.
        /// So initial view the both values would be same
        /// </summary>
        public ObservableCollection<AnalogSignalListItemViewModel> AnalogSignals { get; set; }

        /// <summary>
        /// The register to monitor while testing.
        /// </summary>
        public string Register { get; set; } = "0";

        /// <summary>
        /// Show test completion percentage.
        /// </summary>
        public string Progress { get; set; } = "0.0";

        /// <summary>
        /// How long should <see cref="Register"/> be poll.
        /// </summary>
        public string DwellTime { get; set; } = "120";

        /// <summary>
        /// The time to wait until test step #1.
        /// </summary>
        public string StartDelayTime { get; set; } = "30";

        /// <summary>
        /// How often should <see cref="Register"/> be poll.
        /// </summary>
        public string MeasurementInterval { get; set; } = "100";

        /// <summary>
        /// The time to wait after analog signals applied before <see cref="DwellTime"/> starts.
        /// </summary>
        public string StartMeasurementDelay { get; set; } = "10";

        /// <summary>
        /// The text to use Test button
        /// </summary>
        public string TestText { get; set; }

        /// <summary>
        /// Foreground color for the controls
        /// </summary>
        public string CommandForegroundColor { get; set; }
        #endregion

        #region Public Commands

        ///// <summary>
        ///// Selects all text in the textbox
        ///// </summary>
        //public ICommand SelectAllTextCommand { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestDetailsViewModel()
        {
            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            OnPropertyChanged("Register");
            //// create command
            //SelectAllTextCommand = new RelayParameterizedCommand((parameter) => SelectAllText (parameter));
        }

        #endregion

        #region Public Methods

        #endregion

        #region Helpers

        /// <summary>
        /// Select all the text in the current textbox
        /// </summary>
        private void SelectAllText(object parameter)
        {
            if(!string.IsNullOrWhiteSpace(Register))
                Register = string.Empty;
            Debug.WriteLine("Clicked");
        }
        #endregion
    }
}
