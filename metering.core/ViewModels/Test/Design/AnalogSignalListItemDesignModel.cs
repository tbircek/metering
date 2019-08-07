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
            From = "100.4";
            To = "134.6";
            Delta = "4.333";
            Phase = "0.000";
            Frequency = "59.999";
        }

        #endregion
    }
}
