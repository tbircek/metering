using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// a view model for Omicron Hardware Configurations in the SettingsPage
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {

        #region Public Properties

        /// <summary>
        /// Holds previous view model to return once Settings completed.
        /// </summary>
        public BaseViewModel OldViewModel { get; private set; }

        /// <summary>
        /// Holds previous page to return once Settings completed.
        /// </summary>
        public ApplicationPage OldApplicationPage { get; private set; }

        /// <summary>
        /// Holds wiring diagram for the voltage amplifiers.
        /// </summary>
        public string VoltageDiagramLocation { get; private set; } = "../Images/Omicron/6X12.5, 70VA.PNG";

        /// <summary>
        /// Holds voltage wiring diagram header information
        /// </summary>
        public string VoltageHeader
        {
            get
            {
                if (string.IsNullOrWhiteSpace(IoC.CMCControl.DeviceInfo))
                {
                    return $"(??????) {Strings.omicron_config_voltage_header}";
                }
                return $"{IoC.CMCControl.DeviceInfo} {Strings.omicron_config_voltage_header}";
            }            
        }

        /// <summary>
        /// Holds wiring diagram for the current amplifiers.
        /// </summary>
        public string CurrentDiagramLocation { get; private set; } = "../Images/Omicron/6X12.5, 70VA.PNG";

        /// <summary>
        /// Holds voltage wiring diagram header information
        /// </summary>
        public string CurrentHeader
        {
            get
            {
                if (string.IsNullOrWhiteSpace(IoC.CMCControl.DeviceInfo))
                {
                    return $"(??????) {Strings.omicron_config_current_header}";
                }
                return $"{IoC.CMCControl.DeviceInfo} {Strings.omicron_config_current_header}";
            }
        }

        /// <summary>
        /// indicates if the current text double left clicked to highlight the text
        /// </summary>
        public bool Selected { get; set; }

        ///// <summary>
        ///// Omicron Analog Output Signals.
        ///// </summary>
        //public ObservableCollection<SettingsListItemViewModel> OmicronOutputSignals { get; set; }
        
        /// <summary>
        /// Omicron Voltage Output Signals.
        /// </summary>
        public ObservableCollection<SettingsListItemViewModel> OmicronVoltageOutputs { get; set; }

        /// <summary>
        /// Omicron Current Output Signals.
        /// </summary>
        public ObservableCollection<SettingsListItemViewModel> OmicronCurrentOutputs { get; set; }

        #endregion

        #region Public Commands

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public SettingsViewModel()
        {

            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

        }

        #endregion

        #region Public Method

        /// <summary>
        /// Handles Omicron Hardware Configuration Settings
        /// </summary>
        /// <returns>Returns new Hardware Configuration</returns>
        public async Task HardwareConfiguration()
        {
            // there is a test set attached so run specified tests.
            // lock the task
            await AsyncAwaiter.AwaitAsync(nameof(HardwareConfiguration), async () =>
            {
                // find cmc
                if (await IoC.Task.Run(() => IoC.FindCMC.Find()))
                {
                    // let log start
                    await IoC.Task.Run(() => IoC.Logger.Log($"{nameof(HardwareConfiguration)} started."));
                    // update device info
                    await IoC.Task.Run(() => IoC.Logger.Log($"Following device associated: {IoC.CMCControl.DeviceInfo}."));

                    // save current page so we can return to it.
                    OldApplicationPage = IoC.Application.CurrentPage;
                    // save current view model so we can return to it.
                    OldViewModel = IoC.Application.CurrentPageViewModel;

                    // retrieve voltage capabilities.
                    IoC.Settings.OmicronVoltageOutputs = await GetOmicronHardwareConfigurations("voltage");
                    // retrieve current capabilities.
                    IoC.Settings.OmicronCurrentOutputs = await GetOmicronHardwareConfigurations("current");

                    // Show TestDetails page
                    IoC.Application.GoToPage(ApplicationPage.Settings, IoC.Settings);
                }
            });
        }
        #endregion

        #region Private Methods

        private async Task<ObservableCollection<SettingsListItemViewModel>> GetOmicronHardwareConfigurations(string amplifierType)
        {
            // initialize extract parameters function
            ExtractParameters extract = new ExtractParameters();

            // storage for available Omicron Hardware Configurations
            ObservableCollection<SettingsListItemViewModel> outputConfigurations = new ObservableCollection<SettingsListItemViewModel>();

            // returns a string that contains the CMC's test set number and the number of available configurations of type<integer>.
            string configurationInformation = await IoC.Task.Run(() => IoC.StringCommands.SendStringCommandWithResponseAsync(omicronCommand: string.Format(OmicronStringCmd.amp_cfg)).Result);

            // retrieve the number of available configurations.
            int totalConfiguration = Convert.ToInt16(extract.Parameters(2, configurationInformation).Replace(oldChar: ';', newChar: ' '));

            // delimiters to split Omicron responses
            string[] delimiterStrings = { ";", "," };

            // retrieve all available configurations.
            for (int i = 1; i <= totalConfiguration; i++)
            {
                // Voltage response
                // 1,11,3,3.000000e+02,5.000000e+01,7.500000e+01,6.600000e-01,zero,13,amp_no,1,amp_no,5;
                // Current response
                // 1,16,3,1.250000e+01,7.000000e+01,7.500000e+00,1.000000e+01,zero,40,amp_no,2,amp_no,6;

                // storage for available Omicron Hardware Configuration
                SettingsListItemViewModel outputConfiguration = new SettingsListItemViewModel();

                // two options available. either voltage or current.
                string amplifierInitial = string.Empty;

                // split up the omicron response.
                string[] responses = (await IoC.Task.Run(() => IoC.StringCommands.SendStringCommandWithResponseAsync(omicronCommand: string.Format(OmicronStringCmd.amp_cfg_0, i)).Result)).Split(separator: delimiterStrings, options: StringSplitOptions.RemoveEmptyEntries);

                switch (amplifierType)
                {
                    case "voltage":
                        // pick correct the amplifier type.
                        if (!Equals("1", responses.GetValue(responses.Length - 1)) && !Equals("5", responses.GetValue(responses.Length - 1)))
                        {
                            continue;
                        }

                        // amplifier type is voltage
                        amplifierInitial = "V";

                        break;
                    case "current":
                        // pick correct the amplifier type.
                        if (!Equals("2", responses.GetValue(responses.Length - 1)) && !Equals("6", responses.GetValue(responses.Length - 1)))
                        {
                            continue;
                        }

                        // amplifier type is voltage
                        amplifierInitial = "A";
                        break;
                    default:
                        IoC.Logger.Log($"Omicron amplifier {responses.GetValue(responses.Length - 1)}is not supported");
                        continue;
                }
                
                // add configuration id
                outputConfiguration.ConfigID = Convert.ToInt16(responses[1]);
                // add file name
                outputConfiguration.Mode = $"{responses[7]}{responses[8]}";

                // outputConfiguration.

                // 3x300V, 
                string magnitudeString = $"{responses[2]}x{Convert.ToDouble(responses[3], CultureInfo.CurrentCulture)}{amplifierInitial}, ";
                // 85VA @ 85V,
                string vaString = $"{Convert.ToDouble(responses[4], CultureInfo.CurrentCulture)}VA @ {Convert.ToDouble(responses[5], CultureInfo.CurrentCulture)}{amplifierInitial}, ";
                // 3x300V, 85VA @ 85V, 1Arms
                outputConfiguration.WiringDiagramString = $"{magnitudeString}{vaString}{Convert.ToDouble(responses[6], CultureInfo.CurrentCulture)}{amplifierInitial}rms";
                // add group name for the radio buttons
                outputConfiguration.GroupName = amplifierInitial;

                // construct the string.
                await IoC.Task.Run(() => IoC.Logger.Log(outputConfiguration.WiringDiagramString));

                // construct the view model.
                outputConfigurations.Add(outputConfiguration);
            }
            // return the view model.
            return outputConfigurations;
        }

        private async Task<bool> SetOmicronHardwareConfiguration(SettingsViewModel setting)
        {
            return true;
        }
        #endregion
    }
}
