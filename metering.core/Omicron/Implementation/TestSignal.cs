using System;
using System.Globalization;
using System.Linq;

namespace metering.core
{
    /// <summary>
    /// Holds properties of the Test signal that the user wants to test.
    /// Decides the Test Signal by comparing From and To strings 
    /// </summary>
    public class TestSignal
    {
        /// <summary>
        /// Holds whether a ramping is possible with given <see cref="IoC.TestDetails.AnalogSignals"/>
        /// </summary>
        public bool IsRamping
        {
            get
            {
                // assumption == no ramping
                bool ramping = false;

                // if harmonic is selected magnitude must be a non-zero value
                // since harmonics == magnitude * from(%)
                if (IoC.TestDetails.IsHarmonics)
                {
                    // scan analog signals for magnitude > 0 && from != to && delta > 0
                    var rampingSignal = from signal in IoC.TestDetails.AnalogSignals
                                        where !Convert.ToDouble(signal.Magnitude).Equals(0.000000d)
                                        where !Convert.ToDouble(signal.From).Equals(Convert.ToDouble(signal.To))
                                        select !Convert.ToDouble(signal.Delta).Equals(0.000000d);

                    // more than 0 indicate there is a ramping module
                    ramping = rampingSignal.Count() > 0;
                }
                else
                {
                    // scan analog signals for from != to && delta > 0
                    var rampingSignal = from signal in IoC.TestDetails.AnalogSignals
                                        where !Convert.ToDouble(signal.From).Equals(Convert.ToDouble(signal.To))
                                        select !Convert.ToDouble(signal.Delta).Equals(0.000000d);

                    // more than 0 indicate there is a ramping module
                    ramping = rampingSignal.Count() > 0;
                }

                // return if ramping is possible
                return ramping;
            }
        }

        /// <summary>
        /// Holds if the test is permitted to run
        /// </summary>
        public bool IsRunningPermitted
        {
            get
            {
                // if ramping is possible and current or voltage configuration is selected
                // Returns true, otherwise false.
                return IsRamping &&
                       (IoC.TestDetails.SelectedCurrentConfiguration.CurrentWiringDiagram || IoC.TestDetails.SelectedVoltageConfiguration.CurrentWiringDiagram);             
            }
        }

        /// <summary>
        /// Holds properties of the Ramping Signal that decided by the first 
        /// different From and To values in <see cref="AnalogSignalViewModel.AnalogSignals"/>
        /// </summary>
        /// <returns>Returns a Tuple with ramping signal properties</returns>       
        public (string SignalName, double From, double To, double Delta, double Phase, double Frequency, int Precision) GetRampingSignal()
        {
            // initialize Tuple variables with default values
            string SignalName = string.Empty;
            double From = default;
            double To = default;
            double Delta = default;
            double Phase = default;
            double Frequency = default;
            int Precision = default;

            foreach (AnalogSignalListItemViewModel signal in IoC.TestDetails.AnalogSignals)
            {
                // scan TestDetailsViewModel and return all signal properties where From and To values are not same
                if (!Convert.ToDouble(signal.From).Equals(Convert.ToDouble(signal.To)))
                {
                    // property values of the signal
                    Delta = Convert.ToDouble(signal.Delta);
                    // if Delta is zero move next item
                    if (Equals(Delta, 0.000000d))
                    {
                        continue;
                    }
                    SignalName = signal.SignalName;
                    From = Convert.ToDouble(signal.From);
                    To = Convert.ToDouble(signal.To);                    
                    Phase = Convert.ToDouble(signal.Phase);
                    Frequency = Convert.ToDouble(signal.Frequency);

                    Precision = GetPrecision(signal.From);

                    // return properties of the ramping signal found
                    return (SignalName, From, To, Delta, Phase, Frequency, Precision);
                }
            }

            // no ramping signal found
            return (string.Empty, default, default, default, default, default, default);
        }

        /// <summary>
        /// Retrieves the length of a string after the current culture based decimal separator.
        /// </summary>
        /// <param name="valueToConvert">the string to calculate remaining length after the current culture based decimal separator.</param>
        /// <returns>Returns length after the current culture based decimal separator.</returns>
        private int GetPrecision(string valueToConvert)
        {
            // verify the string contains the current culture based decimal separator.
            if (!valueToConvert.Contains(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator))
            { 
                // doesn't contain the current culture based decimal separator.
                return 0;
            }
            else
            {
                // return string length after the current culture based decimal separator.
                return valueToConvert.Substring(valueToConvert.IndexOf(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator) + 1).Length;
            }
        }
    }
}