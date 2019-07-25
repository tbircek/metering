using metering.model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace metering.viewModel
{
    public class TestDetailsViewModel : ViewModelBase
    {

        private static TestDetailsModel model = new TestDetailsModel();
        private static TestDetailModel testDetail = new TestDetailModel();

        public TestDetailsViewModel()
        {

        }

        public TestDetailsViewModel(string register, string progress, string dwellTime, string startDelayTime, string measurementInterval, string startMeasurementDelay, ObservableCollection<TestDetailModel> testDetails)
        {
            Register = register;
            Progress = progress;
            DwellTime = dwellTime;
            StartDelayTime = startDelayTime;
            MeasurementInterval = measurementInterval;
            StartMeasurementDelay = startMeasurementDelay;
            // model.TestDetail = testDetails;
            //model.TestDetail.CollectionChanged += TestDetails_CollectionChanged;
        }

        //private void TestDetails_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    // throw new System.NotImplementedException();
        //    ObservableCollection<TestDetailModel> senderCollection = sender as ObservableCollection<TestDetailModel>;
        //    NotifyCollectionChangedAction action = e.Action;

        //    Debug.WriteLine($"Collection action:{e.Action}");
        //    if (action == NotifyCollectionChangedAction.Add)
        //    {
        //        //if (SetProperty(model.TestDetail, senderCollection))
        //        //{
        //        //    model.TestDetail = senderCollection;
        //        //}
        //        Debug.WriteLine("Collection changed. Clear first than put new values");

        //        //foreach (var item in senderCollection)
        //        //{
        //        //    testDetail.From = item.From;
        //        //}
        //    }
        //    else if (action == NotifyCollectionChangedAction.Reset)
        //    {
        //        Debug.WriteLine("Collection changed. Clear first than put new values");
        //    }
        //    model.TestDetail = senderCollection;
        //}

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
                if (SetProperty(model.DwellTime, value))
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
                if (SetProperty(model.StartDelayTime, value))
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
            {
                if (SetProperty(model.StartMeasurementDelay, value))
                {
                    model.StartMeasurementDelay = value;
                }
            }
        }

        public ObservableCollection<TestDetailModel> TestDetails { get; set; }
        //{

            //get => model.TestDetail;
            //set => model.TestDetail = value;
        //}
    }
}
