using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metering.model
{
    public class NominalModel
    {
        public NominalModel(string voltage, string current, string frequency, string voltagePhase, string currentPhase, string delta)
        {
            Voltage = voltage;
            Current = current;
            Frequency = frequency;
            VoltagePhase = voltagePhase;
            CurrentPhase = currentPhase;
            Delta = delta;
        }

        public string Voltage { get; set; } = "120.0";

        public string Current { get; set; } = "50.00";

        public string Frequency { get; set; } = "60.000";

        public string VoltagePhase { get; set; } = "0.00";

        public string CurrentPhase { get; set; } = "0.00";

        public string Delta { get; set; } = "1.00";
    }
}
