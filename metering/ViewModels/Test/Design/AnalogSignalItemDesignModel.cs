namespace metering
{
    /// <summary>
    /// Design time data for a <see cref="AnalogSignalListItemViewModel"/>
    /// </summary>
    public class AnalogSignalItemDesignModel : AnalogSignalListItemViewModel
    {
        #region Singleton       

        /// <summary>
        /// Single instance of the design time model
        /// </summary>
        public static AnalogSignalItemDesignModel Instance => new AnalogSignalItemDesignModel();

        #endregion

        #region Constructor

        public AnalogSignalItemDesignModel()
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
