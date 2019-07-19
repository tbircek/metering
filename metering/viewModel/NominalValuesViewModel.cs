using metering.model;
using System;
using System.Collections.ObjectModel;

namespace metering.viewModel
{
    public class NominalValues: ViewModelBase
    {
        private string nominalVoltage;
        private string nominalCurrent;
        private string nominalFrequency;
        private string nominalDelta;
        
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
