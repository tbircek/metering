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
    }
}
