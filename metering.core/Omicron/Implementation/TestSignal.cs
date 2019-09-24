using System;

namespace metering.core
{
    /// <summary>
    /// Holds properties of the Test signal that the user wants to test.
    /// Decides the Test Signal by comparing From and To strings 
    /// </summary>
    public class TestSignal
    {
        #region Public Properties

        /// <summary>
        /// The name of the signal to do ramping while the other signals keep same value
        /// </summary>
        public string SignalName
        {
            get
            {
                // scan TestDetailsViewModel and return SignalName where From and To values are not same
                foreach (AnalogSignalListItemViewModel testValue in IoC.TestDetails.AnalogSignals)
                {
                    // assumption if these values are same the user doesn't want to test this signal
                    if (!Convert.ToDouble(testValue.From).Equals(Convert.ToDouble(testValue.To)))
                        // return SignalName as these two values are not same
                        return testValue.SignalName;
                }

                // there is no value that are not same
                return string.Empty;
            }
        }

        /// <summary>
        /// test start magnitude of <see cref="SignalName"/>
        /// </summary>
        public double From
        {
            get
            {
                // scan TestDetailsViewModel and return From value where From and To values are not same
                foreach (AnalogSignalListItemViewModel testValue in IoC.TestDetails.AnalogSignals)
                {
                    // assumption is if these values are same the user doesn't want to test this signal
                    if (!Convert.ToDouble(testValue.From).Equals(Convert.ToDouble(testValue.To)))
                        // return From as Double since the user wants to test this signal
                        // simply we are ignoring rest of the signal where From and To values are not same  
                        // assumption is: if there are From and To values are not equal simply a mistake
                        return Convert.ToDouble(testValue.From);
                }
                return default(double);
            }
        }

        /// <summary>
        /// test stop magnitude of <see cref="SignalName"/>
        /// </summary>
        public double To
        {
            get
            {
                // scan TestDetailsViewModel and return To value where From and To values are not same
                foreach (AnalogSignalListItemViewModel testValue in IoC.TestDetails.AnalogSignals)
                {
                    // assumption is if these values are same the user doesn't want to test this signal 
                    if (!Convert.ToDouble(testValue.From).Equals(Convert.ToDouble(testValue.To)))
                    // return To as Double since the user wants to test this signal
                    // simply we are ignoring rest of the signal where From and To values are not same
                    // assumption is: if there are From and To values are not equal simply a mistake
                    return Convert.ToDouble(testValue.To);
                }
                return default(double);
            }
        }

        /// <summary>
        /// test Delta value of <see cref="SignalName"/>
        /// </summary>
        public double Delta
        {
            get
            {
                // scan TestDetailsViewModel and return To value where From and To values are not same
                foreach (AnalogSignalListItemViewModel testValue in IoC.TestDetails.AnalogSignals)
                {
                    // assumption is if these values are same the user doesn't want to test this signal
                    if (!Convert.ToDouble(testValue.From).Equals(Convert.ToDouble(testValue.To)))
                        // return Delta as Double since the user wants to test this signal
                        // simply we are ignoring rest of the signal where From and To values are not same
                        // assumption is: if there are From and To values are not equal simply a mistake
                        return Convert.ToDouble(testValue.Delta);
                }
                return default(double);
            }
        }

        /// <summary>
        /// test Phase value of <see cref="SignalName"/>
        /// </summary>
        public double Phase
        {
            get
            {
                // scan TestDetailsViewModel and return To value where From and To values are not same
                foreach (AnalogSignalListItemViewModel testValue in IoC.TestDetails.AnalogSignals)
                {
                    // assumption is if these values are same the user doesn't want to test this signal
                    if (!Convert.ToDouble(testValue.From).Equals(Convert.ToDouble(testValue.To)))
                        // return Phase as Double since the user wants to test this signal
                        // simply we are ignoring rest of the signal where From and To values are not same
                        // assumption is: if there are From and To values are not equal simply a mistake
                        return Convert.ToDouble(testValue.Phase);
                }
                return default(double);
            }
        }


        /// <summary>
        /// test Frequency value of <see cref="SignalName"/>
        /// </summary>
        public double Frequency
        {
            get
            {
                // scan TestDetailsViewModel and return To value where From and To values are not same
                foreach (AnalogSignalListItemViewModel testValue in IoC.TestDetails.AnalogSignals)
                {
                    // assumption is if these values are same the user doesn't want to test this signal
                    if (!Convert.ToDouble(testValue.From).Equals(Convert.ToDouble(testValue.To)))
                        // return Frequency as Double since the user wants to test this signal
                        // simply we are ignoring rest of the signal where From and To values are not same
                        // assumption is: if there are From and To values are not equal simply a mistake
                        return Convert.ToDouble(testValue.Frequency);
                }
                return default(double);
            }
        }
        #endregion

    }
}