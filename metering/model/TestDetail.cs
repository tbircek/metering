namespace metering.model
{
    public class TestDetail
    {
        public TestDetail(string signalName, string from, string to, string delta, string phase, string frequency)
        {
            SignalName = signalName;
            From = from;
            To = to;
            Delta = delta;
            Phase = phase;
            Frequency = frequency;
        }

        public string SignalName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Delta { get; set; }
        public string Phase { get; set; }
        public string Frequency { get; set; }
    }
}