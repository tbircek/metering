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
                    MagnitudeHint = Resources.Strings.header_magnitude_voltage,
                    Magnitude = "105.0",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = false,
                    IsHarmonicsEnabled = true,
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v2",
                    MagnitudeHint = Resources.Strings.header_magnitude_voltage,
                    Magnitude = "105.0",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = true,
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v3",
                    MagnitudeHint = Resources.Strings.header_magnitude_voltage,
                    Magnitude = "105.0",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = true,
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "v4",
                    MagnitudeHint = Resources.Strings.header_magnitude_voltage,
                    Magnitude = "105.0",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = true,
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i1",
                    MagnitudeHint = Resources.Strings.header_magnitude_current,
                    Magnitude = "0.500",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = true,
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i2",
                    MagnitudeHint = Resources.Strings.header_magnitude_current,
                    Magnitude = "0.500",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = true,
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i3",
                    MagnitudeHint = Resources.Strings.header_magnitude_current,
                    Magnitude = "0.500",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = true,
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i4",
                    MagnitudeHint = Resources.Strings.header_magnitude_current,
                    Magnitude = "0.500",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = true,
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i5",
                    MagnitudeHint = Resources.Strings.header_magnitude_current,
                    Magnitude = "0.500",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = true,
                },
                new AnalogSignalListItemViewModel
                {
                    SignalName = "i6",
                    MagnitudeHint = Resources.Strings.header_magnitude_current,
                    Magnitude = "0.500",
                    PhaseHint = Resources.Strings.header_phase,
                    Phase = "-120.000",
                    Frequency = "59.999",
                    FrequencyHint = Resources.Strings.header_frequency,
                    Delta = "104.333",
                    DeltaHint = Resources.Strings.header_delta,
                    From = "100.400",
                    FromHint = Resources.Strings.header_from,
                    To = "134.600",
                    ToHint = Resources.Strings.header_to,
                    IsFrequencyEnabled = false,
                    IsMagnitudeEnabled = false,
                    IsPhaseEnabled = true,
                },
            };
        }

        #endregion
    }
}
