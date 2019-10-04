
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
        public void SendOmicronCommands(string testSignalName, double testStartValue)
        {
            // TODO: Move this methods to its own class and automate

            // inform developer
            IoC.Logger.Log($"SendOmicronCommands started :  ramping signal: {testSignalName} -- test value: {testStartValue}", LogLevel.Informative);

            // set voltage amplifiers default values.
            // Analog signal: Voltage Output 1:
            IoC.StringCommands.SendOutAnaAsync
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.v,
                // generator 
                generatorNumber: "1:1",
                // Signal Amplitude
                amplitude: (string.Equals("v1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Magnitude),
                // Signal Phase
                phase: (string.Equals("v1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Phase),
                // Signal Frequency
                frequency: (string.Equals("v1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[0].Frequency)
                );

            // Analog signal: Voltage Output 2:
            IoC.StringCommands.SendOutAnaAsync
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.v,
                // generator 
                generatorNumber: "1:2",
                 // Signal Amplitude
                 amplitude: (string.Equals("v2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[1].Magnitude),
                // Signal Phase
                phase: (string.Equals("v2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[1].Phase),
                // Signal Frequency
                frequency: (string.Equals("v2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[1].Frequency)
                );

            // Analog signal: Voltage Output 3:
            IoC.StringCommands.SendOutAnaAsync
                (

                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.v,
                // generator 
                generatorNumber: "1:3",
                // Signal Amplitude
                amplitude: (string.Equals("v3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[2].Magnitude),
                // Signal Phase
                phase: (string.Equals("v3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[2].Phase),
                // Signal Frequency
                frequency: (string.Equals("v3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[2].Frequency)
                );

            // set current amplifiers default values.
            // Analog signal: Current Output 1:
            IoC.StringCommands.SendOutAnaAsync
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.i,
                // generator 
                generatorNumber: "1:1",
                // Signal Amplitude
                amplitude: (string.Equals("i1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[3].Magnitude),
                // Signal Phase
                phase: (string.Equals("i1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[3].Phase),
                // Signal Frequency
                frequency: (string.Equals("i1", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[3].Frequency)
                );

            // Analog signal: Current Output 2:
            IoC.StringCommands.SendOutAnaAsync
                (
               // Omicron Test Set internal generator name
               generator: (int)StringCommands.GeneratorList.i,
               // generator 
               generatorNumber: "1:2",
               // Signal Amplitude
               amplitude: (string.Equals("i2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[4].Magnitude),
                // Signal Phase
                phase: (string.Equals("i2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[4].Phase),
                // Signal Frequency
                frequency: (string.Equals("i2", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[4].Frequency)
                );

            // Analog signal: Current Output 3:
            IoC.StringCommands.SendOutAnaAsync
                (
                // Omicron Test Set internal generator name
                generator: (int)StringCommands.GeneratorList.i,
                // generator 
                generatorNumber: "1:3",
                // Signal Amplitude
                amplitude: (string.Equals("i3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[5].Magnitude),
                // Signal Phase
                phase: (string.Equals("i3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[5].Phase),
                // Signal Frequency
                frequency: (string.Equals("i3", testSignalName) && (string.Equals(IoC.TestDetails.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency)))) ? testStartValue : Convert.ToDouble(IoC.TestDetails.AnalogSignals[5].Frequency)
                );

        }

    }
}
