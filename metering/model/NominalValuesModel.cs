using System.Collections.ObjectModel;

namespace metering.model
{
    public class NominalValuesModel
    {
        public string Voltage { get; set; } = "120.0";
        public string Current { get; set; } = "200.0";
        public string Frequency { get; set; } = "60.000";
        public string Delta { get; set; } = "1.000";
        public string SelectedVoltagePhase { get; set; } = "AllZero";
        public string SelectedCurrentPhase { get; set; } = "AllZero";

        //private readonly ObservableCollection<VPhases>
        //public string V1Phase { get; set; } = "0";
        //public string V2Phase { get; set; } = "0";
        //public string V3Phase { get; set; } = "0";
        //public string V4Phase { get; set; } = "0";

        //public string I1Phase { get; set; } = "0";
        //public string I2Phase { get; set; } = "0";
        //public string I3Phase { get; set; } = "0";
        //public string I4Phase { get; set; } = "0";
        //public string I5Phase { get; set; } = "0";
        //public string I6Phase { get; set; } = "0";

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
