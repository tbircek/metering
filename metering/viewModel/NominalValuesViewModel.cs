namespace metering.viewModel
{
    public class NominalValuesViewModel: ViewModelBase
    {
        private string nominalVoltage = "120.0";
        private string nominalCurrent = "100.00";
        private string nominalFrequency = "60.000";
        private string nominalDelta = "1.00";

        public string NominalVoltage
        {
            get => nominalVoltage;
            set => SetProperty(ref nominalVoltage, value);
        }

        public string NominalCurrent
        {
            get => nominalCurrent;
            set => SetProperty(ref nominalCurrent, value);
        }

        public string NominalFrequency
        {
            get => nominalFrequency;
            set => SetProperty(ref nominalFrequency, value);
        }
        public string NominalDelta
        {
            get => nominalDelta;
            set => SetProperty(ref nominalDelta, value);
        }
        // TODO: public radioButtonOptions for NominalVoltagePhase
        // TODO: public radioButtonOptions for NominalCurrentPhase
    }
}
