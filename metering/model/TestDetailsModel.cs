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
        public ObservableCollection<TestDetailModel> TestDetail { get; set; }

        public TestDetailsModel()
        {
        }

        public TestDetailsModel(string register, string progress, string dwellTime, string startDelayTime, string measurementInterval, string startMeasurementDelay)//, ObservableCollection<TestDetail> testDetails)
        {

            Register = register;
            Progress = progress;
            DwellTime = dwellTime;
            StartDelayTime = startDelayTime;
            MeasurementInterval = measurementInterval;
            StartMeasurementDelay = startMeasurementDelay;
            //  TestDetail = testDetails;
        }
    }
}
