using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metering.model
{
    public class Nominal
    {
        public Nominal(string voltage, string current, string frequency, string voltagePhase, string currentPhase, string delta)
        {
            Voltage = voltage;
            Current = current;
            Frequency = frequency;
            VoltagePhase = voltagePhase;
            CurrentPhase = currentPhase;
            Delta = delta;
        }

        public string Voltage { get; set; }

        public string Current { get; set; }

        public string Frequency { get; set; }

        public string VoltagePhase { get; set; }

        public string CurrentPhase { get; set; }

        public string Delta { get; set; }
    }
}
