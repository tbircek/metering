using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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
        public string VoltageDiagramLocation { get; set; } = "../Images/Omicron/not used voltage.png";

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
        public string CurrentDiagramLocation { get; set; } = "../Images/Omicron/not used current.png";

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
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task GetWiringDiagram(object parameter)
        {
            // let log start
            await IoC.Task.Run(() => IoC.Logger.Log($"{nameof(GetWiringDiagram)} started."));

            // let log start
            await IoC.Task.Run(() => IoC.Logger.Log($"Group name: {((SettingsListItemViewModel)parameter).GroupName} selected."));
            await IoC.Task.Run(() => IoC.Logger.Log($"Configuration id: {((SettingsListItemViewModel)parameter).ConfigID} selected."));
            await IoC.Task.Run(() => IoC.Logger.Log($"is checked? {((SettingsListItemViewModel)parameter).CurrentWiringDiagram} selected."));
            await IoC.Task.Run(() => IoC.Logger.Log($"file location: {((SettingsListItemViewModel)parameter).WiringDiagramFileLocation} selected."));
            await IoC.Task.Run(() => IoC.Logger.Log($"mode: {((SettingsListItemViewModel)parameter).Mode} selected."));

            switch (((SettingsListItemViewModel)parameter).GroupName.ToUpper())
            {
                case "V":
                    // set wiring diagram image file location
                    VoltageDiagramLocation = ((SettingsListItemViewModel)parameter).WiringDiagramFileLocation;
                    break;
                case "A":
                    // set wiring diagram image file location
                    CurrentDiagramLocation = ((SettingsListItemViewModel)parameter).WiringDiagramFileLocation;
                    break;
                default:
                    break;
            }
        }

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

                    // disconnect from attached Omicron Test Set
                    await IoC.Task.Run(() => IoC.ReleaseOmicron.Release());
                }
            });
        }
        #endregion

        #region Private Methods
               
        /// <summary>
        /// Gets every available hardware configuration from attached Omicron test set.
        /// </summary>
        /// <param name="amplifierType">2 types available. "voltage" or "current"</param>
        /// <returns>Returns a <see cref="SettingsListItemViewModel"/></returns>
        private async Task<ObservableCollection<SettingsListItemViewModel>> GetOmicronHardwareConfigurations(string amplifierType)
        {
            // initialize extract parameters function
            ExtractParameters extract = new ExtractParameters();

            // storage for available Omicron Hardware Configurations
            ObservableCollection<SettingsListItemViewModel> outputConfigurations = new ObservableCollection<SettingsListItemViewModel>();

            // storage for available Omicron Hardware Configuration to combine to maximize output
            ObservableCollection<SettingsListItemViewModel> outputConfigurationsToCombine = new ObservableCollection<SettingsListItemViewModel>();

            // returns a string that contains the CMC's test set number and the number of available configurations of type<integer>.
            string configurationInformation = await IoC.Task.Run(() => IoC.StringCommands.SendStringCommandWithResponseAsync(omicronCommand: string.Format(OmicronStringCmd.amp_cfg)).Result);

            // retrieve the number of available configurations.
            int totalConfiguration = Convert.ToInt16(extract.Parameters(2, configurationInformation).Replace(oldChar: ';', newChar: ' '));

            // delimiters to split Omicron responses
            string[] delimiterStrings = { ";", "," };
            // delimiter to count amplifiers
            string[] ampDelimiter = { "amp_no" };

            // retrieve all available configurations in reverse order to generate combination configurations.
            for (int i = totalConfiguration; i >= 1; i--)
            {
                // Voltage response
                // 1,11,3,3.000000e+02,5.000000e+01,7.500000e+01,6.600000e-01,zero,13,amp_no,1,amp_no,5;
                // Current response
                // 1,16,3,1.250000e+01,7.000000e+01,7.500000e+00,1.000000e+01,zero,40,amp_no,2,amp_no,6;

                // storage for available Omicron Hardware Configuration
                SettingsListItemViewModel outputConfiguration = new SettingsListItemViewModel();

                // two options available. either voltage or current.
                string amplifierInitial = string.Empty;
                // if amplifierInitial == "V" than "A", if amplifierInitial == "A" than "V"
                string outputType = string.Empty;

                // retrieve Omicron Hardware Configuration Details
                string configurationDetails = (await IoC.Task.Run(() => IoC.StringCommands.SendStringCommandWithResponseAsync(omicronCommand: string.Format(OmicronStringCmd.amp_cfg_0, i)).Result));

                // count amplifiers in configuration details to generate combined hardware configurations
                // starts with 2
                int amplifierNumber = configurationDetails.Split(separator: ampDelimiter, options: StringSplitOptions.RemoveEmptyEntries).Length - 1;
                IoC.Logger.Log($"Total Omicron amplifier: {amplifierNumber}");

                // split up the omicron response.
                string[] responses = configurationDetails.Split(separator: delimiterStrings, options: StringSplitOptions.RemoveEmptyEntries);

                switch (amplifierType)
                {
                    case "voltage":
                        // pick correct the amplifier type.
                        if (!Equals("1", responses.GetValue(responses.Length - 1)) && !Equals("5", responses.GetValue(responses.Length - 1)))
                        {
                            // not a voltage amplifier
                            continue;
                        }
                        // amplifier type is voltage
                        amplifierInitial = "V";
                        // output type is A
                        outputType = "A";
                        break;
                    case "current":
                        // pick correct the amplifier type.
                        if (!Equals("2", responses.GetValue(responses.Length - 1)) && !Equals("6", responses.GetValue(responses.Length - 1)))
                        {
                            // not a current amplifier
                            continue;
                        }
                        // amplifier type is voltage
                        amplifierInitial = "A";
                        // output type is V
                        outputType = "V";
                        break;
                    default:
                        IoC.Logger.Log($"Omicron amplifier {responses.GetValue(responses.Length - 1)}is not supported");
                        continue;
                }

                // add configuration id
                outputConfiguration.ConfigID = responses[1];
                // add phase count
                outputConfiguration.PhaseCount = Convert.ToInt16(responses[2]);
                // retrieve mode
                outputConfiguration.Mode = $"{responses[7]}";
                // retrieve wiring id.
                outputConfiguration.WiringID = Convert.ToInt16(responses[8]);
                // add file name
                outputConfiguration.WiringDiagramFileLocation = $"{outputConfiguration.Mode}{outputConfiguration.WiringID}";

                // is output configuration has an automatically calculated resultant -- String == "zero"?
                string autamaticallyCalculated = string.Empty;

                // only mode == zero where v4 automatically calculated
                if (Equals(responses[7], "zero"))
                {
                    // two options Voltage is "V" and Current is "I"
                    autamaticallyCalculated = $", {(amplifierInitial == "A" ? "I" : amplifierInitial)}E automatically calculated";
                }

                // outputConfiguration.
                // 3x300V, 
                string magnitudeString = $"{responses[2]}x{Convert.ToDouble(responses[3], CultureInfo.CurrentCulture)}{amplifierInitial}, ";
                // 85VA @ 85V,
                string vaString = $"{Convert.ToDouble(responses[4], CultureInfo.CurrentCulture)}VA @ {Convert.ToDouble(responses[5], CultureInfo.CurrentCulture)}{amplifierInitial}, ";
                // 3x300V, 85VA @ 85V, 1Arms
                outputConfiguration.WiringDiagramString = $"{magnitudeString}{vaString}{Convert.ToDouble(responses[6], CultureInfo.CurrentCulture)}{outputType}rms{autamaticallyCalculated}";
                // add group name for the radio buttons
                outputConfiguration.GroupName = amplifierInitial;

                // construct the string.
                await IoC.Task.Run(() => IoC.Logger.Log(outputConfiguration.WiringDiagramString));

                // construct the view model.
                outputConfigurations.Add(outputConfiguration);

                // check this view model if view model can be combined with previous view model.
                switch (outputConfiguration.Mode)
                {
                    case "std":
                    case "ser12":
                    case "ser13":
                    case "gen2":
                    case "par1":
                    case "par2ser1": // not 100% sure if this mode can be combine.
                        // only single amplifiers can be combined
                        if (1 == amplifierNumber)
                        {
                            // add single amplifier to next single amplifier
                            outputConfigurationsToCombine.Add(outputConfiguration);
                            if (2 == outputConfigurationsToCombine.Count)
                            {
                                // the new phase count supported by the combined amplifiers.
                                int phaseCount = outputConfigurationsToCombine[0].PhaseCount + outputConfigurationsToCombine[1].PhaseCount;

                                // add new combination configuration to the list.
                                outputConfigurations.Add(new SettingsListItemViewModel()
                                { 
                                    // the new configuration id for the combined amplifiers
                                    // most likely this number is not supported natively by omicron
                                    ConfigID = $"{outputConfigurationsToCombine[1].ConfigID},{outputConfigurationsToCombine[0].ConfigID}",
                                    // phase count of the combination of the amplifiers
                                    PhaseCount = phaseCount,
                                    // wire diagram file location to show to the user
                                    WiringDiagramFileLocation = $"{outputConfigurationsToCombine[1].Mode}{outputConfigurationsToCombine[1].WiringID}{outputConfigurationsToCombine[0].Mode}{outputConfigurationsToCombine[0].WiringID}",
                                    // construct string to show to the user.
                                    WiringDiagramString = $"{phaseCount}x{Convert.ToDouble(responses[3], CultureInfo.CurrentCulture)}{amplifierInitial}, {vaString}{Convert.ToDouble(responses[6], CultureInfo.CurrentCulture)}{outputType}rms",
                                    // add group name for the radio buttons
                                    GroupName = amplifierInitial,
                                    // add the mode since both amplifiers has same mode just use the last ones
                                    Mode = outputConfigurationsToCombine[1].Mode,
                                });
                            }
                        }
                        break;
                    default:
                        // reset combination list.
                        outputConfigurationsToCombine.Clear();
                        break;
                }
            }
            // return the view model in reverse order to match Omicron Hardware Configuration interface.
            return new ObservableCollection<SettingsListItemViewModel>(outputConfigurations.Reverse());
        }

        private async Task<bool> SetOmicronHardwareConfiguration(SettingsViewModel setting)
        {
            return true;
        }
        #endregion
    }
}
