using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using metering.model;

namespace metering.viewModel
{
    public class NominalValuesViewModel: ViewModelBase
    {
        private static NominalValuesModel model = new NominalValuesModel();
        private static TestDetailsViewModel test = new TestDetailsViewModel();

        DelegateCommand addNewTestCommand;
        DelegateCommand radioButtonCommand;
        
        public NominalValuesViewModel()
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            //CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(NavigationCommands.ShowTestCommand, ShowTestCommandExecuted));
        }
        
        public string NominalVoltage
        {
            get => model.Voltage;
            set
            {
                if (SetProperty(model.Voltage, value))
                {
                    model.Voltage = value;
                }
            }
        }

        public string NominalCurrent
        {
            get => model.Current;
            set
            {
                if (SetProperty(model.Current, value))
                {
                    model.Current = value;
                }
            }
        }

        public string NominalFrequency
        {
            get => model.Frequency;
            set
            {
                if (SetProperty(model.Frequency, value))
                {
                    model.Frequency = value;
                }
            }
        }

        public string SelectedVoltagePhase
        {
            get => model.SelectedVoltagePhase;
            set
            {
                if (SetProperty(model.SelectedVoltagePhase, value))
                {
                    model.SelectedVoltagePhase = value;
                }
            }
        }

        public string SelectedCurrentPhase
        {
            get => model.SelectedCurrentPhase;
            set
            {
                if (SetProperty(model.SelectedCurrentPhase, value))
                {
                    model.SelectedCurrentPhase = value;
                }                 
            }
        }

        public string NominalDelta
        {
            get => model.Delta;
            set
            {
                if (SetProperty(model.Delta, value))
                {
                    model.Delta = value;
                }
            }
        }

        public ICommand RadioButtonCommand
        {
            get
            {
                if (radioButtonCommand == null)
                {
                    radioButtonCommand = new DelegateCommand(
                        param => GetSelectedRadioButton((string)param),
                        param => true);
                }
                return radioButtonCommand;
            }
        }

        private void GetSelectedRadioButton(string param)
        {
            // throw new NotImplementedException();
            string type = param.Split('.')[0];
            string option = param.Split('.')[1];

            switch (type)
            {
                case "Voltage":
                    SelectedVoltagePhase = option;
                    break;
                case "Current":
                    SelectedCurrentPhase = option;
                    break;
                default:
                    break;
            }
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
                string [] phase;
                if (model.SelectedVoltagePhase == "Balanced")
                {
                    phase = new string[] { "0", "-120", "120", "0" }; 
                }
                else
                {
                    phase = new string[] { "0", "0", "0", "0" };
                }
                
                Debug.WriteLine($"signal: v{i}\tfrom: {model.Voltage}\tto: {model.Voltage}\tdelta: {model.Delta}\tphase: {phase[i-1]}\tfrequency: {model.Frequency}");

                //TestDetailModel testDetail = new TestDetailModel("v" + i, model.Voltage, model.Voltage, model.Delta, phase[i - 1], model.Frequency);
                
                
            }

            // TODO: This variable must be obtain thru Omicron Test Set.
            int omicronCurrentOutputNumber = 6;
            for (int i = 1; i <= omicronCurrentOutputNumber; i++)
            {
                string[] phase;
                if (model.SelectedCurrentPhase == "Balanced")
                {
                    phase = new string[] { "0", "-120", "120", "0", "-120", "120" }; 
                }
                else
                {
                    phase = new string[] { "0", "0", "0", "0", "0", "0" };
                }
                
                Debug.WriteLine($"signal: i{i}\tfrom: {model.Current}\tto: {model.Current}\tdelta: {model.Delta}\tphase: {phase[i - 1]}\tfrequency: {model.Frequency}");
            }
            Debug.WriteLine("TODO: show new TestDetailsView");
        }
    }
}
