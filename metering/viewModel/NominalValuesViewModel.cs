using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using metering.model;

namespace metering.viewModel
{
    public class NominalValuesViewModel : ViewModelBase
    {
        private static NominalValuesModel model = new NominalValuesModel();
        // private static TestDetail testDetail = new TestDetail();
        private static ObservableCollection<TestDetailModel> testDetails = new ObservableCollection<TestDetailModel>();
        // private ObservableCollection<Person> persons = new ObservableCollection<Person>();
        private static TestDetailsModel testDetailsModel = new TestDetailsModel("", "", "", "", "", "");// , testDetails);

        // private bool CollectionChanged = false;

        DelegateCommand addNewTestCommand;
        DelegateCommand cancelNewTestCommand;
        DelegateCommand radioButtonCommand;

        public NominalValuesViewModel()
        {
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

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

        public ICommand CancelNewTestCommand
        {
            get
            {
                if (cancelNewTestCommand == null)
                {
                    cancelNewTestCommand = new DelegateCommand(
                    param => CancelNominalValues(),
                    param => true);

                }
                return cancelNewTestCommand;
            }
        }

        private void CancelNominalValues()
        {
            // throw new NotImplementedException();
            Debug.WriteLine("CancelNominalValues is running:");
            testDetailsModel.TestDetail.Clear();
            // TestDetailsModel testDetailsModel = new TestDetailsModel();
            // model.Voltage = "";
        }

        private void CopyNominalValues()
        {
            if (testDetailsModel.TestDetail != null)
            {
                testDetailsModel.TestDetail.Clear();
            }
            else
            {
                testDetailsModel.TestDetail = new ObservableCollection<TestDetailModel>();
            }

            // throw new NotImplementedException();
            Debug.WriteLine("Following values reported:");

            // TODO: This variable must be obtain thru Omicron Test Set.
            int omicronVoltageOutputNumber = 4;
            for (int i = 1; i <= omicronVoltageOutputNumber; i++)
            {
                string[] phase;
                if (model.SelectedVoltagePhase == "Balanced")
                {
                    phase = new string[] { "0", "-120", "120", "0" };
                }
                else
                {
                    phase = new string[] { "0", "0", "0", "0" };
                }

                Debug.WriteLine($"signal: v{i}\tfrom: {model.Voltage}\tto: {model.Voltage}\tdelta: {model.Delta}\tphase: {phase[i - 1]}\tfrequency: {model.Frequency}");
                TestDetailModel test = new TestDetailModel
                {
                    SignalName = "v" + i,
                    From = model.Voltage,
                    To = model.Voltage,
                    Delta = model.Delta,
                    Phase = phase[i - 1],
                    Frequency = model.Frequency
                };
                testDetailsModel.TestDetail.Add(test);



                //TestDetailsModel test = new TestDetailsModel("","","","","","",)
                //test = new TestDetail("v" + i, model.Voltage, model.Voltage, model.Delta, phase[i - 1], model.Frequency);
                //testDetails.Add(test);
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
                TestDetailModel test = new TestDetailModel
                {
                    SignalName = "i" + i,
                    From = model.Current,
                    To = model.Current,
                    Delta = model.Delta,
                    Phase = phase[i - 1],
                    Frequency = model.Frequency
                };
                testDetailsModel.TestDetail.Add(test);
            }
            Debug.WriteLine("TODO: show new TestDetailsView");

            TestDetailsViewModel testDetailsViewModel = new TestDetailsViewModel("", "", "", "", "", "", testDetailsModel.TestDetail);
        }
    }
}
