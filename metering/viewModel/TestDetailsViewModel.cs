using metering.model;
using System.Collections.ObjectModel;

namespace metering.viewModel
{
    public class TestDetailsViewModel : ViewModelBase
    {
        private static TestDetailsModel model = new TestDetailsModel();

        public string Register
        {
            get => model.Register;
            set
            {
                if (SetProperty(model.Register, value))
                {
                    model.Register = value;
                }
            }
        }

        public string Progress
        {
            get => model.Progress;
            set
            {
                if (SetProperty(model.Progress, value))
                {
                    model.Progress = value;
                }
            }
        }

        public string DwellTime
        {
            get => model.DwellTime;
            set
            {
               if( SetProperty(model.DwellTime, value))
                {
                    model.DwellTime = value;
                }
            }
        }

        public string StartDelayTime
        {
            get => model.StartDelayTime;
            set
            {
                if ( SetProperty(model.StartDelayTime, value)) 
                    {
                    model.StartDelayTime = value;
                    }
                }
        }

        public string MeasurementInterval
        {
            get => model.MeasurementInterval;
            set
            {
                if (SetProperty(model.MeasurementInterval, value))
                {
                    model.MeasurementInterval = value;
                }
            } 
        }

        public string StartMeasurementDelay
        {
            get => model.StartMeasurementDelay;
            set
            { if (SetProperty(model.StartMeasurementDelay, value))
                {
                    model.StartMeasurementDelay = value;
                }
            }
        }

        public ObservableCollection<string> Test
        {
            get
            {
                foreach (var test in model.TestDetails)
                {
                    Test.Add(test.SignalName);
                    Test.Add(test.From);
                    Test.Add(test.To);
                    Test.Add(test.Delta);
                    Test.Add(test.Phase);
                    Test.Add(test.Frequency);
                }
                return Test;
            }
            set
            {
                //foreach (var test in value)
                //{
                //    if (SetProperty(model.TestDetails, value))
                //    {

                //    }
                //}
                
            }
        }

    }
}
