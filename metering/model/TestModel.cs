using System;
using System.ComponentModel;
using System.Diagnostics;

namespace metering.model
{
    public class TestModel {}

    public class TestValue : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string voltage;
        public string Voltage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(voltage))
                {
                    return "120.0";
                }
                return voltage;
            }
            set
            {                
                if (string.Equals(voltage, value))
                {
                    voltage = value;
                    RaisePropertyChanged("voltage");
                }
            }
        }

        public string Current { get; set; } = "50.00";

        public string Frequency { get; set; } = "60.000";

        public string VoltagePhase { get; set; } = "0.00";

        public string CurrentPhase { get; set; } = "0.00";

        public string Delta { get; set; } = "1.00";

        public string RegisterNumber { get; set; }

        public string DwellTime { get; set; } = "120";

        public string StartDelayTime { get; set; } = "0";

        public string MeasurementInterval { get; set; } = "100";

        public string StartMeasurementDelay { get; set; } = "60";

        public string Progress { get; set; } = "0";

        public string Signal { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Phase { get; set; }


        public bool ThrowOnInvalidPropertyName { get; private set; }

        // Verify property exists
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string message = "Invalid property name: " + propertyName;

                if (ThrowOnInvalidPropertyName)
                {
                    throw new Exception(message);
                }
                else
                {
                    Debug.Fail(message);
                }
            }
        }

        /// <summary>
        /// Raises when a property changed.
        /// </summary>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            Debug.WriteLine($"RaisePropertyChanged (propertyName: {propertyName}) processed.");

            VerifyPropertyName(propertyName);

            PropertyChangedEventHandler propertyChangedEventHandler = PropertyChanged;
            if (propertyChangedEventHandler != null)
            {
                var eventArgs = new PropertyChangedEventArgs(propertyName);
                propertyChangedEventHandler(this, eventArgs);
            }
        }
    }
}
