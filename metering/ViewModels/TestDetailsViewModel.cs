using System.Globalization;
using System.Threading;
using PropertyChanged;

namespace metering
{
    [AddINotifyPropertyChangedInterface]
    public class TestDetailsViewModel : BaseViewModel
    {
        #region Public Properties
        /// <summary>
        /// The register to monitor while testing.
        /// </summary>
        public string Register { get; set; }

        /// <summary>
        /// Show test completion percentage.
        /// </summary>
        public string Progress { get; set; } = "0.0";

        /// <summary>
        /// How long should <see cref="Register"/> be poll.
        /// </summary>
        public string DwellTime { get; set; } = "120";

        /// <summary>
        /// The time to wait until test step #1.
        /// </summary>
        public string StartDelayTime { get; set; } = "30";

        /// <summary>
        /// How often should <see cref="Register"/> be poll.
        /// </summary>
        public string MeasurementInterval { get; set; } = "100";

        /// <summary>
        /// The time to wait after analog signals applied before <see cref="DwellTime"/> starts.
        /// </summary>
        public string StartMeasurementDelay { get; set; } = "10";

        #endregion

        //private static TestDetailsModel model = new TestDetailsModel();
        //private static ObservableCollection<Test> tests = new ObservableCollection<Test>();

        public TestDetailsViewModel()
        {
            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

        }

        //public TestDetailsViewModel(string register, string progress, string dwellTime, string startDelayTime, string measurementInterval, string startMeasurementDelay, ObservableCollection<Test> testDetails)
        //{
        //    Register = register;
        //    Progress = progress;
        //    DwellTime = dwellTime;
        //    StartDelayTime = startDelayTime;
        //    MeasurementInterval = measurementInterval;
        //    StartMeasurementDelay = startMeasurementDelay;
        //    tests = testDetails;
        //    //model.TestDetail.CollectionChanged += TestDetails_CollectionChanged;
        //}

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

        //public ObservableCollection<Test> Test 
        //{

        //    get => tests;
        //    set => tests = value;
        //}
    }
}
