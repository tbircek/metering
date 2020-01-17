
using System;
using System.Linq;
using System.Threading;

namespace metering.core
{
    /// <summary>
    /// Send string commands to Omicron Test Set
    /// </summary>
    public class GenerateOmicronStringCommands
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public GenerateOmicronStringCommands()
        {

        }

        #endregion

        /// <summary>
        /// Send string commands to Omicron Test Set
        /// </summary>
        /// <param name="testSignalName">the signal that From and To values are not equal</param>
        /// <param name="testStartValue">From value that test starts and increments per <see cref="AnalogSignalListItemViewModel.Delta"/></param>
        /// <example>Output string ex: out:ana:v(1:1):a(120);p(0);f(60);wav(sin)</example>
        public void SendOmicronCommands(string testSignalName, double testStartValue)
        {
            // inform developer
            IoC.Logger.Log($"{nameof(this.SendOmicronCommands)} started: ramping signal: {testSignalName} -- test value: {testStartValue:F6}", LogLevel.Informative);

            // Route Omicron amplifiers.
            // retrieve voltage amplifiers
            var analogSignals = (from signal in IoC.TestDetails.AnalogSignals where signal.SignalName.StartsWith("v") select signal).ToArray();
            // keeper of analog signal positions
            int analogSignalPosition = default;

            // process voltage amplifiers
            for (int amplifier = 0; amplifier < IoC.TestDetails.SelectedVoltageConfiguration.ConfigIDs.Count; amplifier++)
            {
                // retrieve triplet group number this value could be either 1 or 2 for either voltage or current amplifiers.
                int tripletGroupNumber = Convert.ToInt32(IoC.TestDetails.SelectedVoltageConfiguration.AmplifierNumber[amplifier]) == 5 ? 2 : 1;

                // set values per triplets
                for (int triplet = 1; triplet <= IoC.TestDetails.SelectedVoltageConfiguration.PhaseCounts[amplifier]; triplet++)
                {
                    // set the voltage amplifiers values.
                    IoC.StringCommands.SendOutAnaAsync(
                        // Omicron Test Set internal generator type
                        generatorType: analogSignals[analogSignalPosition].SignalName.ToCharArray()[0],
                        // triplet number of the voltage amplifier
                        tripletNumber: $"{tripletGroupNumber}:{triplet}",
                        // Signal Amplitude
                        amplitude: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From) : Convert.ToDouble(analogSignals[analogSignalPosition].Magnitude),
                        // Signal Phase
                        phase: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From) : Convert.ToDouble(analogSignals[analogSignalPosition].Phase),
                        // Signal Frequency
                        // if IoC.TestDetails.IsLinked == true, use ramping signals frequency
                        frequency: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName) || IoC.TestDetails.IsLinked ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From)) : Convert.ToDouble(analogSignals[analogSignalPosition].Frequency),
                        // Harmonics only. Amplitude of the fundamental relative to the setting of the a() command
                        // Always 1 until further notice.
                        amplitude_factor: 1,
                        // the order of a harmonic
                        harmonicX: IoC.Communication.TestingHarmonicOrder,
                        // Signal Harmonic % of fundamental
                        amplitudeFactorX: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Harmonics)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From) : Convert.ToDouble(analogSignals[analogSignalPosition].Magnitude),
                        // the phase of the harmonic
                        phaseX: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From) : Convert.ToDouble(analogSignals[analogSignalPosition].Phase)
                        );

                    // increment analog signal position
                    Interlocked.Increment(ref analogSignalPosition);
                }
            }

            // retrieve current amplifiers
            analogSignals = (from signal in IoC.TestDetails.AnalogSignals where signal.SignalName.StartsWith("i") select signal).ToArray();

            // reset analog signal position for new analog signal group
            Interlocked.Exchange(ref analogSignalPosition, 0); // analogSignalPosition = 0;

            // process current amplifiers
            for (int amplifier = 0; amplifier < IoC.TestDetails.SelectedCurrentConfiguration.ConfigIDs.Count; amplifier++)
            {
                // retrieve triplet group number this value could be either 1 or 2 for either voltage or current amplifiers.
                int tripletGroupNumber = Convert.ToInt32(IoC.TestDetails.SelectedCurrentConfiguration.AmplifierNumber[amplifier]) == 6 ? 2 : 1;

                // set values per triplets
                for (int triplet = 1; triplet <= IoC.TestDetails.SelectedCurrentConfiguration.PhaseCounts[amplifier]; triplet++)
                {
                    // set the current amplifiers values.
                    IoC.StringCommands.SendOutAnaAsync(
                        // Omicron Test Set internal generator type
                        generatorType: analogSignals[analogSignalPosition].SignalName.ToCharArray()[0],
                        // triplet number of the current amplifier
                        tripletNumber: $"{tripletGroupNumber}:{triplet}",
                        // Signal Amplitude
                        amplitude: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From) : Convert.ToDouble(analogSignals[analogSignalPosition].Magnitude),
                        // Signal Phase
                        phase: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From) : Convert.ToDouble(analogSignals[analogSignalPosition].Phase),
                        // Signal Frequency
                        // if IoC.TestDetails.IsLinked == true, use ramping signals frequency
                        frequency: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName) || IoC.TestDetails.IsLinked ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From)) : Convert.ToDouble(analogSignals[analogSignalPosition].Frequency),
                        // Harmonics only. Amplitude of the fundamental relative to the setting of the a() command
                        // Always 1 until further notice.
                        amplitude_factor: 1,
                        // the order of a harmonic
                        harmonicX: IoC.Communication.TestingHarmonicOrder,
                        // Signal Harmonic % of fundamental
                        amplitudeFactorX: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Harmonics)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From) : Convert.ToDouble(analogSignals[analogSignalPosition].Magnitude),
                        // the phase of the harmonic
                        phaseX: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)) ? (string.Equals(analogSignals[analogSignalPosition].SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignals[analogSignalPosition].From) : Convert.ToDouble(analogSignals[analogSignalPosition].Phase)
                        );

                    // increment analog signal position
                    Interlocked.Increment(ref analogSignalPosition); // analogSignalPosition++;
                }
            }

        }
    }
}
