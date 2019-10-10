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
            Magnitude = "105.0";
            Phase = "-120.000";
            Frequency = "59.999";
            Delta = "104.333";
            From = "100.400";
            To = "134.600";
        }

        #endregion
    }
}
