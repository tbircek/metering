namespace metering.model
{
    public class TestDetail
    {
        public string SignalName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Delta { get; set; }
        public string Phase { get; set; }
        public string Frequency { get; set; }

        public TestDetail GetTestDetailModel()
        {
            return new TestDetail();
        }

        public TestDetail GetTestDetailModel(string signalName, string from, string to, string delta, string phase, string frequency)
        {
            return new TestDetail
            {
                SignalName = signalName,
                From = from,
                To = to,
                Delta = delta,
                Phase = phase,
                Frequency = frequency
            };
        }
    }
}