using System.Collections.ObjectModel;
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
        /// Provides a hint text for the <see cref="Register"/> textbox
        /// </summary>
        public string RegisterHint { get; set; } = Resources.Strings.header_register;

        /// <summary>
        /// The register to monitor while testing.
        /// </summary>
        public string Register { get; set; } = "2279";

        /// <summary>
        /// Show test completion percentage.
        /// </summary>
        public string Progress { get; set; } = "0.0";

        /// <summary>
        /// Provides a hint text for the <see cref="DwellTime"/> textbox
        /// </summary>
        public string DwellTimeHint { get; set; } = Resources.Strings.header_dwell_time;

        /// <summary>
        /// How long should <see cref="Register"/> be poll.
        /// </summary>
        public string DwellTime { get; set; } = "20";

        /// <summary>
        /// Provides a hint text for the <see cref="StartDelayTime"/> textbox
        /// </summary>
        public string StartDelayTimeHint { get; set; } = Resources.Strings.header_start_delay_time;

        /// <summary>
        /// The time to wait until test step #1.
        /// </summary>
        public string StartDelayTime { get; set; } = "0.1";

        /// <summary>
        /// Provides a hint text for the <see cref="MeasurementInterval"/> textbox
        /// </summary>
        public string MeasurementIntervalHint { get; set; } = Resources.Strings.header_measurement_interval;

        /// <summary>
        /// How often should <see cref="Register"/> be poll.
        /// </summary>
        public string MeasurementInterval { get; set; } = "250";

        /// <summary>
        /// Provides a hint text for the <see cref="StartMeasurementDelay"/> textbox
        /// </summary>
        public string StartMeasurementDelayHint { get; set; } = Resources.Strings.header_start_measurement_delay;

        /// <summary>
        /// The time to wait after analog signals applied before <see cref="DwellTime"/> starts.
        /// </summary>
        public string StartMeasurementDelay { get; set; } = "5";

        /// <summary>
        /// indicates if the current text double left clicked to highlight the text
        /// </summary>
        public bool Selected { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Selects all text on left clicked text box.
        /// </summary>
        public ICommand SelectAllTextCommand { get; set; }

        /// <summary>
        /// command to provide connection to both Attached Omicron and 
        /// specified Test Unit IpAddress and port.
        /// </summary>
        public ICommand ConnectCommand { get; set; }

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

            // create the commands.
            ConnectCommand = new RelayCommand(async () => await IoC.Communication.StartCommunicationAsync());
            SelectAllTextCommand = new RelayCommand(SelectAll);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Selects all text on the text box
        /// </summary>
        public void SelectAll()
        {
            // simulate property change briefly to select all text in the text box
            // as selecting all text should be last until the user leaves the control or types something

            // Sets FocusAndSelectProperty to true
            Selected = true;

            // Sets FocusAndSelectProperty to false
            Selected = false;
        }

        #endregion

        #region Helpers

        #endregion
    }
}
