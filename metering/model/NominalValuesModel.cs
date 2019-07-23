﻿namespace metering.model
{
    public class NominalValuesModel
    {
        public string Voltage { get; set; } = "120.0";
        public string Current { get; set; } = "200.0";
        public string Frequency { get; set; } = "60.000";
        public string Delta { get; set; } = "1.000";
        public string VoltagePhase { get; set; } = "0";
        public string CurrentPhase { get; set; } = "0";

        public NominalValuesModel GetNominalValuesModel()
        {
            return new NominalValuesModel();
        }

        public NominalValuesModel GetNominalValuesModel( string voltage, string current, string frequency, string voltagePhase, string currentPhase, string delta)
        {
            return new NominalValuesModel
            {
                Voltage = voltage,
                Current = current,
                Frequency = frequency,
                VoltagePhase = voltagePhase,
                CurrentPhase = currentPhase,
                Delta = delta
            };
        }

        // TODO: Implement IDataErrorInfo
    }
}
