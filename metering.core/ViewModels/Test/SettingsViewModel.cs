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
        /// Gets Wiring diagram file location for both voltage and current amplifier
        /// </summary>
        /// <returns></returns>
        public async Task GetWiringDiagram(object parameter)
        {
            // update the log
            await IoC.Task.Run(() => IoC.Logger.Log($"{nameof(GetWiringDiagram)} started."));
            await IoC.Task.Run(() => IoC.Logger.Log($"Group name: {((SettingsListItemViewModel)parameter).GroupName} selected."));
            // retrieve configuration identification(s)
            foreach (var item in ((SettingsListItemViewModel)parameter).ConfigIDs)
            {
                // update the log
                await IoC.Task.Run(() => IoC.Logger.Log($"Configuration id: {item} selected."));
            }
            // update the log
            await IoC.Task.Run(() => IoC.Logger.Log($"file location: {((SettingsListItemViewModel)parameter).WiringDiagramFileLocation} selected."));
            // update the log
            await IoC.Task.Run(() => IoC.Logger.Log($"mode: {((SettingsListItemViewModel)parameter).Mode} selected."));
            // retrieve phase count(s)
            foreach (var item in ((SettingsListItemViewModel)parameter).PhaseCounts)
            {
                // update the log
                await IoC.Task.Run(() => IoC.Logger.Log($"phase count: {item} selected."));
            }
            // update the log
            await IoC.Task.Run(() => IoC.Logger.Log($"raw response: {((SettingsListItemViewModel)parameter).RawOmicronResponse} selected."));
            // retrieve amplifier number(s) 
            foreach (var item in ((SettingsListItemViewModel)parameter).AmplifierNumber)
            {
                // update the log
                await IoC.Task.Run(() => IoC.Logger.Log($"amplifier descriptor: {item} selected."));
            }

            // set wiring diagrams per group names.
            switch (((SettingsListItemViewModel)parameter).GroupName.ToUpper())
            {
                // signal == "Voltage"
                case "V":
                    // set wiring diagram image file location
                    VoltageDiagramLocation = ((SettingsListItemViewModel)parameter).WiringDiagramFileLocation;
                    // assign Selected Voltage Amplifier Hardware Configuration.
                    IoC.TestDetails.SelectedVoltageConfiguration = (SettingsListItemViewModel)parameter;
                    break;
                // signal == "Current"
                case "A":
                    // set wiring diagram image file location
                    CurrentDiagramLocation = ((SettingsListItemViewModel)parameter).WiringDiagramFileLocation;
                    // assign Selected Current Amplifier Hardware Configuration.
                    IoC.TestDetails.SelectedCurrentConfiguration = (SettingsListItemViewModel)parameter;
                    break;
                // should never come here.
                default:
                    // update the log
                    IoC.Logger.Log($"Omicron amplifier {((SettingsListItemViewModel)parameter).WiringDiagramFileLocation}is not supported");
                    break;
            }
            // update the log
            await IoC.Task.Run(() => IoC.Logger.Log($"{nameof(GetWiringDiagram)} ended."));
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

                    // update log file about the connected Omicron capabilities.
                    await IoC.Task.Run(() => IoC.Logger.Log($"Following hardware configurations available:"));

                    // retrieve voltage capabilities.
                    IoC.Settings.OmicronVoltageOutputs = await IoC.Configurations.Get("voltage");
                    // retrieve current capabilities.
                    IoC.Settings.OmicronCurrentOutputs = await IoC.Configurations.Get("current");

                    // set visibility of "Cancel tests" button
                    IoC.Commands.Cancellation = true;

                    // Show TestDetails page
                    IoC.Application.GoToPage(ApplicationPage.Settings, IoC.Settings);

                    // disconnect from attached Omicron Test Set
                    await IoC.Task.Run(() => IoC.ReleaseOmicron.Release());
                }
            });
        }
        #endregion

        #region Private Methods

        private async Task<bool> SetOmicronHardwareConfiguration(SettingsViewModel setting)
        {
            return true;
        }
        #endregion
    }
}
