using System.Collections.ObjectModel;

namespace metering.core
{
    /// <summary>
    /// Design time data for a <see cref="AnalogSignalListViewModel"/>
    /// </summary>
    public class AnalogSignalDesignModel : AnalogSignalViewModel
    {
        #region Singleton       

        /// <summary>
        /// Single instance of the design time model
        /// </summary>
        public static AnalogSignalDesignModel Instance => new AnalogSignalDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public AnalogSignalDesignModel()
        {
            AnalogSignals = new ObservableCollection<AnalogSignalListItemViewModel>
            {
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v1",
                    Magnitude = "105.0",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v2",
                    Magnitude = "105.0",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v3",
                    Magnitude = "105.0",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v4",
                    Magnitude = "105.0",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i1",
                    Magnitude = "0.500",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i2",
                    Magnitude = "0.500",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i3",
                    Magnitude = "0.500",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i4",
                    Magnitude = "0.500",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i5",
                    Magnitude = "0.500",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i6",
                    Magnitude = "0.500",
                    Phase = "-120.000",
                    Frequency = "59.999",
                    Delta = "104.333",
                    From = "100.400",
                    To = "134.600"
                },
            };
        }

        #endregion
    }
}
