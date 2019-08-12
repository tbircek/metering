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

            // TODO: Pass NominalValues page values to the TestDetails page;
            IoC.ContentManager.PassNominalValues(new ObservableCollection<AnalogSignalListItemViewModel>
            {
               for (int i = 1; i < 4; i++)
               {
                 
               }
            });

            // Show TestDetails page
            await Task.Factory.StartNew(() => IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.TestDetails));
            Debug.WriteLine("CopyNominalValues() is running:");
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
