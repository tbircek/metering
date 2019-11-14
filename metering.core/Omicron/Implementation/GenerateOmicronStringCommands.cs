
using System;

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
            IoC.Logger.Log($"{nameof(this.SendOmicronCommands)} started :  ramping signal: {testSignalName} -- test value: {testStartValue}", LogLevel.Informative);

            foreach (AnalogSignalListItemViewModel analogSignal in IoC.TestDetails.AnalogSignals)
            {
                // set voltage amplifiers values.

                IoC.StringCommands.SendOutAnaAsync(
                    // Omicron Test Set internal generator type
                    generatorType: analogSignal.SignalName.ToCharArray()[0],
                    // tripletNumber --- as long as we use (v|i)1 to 3 this value is 1
                    tripletNumber: $"1:{analogSignal.SignalName.ToCharArray()[1]}",
                    // Signal Amplitude
                    amplitude: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)) ? (string.Equals(analogSignal.SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignal.From) : Convert.ToDouble(analogSignal.Magnitude),
                    // Signal Phase
                    phase: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)) ? (string.Equals(analogSignal.SignalName, testSignalName)) ? testStartValue : Convert.ToDouble(analogSignal.From) : Convert.ToDouble(analogSignal.Phase),
                    // Signal Frequency
                    frequency: string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)) ? (string.Equals(analogSignal.SignalName, testSignalName) ? testStartValue : Convert.ToDouble(analogSignal.From)) : Convert.ToDouble(analogSignal.Frequency)
                    );
            }
        }
    }
}
