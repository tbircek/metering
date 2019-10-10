using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// Shows test details such as <see cref="Register"/>, <see cref="DwellTime"/> and such
    /// with the user specified nominal values in <see cref="NominalValuesViewModel"/> 
    /// </summary>
    public class TestDetailsViewModel : BaseViewModel
    {
        #region Public Enums

        /// <summary>
        /// Holds all possible Ramping signal names
        /// </summary>
        public enum RampingSignals
        {
            /// <summary>
            /// Ramping signal is "Magnitude"
            /// </summary>
            Magnitude = 0,

            /// <summary>
            /// Ramping signal is "Phase"
            /// </summary>
            Phase = 1,

            /// <summary>
            /// Ramping Signal is "Frequency"
            /// </summary>
            Frequency = 2,
        }

        #endregion
        
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
        public string RegisterHint { get; set; } = Strings.header_register;

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
        public string DwellTimeHint { get; set; } = Strings.header_dwell_time;

        /// <summary>
        /// How long should <see cref="Register"/> be poll.
        /// </summary>
        public string DwellTime { get; set; } = "20";

        /// <summary>
        /// Provides a hint text for the <see cref="StartDelayTime"/> textbox
        /// </summary>
        public string StartDelayTimeHint { get; set; } = Strings.header_start_delay_time;

        /// <summary>
        /// The time to wait until test step #1.
        /// </summary>
        public string StartDelayTime { get; set; } = "0.1";

        /// <summary>
        /// Provides a hint text for the <see cref="MeasurementInterval"/> textbox
        /// </summary>
        public string MeasurementIntervalHint { get; set; } = Strings.header_measurement_interval;

        /// <summary>
        /// How often should <see cref="Register"/> be poll.
        /// </summary>
        public string MeasurementInterval { get; set; } = "250";

        /// <summary>
        /// Provides a hint text for the <see cref="StartMeasurementDelay"/> textbox
        /// </summary>
        public string StartMeasurementDelayHint { get; set; } = Strings.header_start_measurement_delay;

        /// <summary>
        /// The time to wait after analog signals applied before <see cref="DwellTime"/> starts.
        /// </summary>
        public string StartMeasurementDelay { get; set; } = "5";

        /// <summary>
        /// indicates if the current text double left clicked to highlight the text
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Holds information about the signal parameter to ramp
        /// </summary>
        public string SelectedRampingSignal { get; set; } = "Magnitude";
        
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

        /// <summary>
        /// The command handles radio button selections
        /// </summary>
        public ICommand SelectRampingSignalCommand { get; set; }

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
            SelectRampingSignalCommand = new RelayParameterizedCommand((parameter) => RampingSelectionAsync((string)parameter));

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

        #region Private Helpers

        /// <summary>
        /// Modifies textbox hint text per selected ramping signal option.
        /// </summary>
        /// <returns></returns>
        private void RampingSelectionAsync(object parameter)
        {
            // convert command parameter to string
            SelectedRampingSignal = (string)parameter;

            foreach (var item in AnalogSignals)
            {
                // these fields work inversely.
                // enable/disable Magnitude text field...
                item.IsMagnitudeEnabled = !(string.Equals(SelectedRampingSignal, nameof(RampingSignals.Magnitude)));
                // enable/disable Phase text field...
                item.IsPhaseEnabled = !(string.Equals(SelectedRampingSignal, nameof(RampingSignals.Phase)));
                // enable/disable Frequency text field...
                item.IsFrequencyEnabled = !(string.Equals(SelectedRampingSignal, nameof(RampingSignals.Frequency)));
                
                // process radio button parameter
                switch (SelectedRampingSignal)
                {
                    // Phase option selected:
                    case nameof(RampingSignals.Phase): 
                        // Update From value with Phase value
                        item.From = item.Phase;
                        // Update To value with Phase value
                        item.To = item.Phase;
                        // From text field hint
                        item.FromHint = Strings.header_from_phase;
                        // To text field hint
                        item.ToHint = Strings.header_to_phase;
                        // Delta text field hint
                        item.DeltaHint = Strings.header_delta_phase;
                        break;

                    // Frequency option selected:
                    case nameof(RampingSignals.Frequency):
                        // Update From value with Frequency value
                        item.From = item.Frequency;
                        // Update To value with Frequency value
                        item.To = item.Frequency;
                        // From text field hint
                        item.FromHint = Strings.header_from_frequency;
                        // To text field hint
                        item.ToHint = Strings.header_to_frequency;
                        // Delta text field hint
                        item.DeltaHint = Strings.header_delta_frequency;
                        break;

                    // Magnitude or first load up selected:
                    default:
                        // Magnitude values return appropriate string values when they empty
                        // Update From value with Magnitude value
                        item.From = item.Magnitude;
                        // Update To value with Magnitude value
                        item.To = item.Magnitude;
                        // From text field hint
                        item.FromHint = string.Empty;
                        // To text field hint
                        item.ToHint = string.Empty;
                        // Delta text field hint
                        item.DeltaHint = string.Empty;
                        break;
                }
            }
        }
        #endregion
    }
}
