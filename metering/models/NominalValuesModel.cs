namespace metering
{
    public class NominalValuesModel
    {
        public string Voltage { get; set; } = "120.0";
        public string Current { get; set; } = "200.0";
        public string Frequency { get; set; } = "60.000";
        public string Delta { get; set; } = "1.000";
        public string SelectedVoltagePhase { get; set; } = "AllZero";
        public string SelectedCurrentPhase { get; set; } = "AllZero";

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
                SelectedVoltagePhase = voltagePhase,
                SelectedCurrentPhase = currentPhase,
                Delta = delta
            };
        }

        // TODO: Implement IDataErrorInfo
    }
}
