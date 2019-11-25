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
        /// Holds properties of the Ramping Signal that decided by the first 
        /// different From and To values in <see cref="AnalogSignalViewModel.AnalogSignals"/>
        /// </summary>
        /// <returns>Returns a Tuple with ramping signal properties</returns>       
        public (string SignalName, double From, double To, double Delta, double Phase, double Frequency, int Precision) GetRampingSignal()
        {
            // initialize Tuple variables with default values
            string SignalName = string.Empty;
            double From = default(double);
            double To = default(double);
            double Delta = default(double);
            double Phase = default(double);
            double Frequency = default(double);
            int Precision = default;

            foreach (AnalogSignalListItemViewModel signal in IoC.TestDetails.AnalogSignals)
            {
                // scan TestDetailsViewModel and return all signal properties where From and To values are not same
                if (!Convert.ToDouble(signal.From).Equals(Convert.ToDouble(signal.To)))
                {
                    // property values of the signal
                    SignalName = signal.SignalName;
                    From = Convert.ToDouble(signal.From);
                    To = Convert.ToDouble(signal.To);
                    Delta = Convert.ToDouble(signal.Delta);
                    Phase = Convert.ToDouble(signal.Phase);
                    Frequency = Convert.ToDouble(signal.Frequency);
                    Precision = signal.From.Substring(signal.From.IndexOf(CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator), signal.From.Length - 1).Length;

                    // return properties of the ramping signal found
                    return (SignalName, From, To, Delta, Phase, Frequency, Precision);
                }
            }

            // no ramping signal found
            return (string.Empty, default(double), default(double), default(double), default(double), default(double), default);
        }
    }
}