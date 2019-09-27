namespace metering.core
{
    /// <summary>
    /// Design time data for a <see cref="AnalogSignalListViewModel"/>
    /// </summary>
    public class AnalogSignalListItemDesignModel : AnalogSignalListItemViewModel
    {
        #region Singleton       

        /// <summary>
        /// Single instance of the design time model
        /// </summary>
        public static AnalogSignalListItemDesignModel Instance => new AnalogSignalListItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public AnalogSignalListItemDesignModel()
        {
            SignalNameHint = Resources.Strings.header_signal;
            SignalName = "v1";
            MagnitudeHint = Resources.Strings.header_magnitude_voltage;
            Magnitude = "105.0";
            PhaseHint = Resources.Strings.header_phase;
            Phase = "-120.000";
            FrequencyHint = Resources.Strings.header_frequency;
            Frequency = "59.999";
            DeltaHint = Resources.Strings.header_delta;
            Delta = "104.333";
            FromHint = Resources.Strings.header_from;
            From = "100.400";
            ToHint = Resources.Strings.header_to;
            To = "134.600";
        }

        #endregion
    }
}
