using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace metering.core
{
    /// <summary>
    /// a view model for each analog signal in the TestDetailsPage
    /// </summary>
    public class AnalogSignalListItemViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Omicron Analog Output name.
        /// </summary>
        public string SignalName { get; set; }

        /// <summary>
        /// Hint label for SignalName entry.
        /// </summary>
        public string SignalNameHint { get; set; } = Resources.Strings.header_signal;

        /// <summary>
        /// Omicron Analog Output start magnitude 
        /// </summary>
        public string Magnitude { get; set; }

        /// <summary>
        /// Hint label for From entry.
        /// </summary>
        public string MagnitudeHint
        {
            get
            {
                // Returns Voltage hint text for "v" signals, or Current hint text for "i" signals
                return SignalName.StartsWith("v") ? Resources.Strings.header_magnitude_voltage : Resources.Strings.header_magnitude_current;
            }
            set { }
        }

        /// <summary>
        /// Omicron Analog Output start magnitude 
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Hint label for From entry.
        /// </summary>
        public string FromHint 
        {
            get
            {
                // Returns Voltage hint text for "v" signals, or Current hint text for "i" signals
                return SignalName.StartsWith("v") ? Resources.Strings.header_from_voltage : Resources.Strings.header_from_current;
            }
            set { }
        }

        /// <summary>
        /// Omicron Analog Output end magnitude 
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Hint label for To entry.
        /// </summary>
        public string ToHint
        {
            get
            {
                // Returns Voltage hint text for "v" signals, or Current hint text for "i" signals
                return SignalName.StartsWith("v") ? Resources.Strings.header_to_voltage : Resources.Strings.header_to_current;
            }
            set { }
        }

        /// <summary>
        /// Omicron Analog Output magnitude increment/decrement
        /// </summary>
        public string Delta { get; set; }

        /// <summary>
        /// Hint label for Delta entry.
        /// </summary>
        public string DeltaHint
        {
            get
            {
                // Returns Voltage hint text for "v" signals, or Current hint text for "i" signals
                return SignalName.StartsWith("v") ? Resources.Strings.header_delta_voltage : Resources.Strings.header_delta_current;
            }
            set { }
        }
        
        /// <summary>
        /// Omicron Analog Output phase
        /// </summary>
        public string Phase { get; set; }

        /// <summary>
        /// Hint label for Phase entry.
        /// </summary>
        public string PhaseHint { get; set; } = Resources.Strings.header_phase;

        /// <summary>
        /// Omicron Analog Output frequency
        /// </summary>
        public string Frequency { get; set; }

        /// <summary>
        /// Hint label for Frequency entry.
        /// </summary>
        public string FrequencyHint { get; set; } = Resources.Strings.header_frequency;

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

        #endregion

        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public AnalogSignalListItemViewModel()
        {

            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            // create command
            SelectAllTextCommand = new RelayCommand(SelectAll);
        }

        #endregion

        #region Public Method

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
    }
}
