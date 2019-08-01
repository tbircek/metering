using System.Globalization;
using System.Threading;
using PropertyChanged;

namespace metering
{
    [AddINotifyPropertyChangedInterface]
    public class TestDetailViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Omicron Analog Output name.
        /// </summary>
        public string SignalName { get; set; }

        /// <summary>
        /// Omicron Analog Output start magnitude 
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Omicron Analog Output end magnitude 
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Omicron Analog Output magnitude increment/decrement
        /// </summary>
        public string Delta { get; set; }

        /// <summary>
        /// Omicron Analog Output phase
        /// </summary>
        public string Phase { get; set; }

        /// <summary>
        /// Omicron Analog Output frequency
        /// </summary>
        public string Frequency { get; set; }
        #endregion


        // private static Test model = new Test("", "", "", "", "", "");

        public TestDetailViewModel()
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

        }

        //public TestDetailViewModel(string signalName, string from, string to, string delta, string phase, string frequency)
        //{
        //    SignalName = signalName;
        //    From = from;
        //    To = to;
        //    Delta = delta;
        //    Phase = phase;
        //    Frequency = frequency;
        //}

    }
}
