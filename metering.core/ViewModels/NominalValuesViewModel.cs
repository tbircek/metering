using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace metering.core
{
    public class NominalValuesViewModel : BaseViewModel
    {

        #region Public Properties
        /// <summary>
        /// Default Voltage magnitude to use through out the test
        /// </summary>
        public string NominalVoltage { get; set; } = "120.0";

        /// <summary>
        /// Default Current magnitude to use through out the test
        /// </summary>
        public string NominalCurrent { get; set; } = "100.0";

        /// <summary>
        /// Default Frequency magnitude to use through out the test
        /// </summary>
        public string NominalFrequency { get; set; } = "60.00";

        /// <summary>
        /// Default Voltage phase to use through out the test
        /// </summary>
        public string SelectedVoltagePhase { get; set; } = "Voltage.AllZero";

        /// <summary>
        /// Default Current phase to use through out the test
        /// </summary>
        public string SelectedCurrentPhase { get; set; } = "Current.AllZero";

        /// <summary>
        /// Default Delta value to use through out the test
        /// Delta == magnitude difference between test steps
        /// </summary>
        public string NominalDelta { get; set; } = "1.000";

        /// <summary>
        /// Title of AddNewTestCommand
        /// </summary>
        public string AddNewTestCommandTitle { get; set; } = "New Test";


        #endregion

        #region Public Commands

        /// <summary>
        /// The command to handle change view to test plan detail view
        /// and populate items with nominal values
        /// </summary>
        public ICommand AddNewTestCommand { get; set; }

        /// <summary>
        /// The command handles radio button selections
        /// </summary>
        public ICommand RadioButtonCommand { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor.
        /// </summary>
        public NominalValuesViewModel()
        {
            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            RadioButtonCommand = new RelayParameterizedCommand((parameter) => GetSelectedRadioButton((string)parameter));
            AddNewTestCommand = new RelayParameterizedCommand((parameter) => CopyNominalValues((NominalValuesViewModel)parameter));

        }
        #endregion

        #region Helpers

        /// <summary>
        /// Shows test steps with values reset to nominal values
        /// </summary>
        private async void CopyNominalValues(NominalValuesViewModel parameter)
        {
            // Simulate the page creation.
            // await Task.Delay(100);

            // TODO: Pass NominalValues page values to the TestDetails page using Dependency Injection
            await Task.Run(() => IoC.UI.ShowTestDetails(new TestDetailsViewModel()
            {
                Register = "Test- register value",
                DwellTime = "test- dwell time",
                StartDelayTime = "test-StartDelayTime",
                MeasurementInterval = "test-MeasurementInterval",
                StartMeasurementDelay = "test - StartMeasurementDelay",
                TestText = "Test",
                AnalogSignals = new ObservableCollection<AnalogSignalListItemViewModel>
                {
                    new AnalogSignalListItemViewModel
                    {
                        SignalName = "test- v1",
                        From = "test-100.4",
                        To = "-test - 134.6",
                        Delta = "test- 4.333",
                        Phase = "test- 40.000",
                        Frequency = "test- 459.999"
                    },
                    new AnalogSignalListItemViewModel
                    {
                        SignalName = "test- 4v2",
                        From = "1test- 400.4",
                        To = "13test- 44.6",
                        Delta = "4test- 4.333",
                        Phase = "0.0test- 400",
                        Frequency = "59test- 4.999"
                    }
                }
            }));

            //await Task.Run(() =>
            //{

            //});
            //// Show TestDetails page
            //await Task.Factory.StartNew(() => IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.TestDetails));
            //Debug.WriteLine("CopyNominalValues() is running:");
        }

        /// <summary>
        /// The command handles radio button selection events
        /// </summary>
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

        #endregion
    }
}
