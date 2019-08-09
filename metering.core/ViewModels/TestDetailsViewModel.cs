using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;

namespace metering.core
{
    public class TestDetailsViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// Omicron Analog Output Signals.
        /// </summary>
        public ObservableCollection<AnalogSignalListItemViewModel> AnalogSignals { get; set; }

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


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public TestDetailsViewModel()
        {
            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
        }
        #endregion

    }
}
