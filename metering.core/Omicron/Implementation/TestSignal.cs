using System;
using System.Globalization;

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
                bool ramping = default;

                foreach (AnalogSignalListItemViewModel signal in IoC.TestDetails.AnalogSignals)
                {
                    // scan TestDetailsViewModel and return all signal properties where From and To values are not same
                    if (!Convert.ToDouble(signal.From).Equals(Convert.ToDouble(signal.To)))
                    {
                        // property values of the signal
                        // Delta = Convert.ToDouble(signal.Delta);
                        // if Delta is zero move next item
                        if (Equals(Convert.ToDouble(signal.Delta), 0.000000d))
                        {
                            ramping = false;
                            continue;
                        }
                        else
                        {
                            ramping = true;
                        }
                    }
                }
                return ramping;
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