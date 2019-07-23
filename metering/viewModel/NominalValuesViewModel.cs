using System.Diagnostics;
using System.Windows.Input;
using metering.model;

namespace metering.viewModel
{
    public class NominalValuesViewModel: ViewModelBase
    {
        // TODO: public radioButtonOptions for NominalVoltagePhase
        // TODO: public radioButtonOptions for NominalCurrentPhase
        private static NominalValuesModel model = new NominalValuesModel();
        DelegateCommand addNewTestCommand;
        // DelegateCommand radioButtonCommand;
        
        
        public string NominalVoltage
        {
            get => model.Voltage;
            set
            {
                SetProperty(model.Voltage, value);
                model.Voltage = value;
            }
        }

        public string NominalCurrent
        {
            get => model.Current;
            set
            {
                SetProperty(model.Current, value);
                model.Current = value;
            }
        }

        public string NominalFrequency
        {
            get => model.Frequency;
            set
            {
                SetProperty(model.Frequency, value);
                model.Frequency = value;
            }
        }

        public string VoltagePhase
        {
            get => model.VoltagePhase;
            set
            {
                SetProperty(model.VoltagePhase, value);
                model.VoltagePhase = value;
            }
        }

        public string CurrentPhase
        {
            get => model.CurrentPhase;
            set
            {
                SetProperty(model.CurrentPhase, value);
                model.CurrentPhase = value;
            }
        }

        public string NominalDelta
        {
            get => model.Delta;
            set
            {
                SetProperty(model.Delta, value);
                model.Delta = value;
            }
        }

        public ICommand RadioButtonCommand
        {
            get
            {
                if (radioButtonCommand == null)
                {
                    radioButtonCommand = new DelegateCommand(
                        param => GetSelectedRadioButton(),
                        param => true);
                }
                return radioButtonCommand;
            }
        }

        private void GetSelectedRadioButton()
        {
            // throw new NotImplementedException();            
            Debug.WriteLine($"selected an option...{VoltagePhase}");
        }

        public ICommand AddNewTestCommand
        {
            get
            {
                if (addNewTestCommand == null)
                {
                    addNewTestCommand = new DelegateCommand(
                    param => CopyNominalValues(),
                    param => true);

                }
                return addNewTestCommand;
            }
        }

        private void CopyNominalValues()
        {
            // throw new NotImplementedException();
            Debug.WriteLine("Following values reported:");

            // TODO: This variable must be obtain thru Omicron Test Set.
            int omicronVoltageOutputNumber = 4;
            for (int i = 1; i <= omicronVoltageOutputNumber; i++)
            {
                Debug.WriteLine($"signal: v{i}\tfrom: {model.Voltage}\tto: {model.Voltage}\tdelta: {model.Delta}\tphase: {0}\tfrequency: {model.Frequency}");
            }

            // TODO: This variable must be obtain thru Omicron Test Set.
            int omicronCurrentOutputNumber = 6;
            for (int i = 1; i <= omicronCurrentOutputNumber; i++)
            {
                Debug.WriteLine($"signal: i{i}\tfrom: {model.Current}\tto: {model.Current}\tdelta: {model.Delta}\tphase: {0}\tfrequency: {model.Frequency}");
            }
            Debug.WriteLine("TODO: show new TestDetailsView");
        }
    }
}
