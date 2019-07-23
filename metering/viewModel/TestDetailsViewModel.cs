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
            set => SetProperty( register, value);
        }

        public string Progress
        {
            get => progress;
            set => SetProperty( progress, value);
        }

        public string DwellTime
        {
            get => dwellTime;
            set => SetProperty( dwellTime, value);
        }

        public string StartDelayTime
        {
            get => startDelayTime;
            set => SetProperty( startDelayTime, value);
        }

        public string MeasurementInterval
        {
            get => measurementInterval;
            set => SetProperty( measurementInterval, value);
        }

        public string StartMeasurementDelay
        {
            get => startMeasurementDelay;
            set => SetProperty( startMeasurementDelay, value);
        }

        public TestDetail Test
        {
            get { return test; }
            set { SetProperty( test, value); }
        }
    }
}
