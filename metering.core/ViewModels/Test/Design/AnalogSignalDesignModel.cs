﻿using System.Collections.ObjectModel;

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
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v2",
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v3",
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v4",
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i1",
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i2",
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i3",
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i4",
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i5",
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i6",
                    From = "100.400",
                    To = "134.600",
                    Delta = "104.333",
                    Phase = "-120.000",
                    Frequency = "59.999"
                },
            };
        }

        #endregion
    }
}
