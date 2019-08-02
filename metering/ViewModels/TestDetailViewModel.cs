using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace metering
{
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

        /// <summary>
        /// A list of all Omicron Signal values for each test
        /// </summary>
        public ObservableCollection<TestDetailViewModel> TestStep { get; set; }
        #endregion

        #region Public Command
        /// <summary>
        /// Initializes new view with Nominal Values specified by the user.
        /// </summary>
        public ICommand AddTestCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="signalName"><see cref="SignalName"/></param>
        /// <param name="from"><see cref="From"/></param>
        /// <param name="to"><see cref="To"/></param>
        /// <param name="delta"><see cref="Delta"/></param>
        /// <param name="phase"><see cref="Phase"/></param>
        /// <param name="frequency"><see cref="Frequency"/></param>
        public TestDetailViewModel(string signalName, string from, string to, string delta, string phase, string frequency)
        {
            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            // set up test step values.
            SignalName = signalName;
            From = from;
            To = to;
            Delta = delta;
            Phase = phase;
            Frequency = frequency;
        }

        #endregion

        #region Helper Methods
        /// <summary>
        /// Clear the TestSteps
        /// </summary>
        private void ClearTestStep()
        {
            TestStep = new ObservableCollection<TestDetailViewModel>();

        }




        /// <summary>
        /// Adds all test step to view with nominal values specified previous view
        /// </summary>
        private void NewTest()
        {

            Debug.WriteLine("Following values reported:");

            // TODO: This variable must be obtain thru Omicron Test Set.
            int omicronVoltageOutputNumber = 4;
            for (int i = 1; i <= omicronVoltageOutputNumber; i++)
            {
                string[] phase;
                if (Phase == "Balanced")
                {
                    phase = new string[] { "0", "-120", "120", "0" };
                }
                else
                {
                    phase = new string[] { "0", "0", "0", "0" };
                }

                Debug.WriteLine($"signal: v{i}\tfrom: {From}\tto: {To}\tdelta: {Delta}\tphase: {phase[i - 1]}\tfrequency: {Frequency}");
            }

            // TODO: This variable must be obtain thru Omicron Test Set.
            int omicronCurrentOutputNumber = 6;
            for (int i = 1; i <= omicronCurrentOutputNumber; i++)
            {
                string[] phase;
                if (Phase == "Balanced")
                {
                    phase = new string[] { "0", "-120", "120", "0", "-120", "120" };
                }
                else
                {
                    phase = new string[] { "0", "0", "0", "0", "0", "0" };
                }

                Debug.WriteLine($"signal: i{i}\tfrom: {From}\tto: {To}\tdelta: {Delta}\tphase: {phase[i - 1]}\tfrequency: {Frequency}");
            }
            Debug.WriteLine("TODO: show new TestDetailsView");
        }

        #endregion

    }
}
