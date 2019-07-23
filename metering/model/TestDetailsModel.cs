using System.Collections.ObjectModel;

namespace metering.model
{
    public class TestDetailsModel
    {
        public string Register { get; set; }
        public string Progress { get; set; }
        public string DwellTime { get; set; }
        public string StartDelayTime { get; set; }
        public string MeasurementInterval { get; set; }
        public string StartMeasurementDelay { get; set; }
        public ObservableCollection<TestDetail> TestDetails { get; set; }

        public TestDetailsModel GetTestDetailsModel()
        {
            return new TestDetailsModel();
        }

        public TestDetailsModel GetTestDetailsModel(string register, string progress, string dwellTime, string startDelayTime, string measurementInterval, string startMeasurementDelay, ObservableCollection<TestDetail> testDetails)
        {
            return new TestDetailsModel
            {
                Register = register,
                Progress = progress,
                DwellTime = dwellTime,
                StartDelayTime = startDelayTime,
                MeasurementInterval = measurementInterval,
                StartMeasurementDelay = startMeasurementDelay,
                TestDetails = testDetails
            };
        }
    }
}
