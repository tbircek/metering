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
        /// indicates if the current text double left clicked to highlight the text
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Omicron Analog Output Signals.
        /// </summary>
        public ObservableCollection<SettingsListItemViewModel> OmicronOutputSignals { get; set; }
        
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

                    await IoC.Task.Run(() => IoC.Logger.Log($"{nameof(HardwareConfiguration)} started."));

                    await IoC.Task.Run(() => IoC.Logger.Log($"Following device associated: {IoC.CMCControl.DeviceInfo}."));

                    OldApplicationPage = IoC.Application.CurrentPage;

                    OldViewModel = IoC.Application.CurrentPageViewModel;

                    IoC.Settings.OmicronOutputSignals = await GetOmicronHardwareConfigurations();

                    // Show TestDetails page
                    IoC.Application.GoToPage(ApplicationPage.Settings, IoC.Settings);
                }
            });
        }
        #endregion

        #region Private Methods

        private async Task<ObservableCollection<SettingsListItemViewModel>> GetOmicronHardwareConfigurations()
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

                // storage for available Omicron Hardware Configuration
                SettingsListItemViewModel outputConfiguration = new SettingsListItemViewModel();

                // two options available. either voltage or current.
                string amplifierType = string.Empty;

                // split up the omicron response.
                string[] responses = (await IoC.Task.Run(() => IoC.StringCommands.SendStringCommandWithResponseAsync(omicronCommand: string.Format(OmicronStringCmd.amp_cfg_0, i)).Result)).Split(separator: delimiterStrings, options: StringSplitOptions.RemoveEmptyEntries);

                // decide the amplifier type.
                if (Equals("1", responses.GetValue(responses.Length - 1)) || Equals("5", responses.GetValue(responses.Length - 1)))
                {
                    // amplifier type is voltage
                    amplifierType = "V";
                }
                else
                {
                    // amplifier type is current
                    amplifierType = "A";
                }
                // Current response
                // 1,16,3,1.250000e+01,7.000000e+01,7.500000e+00,1.000000e+01,zero,40,amp_no,2,amp_no,6;

                outputConfiguration.ConfigID = Convert.ToInt16(responses[1]);
                outputConfiguration.Mode = $"{responses[7]}{responses[8]}";

                // outputConfiguration.

                // 3x300V, 
                string magnitudeString = $"{responses[2]}x{Convert.ToDouble(responses[3], CultureInfo.CurrentCulture)}{amplifierType}, ";
                // 85VA @ 85V,
                string vaString = $"{Convert.ToDouble(responses[4], CultureInfo.CurrentCulture)}VA @ {Convert.ToDouble(responses[5], CultureInfo.CurrentCulture)}{amplifierType}, ";
                // 3x300V, 85VA @ 85V, 1Arms
                outputConfiguration.UIString = $"{magnitudeString}{vaString}{Convert.ToDouble(responses[6], CultureInfo.CurrentCulture)}{amplifierType}rms";

                // construct the string.
                await IoC.Task.Run(() => IoC.Logger.Log(outputConfiguration.UIString));

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
