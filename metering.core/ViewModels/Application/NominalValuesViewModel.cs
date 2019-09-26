using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace metering.core
{
    /// <summary>
    /// Handles Nominal Values page.
    /// </summary>
    public class NominalValuesViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// Group box title for Nominal Analog Values
        /// </summary>
        public string AnalogValuesHeaderTitle { get; set; } = Resources.Strings.global_nominal_analog_header;

        /// <summary>
        /// Hint for the Nominal Voltage text entry
        /// </summary>
        public string NominalVoltageHint { get; set; } = Resources.Strings.global_nominal_voltage;

        /// <summary>
        /// Default Voltage magnitude to use through out the test
        /// </summary>
        public string NominalVoltage { get; set; } = "0.000";

        /// <summary>
        /// Hint for the Nominal Current text entry
        /// </summary>
        public string NominalCurrentHint { get; set; } = Resources.Strings.global_nominal_current;

        /// <summary>
        /// Default Current magnitude to use through out the test
        /// </summary>
        public string NominalCurrent { get; set; } = "0.000";

        /// <summary>
        /// Hint for the Nominal Frequency text entry
        /// </summary>
        public string NominalFrequencyHint { get; set; } = Resources.Strings.global_nominal_frequency;

        /// <summary>
        /// Default Frequency magnitude to use through out the test
        /// </summary>
        public string NominalFrequency { get; set; } = "60.000";

        /// <summary>
        /// Group box title for Voltage Phase options
        /// </summary>
        public string VoltagePhaseHeaderTitle { get; set; } = Resources.Strings.global_nominal_phase_voltage;

        /// <summary>
        /// Content of the Nominal Voltage Phase 0°
        /// </summary>
        public string VoltagePhaseOptionZero { get; set; } = Resources.Strings.global_nominal_phase_zero;

        /// <summary>
        /// Content of the Nominal Voltage Phase Balanced
        /// </summary>
        public string VoltagePhaseOptionBalanced { get; set; } = Resources.Strings.global_nominal_phase_balance;

        /// <summary>
        /// Group box title for Current Phase options
        /// </summary>
        public string CurrentPhaseHeaderTitle { get; set; } = Resources.Strings.global_nominal_phase_current;

        /// <summary>
        /// Content of the Nominal Current Phase 0°
        /// </summary>
        public string CurrentPhaseOptionZero { get; set; } = Resources.Strings.global_nominal_phase_zero;

        /// <summary>
        /// Content of the Nominal Current Phase Balanced
        /// </summary>
        public string CurrentPhaseOptionBalanced { get; set; } = Resources.Strings.global_nominal_phase_balance;

        /// <summary>
        /// Default Voltage phase to use through out the test
        /// </summary>
        public string SelectedVoltagePhase { get; set; } = "AllZero";

        /// <summary>
        /// Default Current phase to use through out the test
        /// </summary>
        public string SelectedCurrentPhase { get; set; } = "AllZero";

        /// <summary>
        /// Group box title for Test Values
        /// </summary>
        public string TestValuesHeaderTitle { get; set; } = Resources.Strings.global_nominal_test_values;

        /// <summary>
        /// Content of the Nominal Delta
        /// </summary>
        public string NominalDeltaHint { get; set; } = Resources.Strings.global_nominal_delta;

        /// <summary>
        /// Default Delta value to use through out the test
        /// Delta == magnitude difference between test steps
        /// </summary>
        public string NominalDelta { get; set; } = "0.000";

        /// <summary>
        /// indicates if the current text double left clicked to highlight the text
        /// </summary>
        public bool Selected { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// The command handles radio button selections
        /// </summary>
        public ICommand RadioButtonCommand { get; set; }

        /// <summary>
        /// Selects all text on left clicked text box.
        /// </summary>
        public ICommand SelectAllTextCommand { get; set; }

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

            // create commands
            RadioButtonCommand = new RelayParameterizedCommand((parameter) => GetSelectedRadioButton((string)parameter));

            SelectAllTextCommand = new RelayCommand(SelectAll);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Selects all text on the text box
        /// </summary>
        public void SelectAll()
        {
            // simulate property change briefly to select all text in the text box
            // as selecting all text should be last until the user leaves the control or types something

            // Sets FocusAndSelectProperty to true
            Selected = true;

            // Sets FocusAndSelectProperty to false
            Selected = false;
        }

        /// <summary>
        /// Shows test steps with values reset to nominal values
        /// </summary>
        public void CopyNominalValues()
        {
            try
            {
                // change CancelForegroundColor to Red
                IoC.Commands.CancelForegroundColor = "ff0000";

                // set visibility of Command buttons
                IoC.Commands.NewTestAvailable = true;

                // generate AnalogSignals from nominal values.
                ObservableCollection<AnalogSignalListItemViewModel> analogSignals = new ObservableCollection<AnalogSignalListItemViewModel>();


                // TODO: these values should receive from associated Omicron test set
                // Voltage Amplifier number
                int omicronVoltageSignalNumber = 4;

                // Current Amplifier number
                int omicronCurrentSignalNumber = 6;

                // total of current and voltage Analog Signals of associated Omicron Test set
                int omicronAnalogSignalNumber = omicronVoltageSignalNumber + omicronCurrentSignalNumber;

                // generate AnalogSignalListItems
                for (int i = 1; i <= omicronAnalogSignalNumber; i++)
                {

                    // exclude "V4", "I4", "I5", and "I6" signals until further notice
                    if (i == 4 || i > 7)
                        // continue next iteration
                        continue;

                    // Generate AnalogSignals values.
                    analogSignals.Add(new AnalogSignalListItemViewModel
                    {
                        // is this condition true ? yes : no

                        // current signals names restart at 1 => (i - omicronVoltageSignalNumber)

                        // Omicron Analog Signal Name
                        SignalName = i <= omicronVoltageSignalNumber ? "v" + i : "i" + (i - omicronVoltageSignalNumber),
                        // Omicron Analog Signal Magnitude
                        Magnitude = i <= omicronVoltageSignalNumber ? $"{Convert.ToDouble(NominalVoltage):F3}" : $"{Convert.ToDouble(NominalCurrent):F3}",
                        // Omicron Analog Signal Magnitude From value
                        From = i <= omicronVoltageSignalNumber ? $"{Convert.ToDouble(NominalVoltage):F3}" : $"{Convert.ToDouble(NominalCurrent):F3}",
                        // Omicron Analog Signal Magnitude To value
                        To = i <= omicronVoltageSignalNumber ? $"{Convert.ToDouble(NominalVoltage):F3}" : $"{Convert.ToDouble(NominalCurrent):F3}",
                        // Omicron Analog Signal Magnitude Delta value
                        Delta = $"{Convert.ToDouble(NominalDelta):F3}",
                        // Omicron Analog Signal Phase
                        Phase = i <= omicronVoltageSignalNumber ? SelectedPhaseToString(SelectedVoltagePhase, (i - 1)) : SelectedPhaseToString(SelectedCurrentPhase, (i - 2)),
                        // Omicron Analog Signal Frequency
                        Frequency = $"{Convert.ToDouble(NominalFrequency):F3}"
                    });
                }

                // Update only AnalogSignal values in the single instance of TestDetailsViewModel
                IoC.TestDetails.AnalogSignals = analogSignals;

                // Show TestDetails page
                IoC.Application.GoToPage(ApplicationPage.TestDetails, IoC.TestDetails);
            }
            catch (Exception ex)
            {
                // log the error
                // IoC.Logger.Log($"Exception: {ex.Message}");

                // inform the user
                IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Exception: {ex.Message}.\n";
            }

        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Converts RadioButton selection into phase information
        /// </summary>
        /// <param name="phase">Selected radio button parameter</param>
        /// <param name="signalNumber">the signal number to process</param>
        /// <returns>String that represent user selected phase information per analog signal</returns>
        private string SelectedPhaseToString(string phase, int signalNumber)
        {
            // Return 0.00 if the user selected phase is 0°
            if (phase == "AllZero")
                return $"{Convert.ToDouble(0):F2}";

            // formula used: 2𝜋−(2𝑥𝜋/3) => 120*(9-x)
            // 𝜋 = 180°
            // x = 1 to 10
            double result = 120.0 * (9 - signalNumber) % 360.0;

            // if the result phase larger than 180° show it as negative phase
            // else show the phase as is
            return $"{(result > 180.0 ? result - 360.0 : result):F2}";
        }

        /// <summary>
        /// The command handles radio button selection events
        /// </summary>
        private void GetSelectedRadioButton(string param)
        {
            // Signal type: Voltage or Current
            string type = param.Split('.')[0];

            // Is phase balanced or all zero?
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
                    // Something wrong
                    Debugger.Break();
                    break;
            }
        }

        #endregion
    }
}
