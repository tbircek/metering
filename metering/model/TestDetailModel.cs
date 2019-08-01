namespace metering
{
    //public class TestDetailList : ObservableCollection<Test>
    //{
    //    public TestDetailList():base()
    //    {
    //        Add(new Test("v1", "100.0", "120.0", "2.345", "0", "59.999"));
    //        Add(new Test("v2", "100.0", "120.0", "2.345", "-120", "59.999"));
    //        Add(new Test("v3", "100.0", "120.0", "2.345", "120", "59.999"));
    //        Add(new Test("v4", "100.0", "120.0", "2.345", "0", "59.999"));
    //        Add(new Test("i1", "25.0", "35.0", "0.100", "0", "59.999"));
    //        Add(new Test("i2", "25.0", "35.0", "0.100", "-120", "59.999"));
    //        Add(new Test("i3", "25.0", "35.0", "0.100", "120", "59.999"));
    //        Add(new Test("i4", "25.0", "35.0", "0.100", "0", "59.999"));
    //        Add(new Test("i5", "25.0", "35.0", "0.100", "-120", "59.999"));
    //        Add(new Test("i6", "25.0", "35.0", "0.100", "1200", "59.999"));
    //    }
    //}

    public class Test
    {
        private string signalName { get; set; }
        private string from { get; set; }
        private string to { get; set; }
        private string delta { get; set; }
        private string phase { get; set; }
        private string frequency { get; set; }

        public Test(string signalName, string from, string to, string delta, string phase, string frequency)
        {
            this.signalName = signalName;
            this.from = from;
            this.to = to;
            this.delta = delta;
            this.phase = phase;
            this.frequency = frequency;
        }

        public string SignalName
        {
            get { return signalName; }
            set { signalName = value; }
        }

        public string From
        {
            get { return from; }
            set { from = value; }
        }

        public string To
        {
            get { return to; }
            set { to = value; }
        }

        public string Delta
        {
            get { return delta; }
            set { delta = value; }
        }

        public string Phase
        {
            get { return phase; }
            set { phase = value; }
        }

        public string Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }
    }


    //public class TestDetailModel
    //{
    //    public string SignalName { get; set; }
    //    public string From { get; set; }
    //    public string To { get; set; }
    //    public string Delta { get; set; }
    //    public string Phase { get; set; }
    //    public string Frequency { get; set; }

    //    public TestDetailModel CreateNewTestDetail()
    //    {
    //        return new TestDetailModel();
    //    }

    //    /// <summary>
    //    /// Holds test details specified by the user.
    //    /// </summary>
    //    /// <param name="signalName">Omicron test set analog signal.</param>
    //    /// <param name="from">Test start magnitude given for Omicron analog output.</param>
    //    /// <param name="to">Test end magnitude given for Omicron analog output.</param>
    //    /// <param name="delta">The magnitude increment between tests.</param>
    //    /// <param name="phase">The magnitude phase to apply.</param>
    //    /// <param name="frequency">The magnitude frequency to apply.</param>
    //    public TestDetailModel CreateNewTestDetail(string signalName, string from, string to, string delta, string phase, string frequency)
    //    {
    //        return new TestDetailModel
    //        {
    //            SignalName = signalName,
    //            From = from,
    //            To = to,
    //            Delta = delta,
    //            Phase = phase,
    //            Frequency = frequency
    //        };
    //    }

    //}
}