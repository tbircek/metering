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
            From = "100.400";
            To = "134.600";
            Delta = "104.333";
            Phase = "-120.000";
            Frequency = "59.999";
        }

        #endregion
    }
}
