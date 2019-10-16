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
            SignalName = "v1";
            MagnitudeHint = Resources.Strings.header_magnitude_voltage;
            Magnitude = "105.0";
            PhaseHint = Resources.Strings.header_phase;
            Phase = "-120.000";
            Frequency = "59.999";
            FrequencyHint = Resources.Strings.header_frequency;
            Delta = "104.333";
            DeltaHint = Resources.Strings.header_delta;
            From = "100.400";
            FromHint = Resources.Strings.header_from;
            To = "134.600";
            ToHint = Resources.Strings.header_to;
            IsFrequencyEnabled = false;
            IsMagnitudeEnabled = false;
            IsPhaseEnabled = true;
        }

        #endregion
    }
}
