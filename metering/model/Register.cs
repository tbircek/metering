using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace metering.model
{
    public class Register
    {
        public Register(string registerNumber, string dwellTime, string startDelayTime, string measurementInterval, string startMeasurementDelay, string progress)
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

        public string DwellTime { get; set; }

        public string StartDelayTime { get; set; }

        public string MeasurementInterval { get; set; }

        public string StartMeasurementDelay { get; set; }

        public string Progress { get; set; }
    }
}
