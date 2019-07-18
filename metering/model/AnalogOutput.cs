using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metering.model
{
    public class AnalogOutput
    {
        public AnalogOutput(string signal, string from, string to, string delta, string phase, string frequency)
        {
            if (string.IsNullOrWhiteSpace(signal))
            {
                throw new ArgumentNullException("Signal");
            }

            Signal = signal;
            From = from;
            To = to;
            Delta = delta;
            Phase = phase;
            Frequency = frequency;
        }

        public string Signal { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Delta { get; set; }

        public string Phase { get; set; }

        public string Frequency { get; set; }
    }
}
