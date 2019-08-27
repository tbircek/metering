namespace metering.core
{
    /// <summary>
    /// a viewmodel for each analog signal in the TestDetailsPage
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
        public string From { get; set; }

        /// <summary>
        /// Hint label for From entry.
        /// </summary>
        public string FromHint { get; set; } = Resources.Strings.header_from;

        /// <summary>
        /// Omicron Analog Output end magnitude 
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Hint label for To entry.
        /// </summary>
        public string ToHint { get; set; } = Resources.Strings.header_to;

        /// <summary>
        /// Omicron Analog Output magnitude increment/decrement
        /// </summary>
        public string Delta { get; set; }

        /// <summary>
        /// Hint label for Delta entry.
        /// </summary>
        public string DeltaHint { get; set; } = Resources.Strings.header_delta;

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

        #endregion
    }
}
