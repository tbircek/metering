using metering.model;
using System.Collections.ObjectModel;

namespace metering.viewModel
{
    public class TestViewModel
    {
       public ObservableCollection<TestValue> TestValues
        {
            get;
            set;
        }

        public void LoadTestValues()
        {
            ObservableCollection<TestValue> testValues = new ObservableCollection<TestValue>
            {
                //ObservableCollection<Communication> communications = new ObservableCollection<Communication>();

                new TestValue
                {
                    Voltage = "12.0.0",
                    Current = "10.0",
                    Frequency = "60.000",
                    VoltagePhase = "0",
                    CurrentPhase = "0",
                    Delta = "1.0",
                    DwellTime = "120",
                    StartDelayTime = "30",
                    MeasurementInterval = "100",
                    StartMeasurementDelay = "60"
                }
            };

            TestValues = testValues;
        }
    }
}
