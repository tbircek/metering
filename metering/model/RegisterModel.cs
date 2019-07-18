using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metering.model
{
    public class RegisterModel
    {
        public RegisterModel(string registerNumber, string dwellTime, string startDelayTime, string measurementInterval, string startMeasurementDelay, string progress)
        {
            if (string.IsNullOrWhiteSpace(registerNumber))
            {
                throw new ArgumentNullException("Register");
            }

            RegisterNumber = registerNumber;
            DwellTime = dwellTime;
            StartDelayTime = startDelayTime;
            MeasurementInterval = measurementInterval;
            StartMeasurementDelay = startMeasurementDelay;
            Progress = progress;
        }

        public string RegisterNumber { get; set; }

        public string DwellTime { get; set; } = "120";

        public string StartDelayTime { get; set; } = "0";

        public string MeasurementInterval { get; set; } = "100";

        public string StartMeasurementDelay { get; set; } = "60";

        public string Progress { get; set; } = "0";
    }
}
