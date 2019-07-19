using metering.model;
using System.Collections.ObjectModel;

namespace metering.viewModel
{
    public class TestDetailsViewModel : ViewModelBase
    {
        private string register;
        private string progress;
        private string dwellTime;
        private string startDelayTime;
        private string measurementInterval;
        private string startMeasurementDelay;
        private TestDetail test;

        public string Register
        {
            get => register;
            set => SetProperty(ref register, value);
        }

        public string Progress
        {
            get => progress;
            set => SetProperty(ref progress, value);
        }

        public string DwellTime
        {
            get => dwellTime;
            set => SetProperty(ref dwellTime, value);
        }

        public string StartDelayTime
        {
            get => startDelayTime;
            set => SetProperty(ref startDelayTime, value);
        }

        public string MeasurementInterval
        {
            get => measurementInterval;
            set => SetProperty(ref measurementInterval, value);
        }

        public string StartMeasurementDelay
        {
            get => startMeasurementDelay;
            set => SetProperty(ref startMeasurementDelay, value);
        }

        public TestDetail Test
        {
            get { return test; }
            set { SetProperty(ref test, value); }
        }
    }
}
