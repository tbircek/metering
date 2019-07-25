namespace metering.model
{
    public class TestDetailModel
    {
        public string SignalName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Delta { get; set; }
        public string Phase { get; set; }
        public string Frequency { get; set; }

        public TestDetailModel()
        {

        }

        /// <summary>
        /// Holds test details specified by the user.
        /// </summary>
        /// <param name="signalName">Omicron test set analog signal.</param>
        /// <param name="from">Test start magnitude given for Omicron analog output.</param>
        /// <param name="to">Test end magnitude given for Omicron analog output.</param>
        /// <param name="delta">The magnitude increment between tests.</param>
        /// <param name="phase">The magnitude phase to apply.</param>
        /// <param name="frequency">The magnitude frequency to apply.</param>
        public TestDetailModel(string signalName, string from, string to, string delta, string phase, string frequency)
        {
            SignalName = signalName;
            From = from;
            To = to;
            Delta = delta;
            Phase = phase;
            Frequency = frequency;
        }

    }
}