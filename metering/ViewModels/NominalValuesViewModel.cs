using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace metering
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

        #endregion

        #region Public Commands

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

        }
        #endregion

        #region Helpers

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
