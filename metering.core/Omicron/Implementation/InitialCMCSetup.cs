using System;
using System.Threading.Tasks;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// Initializes Omicron Test Set control engine
    /// </summary>
    public class InitialCMCSetup
    {
        #region Private Properties

        #endregion

        #region Private Variables

        #endregion

        #region Private Methods

        /// <summary>
        /// Sends Omicron commands to CMEngine
        /// </summary>
        /// <param name="CommandToSend">Omicron String Command</param>
        private Task SendOmicronCommand(string CommandToSend)
        {
            // send commands to Omicron Test Set
            return IoC.Task.Run(async () => await IoC.StringCommands.SendStringCommandsAsync(omicronCommand: CommandToSend));

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets Omicron Test Set default values and limits.
        /// </summary>
        [Obsolete("Please use Async version", true)]
        public Task InitialSetup()
        {
            try
            {
                // Switches OFF all generators of a CMC immediately. 
                SendOmicronCommand(OmicronStringCmd.out_ana_off);

                // Clears the routings of all triples and the amplifier’s power supply that is turned off (clr) or not (clrnooff).
                SendOmicronCommand(OmicronStringCmd.amp_route_clr);

                // command to undefined an amplifier.. 
                SendOmicronCommand(OmicronStringCmd.amp_def_clr);

                // initiate max values
                double MaxVoltageMagnitude = default;
                double MaxCurrentMagnitude = default;

                // Route Omicron amplifiers.
                // voltage amplifiers
                foreach (var config in IoC.TestDetails.SelectedVoltageConfiguration.ConfigIDs)
                {
                    // current hardware configuration location in the collection
                    int hardwareConfigurationPosition = IoC.TestDetails.SelectedVoltageConfiguration.ConfigIDs.IndexOf(config);

                    // current hardware configuration value in the collection
                    int amp_no = Convert.ToInt32(IoC.TestDetails.SelectedVoltageConfiguration.AmplifierNumber[hardwareConfigurationPosition]);

                    // route the amplifier to the user specification
                    // amp_no == 1 is 1st,  amp_no == 5 is 2nd voltage amplifier per Omicron Documentation
                    SendOmicronCommand(string.Format(OmicronStringCmd.amp_route_v_0_1, 5 == amp_no ? 2 : amp_no, config));

                    // reset Max output value
                    MaxVoltageMagnitude = IoC.TestDetails.SelectedVoltageConfiguration.MaxOutput[hardwareConfigurationPosition];

                    // update Omicron voltage amplifier ranges.
                    SendOmicronCommand(string.Format(OmicronStringCmd.amp_range_v_0_1, 5 == amp_no ? 2 : amp_no, MaxVoltageMagnitude));

                }

                // current amplifiers
                foreach (var config in IoC.TestDetails.SelectedCurrentConfiguration.ConfigIDs)
                {
                    // current hardware configuration location in the collection
                    int hardwareConfigurationPosition = IoC.TestDetails.SelectedCurrentConfiguration.ConfigIDs.IndexOf(config);

                    // current hardware configuration value in the collection
                    int amp_no = Convert.ToInt32(IoC.TestDetails.SelectedCurrentConfiguration.AmplifierNumber[hardwareConfigurationPosition]);

                    // route the amplifier to the user specification
                    // amp_no == 2 is 1st,  amp_no == 6 is 2nd current amplifier per Omicron Documentation
                    SendOmicronCommand(string.Format(OmicronStringCmd.amp_route_i_0_1, 6 == amp_no ? 2 : 1, config));

                    // reset Max output value
                    MaxCurrentMagnitude = IoC.TestDetails.SelectedCurrentConfiguration.MaxOutput[hardwareConfigurationPosition];

                    // update Omicron current amplifier ranges.
                    SendOmicronCommand(string.Format(OmicronStringCmd.amp_range_i_0_1, 6 == amp_no ? 2 : 1, MaxCurrentMagnitude));

                }

                // update the user about the Omicron Test Set voltage connection scheme.
                // 3x600V, 70VA @ 7.5V 10Arms
                IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff} Voltage Configuration: {(string.IsNullOrWhiteSpace(IoC.TestDetails.SelectedVoltageConfiguration.WiringDiagramString) ? "Not used." : IoC.TestDetails.SelectedVoltageConfiguration.WiringDiagramString)}";

                // update the user about the Omicron Test Set voltage connection scheme.
                //  Selecting CConfig, 3x12.5A, 140VA @ 15V, 10Arms
                IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff} Current Configuration: {(string.IsNullOrWhiteSpace(IoC.TestDetails.SelectedCurrentConfiguration.WiringDiagramString) ? "Not used." : IoC.TestDetails.SelectedCurrentConfiguration.WiringDiagramString)}";

                // change power mode.
                SendOmicronCommand(string.Format(OmicronStringCmd.out_ana_pmode_0, "timeabs"));

                // return Task Completed
                return Task.CompletedTask;

            }
            catch (Exception ex)
            {
                // update the log
                IoC.Logger.Log($"InnerException: {ex.Message}");
                IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tinitial setup::Exception InnerException is : {ex.Message}.";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the user about more details about error.
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.";
                }

                // update Current Test File
                IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);

                // return Task Completed
                return Task.CompletedTask;
            }
        }

        /// <summary>
        /// Sets Omicron Test Set default values and limits.
        /// </summary>
        public async Task InitialSetupAsync()
        {
            try
            {
                // Switches OFF all generators of a CMC immediately. 
                await IoC.Task.Run(() => SendOmicronCommand(OmicronStringCmd.out_ana_off));

                // Clears the routings of all triples and the amplifier’s power supply that is turned off (clr) or not (clrnooff).
                await IoC.Task.Run(() => SendOmicronCommand(OmicronStringCmd.amp_route_clr));

                // command to undefined an amplifier.. 
                await IoC.Task.Run(() => SendOmicronCommand(OmicronStringCmd.amp_def_clr));

                // initiate max values
                double MaxVoltageMagnitude = default;
                double MaxCurrentMagnitude = default;

                // Route Omicron amplifiers.
                // voltage amplifiers
                foreach (var config in IoC.TestDetails.SelectedVoltageConfiguration.ConfigIDs)
                {
                    // current hardware configuration location in the collection
                    int hardwareConfigurationPosition = await IoC.Task.Run(() => IoC.TestDetails.SelectedVoltageConfiguration.ConfigIDs.IndexOf(config));

                    // current hardware configuration value in the collection
                    int amp_no = await IoC.Task.Run(() => Convert.ToInt32(IoC.TestDetails.SelectedVoltageConfiguration.AmplifierNumber[hardwareConfigurationPosition]));

                    // route the amplifier to the user specification
                    // amp_no == 1 is 1st,  amp_no == 5 is 2nd voltage amplifier per Omicron Documentation
                    await IoC.Task.Run(() => SendOmicronCommand(string.Format(OmicronStringCmd.amp_route_v_0_1, 5 == amp_no ? 2 : amp_no, config)));

                    // reset Max output value
                    MaxVoltageMagnitude = await IoC.Task.Run(() => IoC.TestDetails.SelectedVoltageConfiguration.MaxOutput[hardwareConfigurationPosition]);

                    // update Omicron voltage amplifier ranges.
                    await IoC.Task.Run(() => SendOmicronCommand(string.Format(OmicronStringCmd.amp_range_v_0_1, 5 == amp_no ? 2 : amp_no, MaxVoltageMagnitude)));

                }

                // current amplifiers
                foreach (var config in IoC.TestDetails.SelectedCurrentConfiguration.ConfigIDs)
                {
                    // current hardware configuration location in the collection
                    int hardwareConfigurationPosition = await IoC.Task.Run(() => IoC.TestDetails.SelectedCurrentConfiguration.ConfigIDs.IndexOf(config));

                    // current hardware configuration value in the collection
                    int amp_no = await IoC.Task.Run(() => Convert.ToInt32(IoC.TestDetails.SelectedCurrentConfiguration.AmplifierNumber[hardwareConfigurationPosition]));

                    // route the amplifier to the user specification
                    // amp_no == 2 is 1st,  amp_no == 6 is 2nd current amplifier per Omicron Documentation
                    await IoC.Task.Run(() => SendOmicronCommand(string.Format(OmicronStringCmd.amp_route_i_0_1, 6 == amp_no ? 2 : 1, config)));

                    // reset Max output value
                    MaxCurrentMagnitude = await IoC.Task.Run(() => IoC.TestDetails.SelectedCurrentConfiguration.MaxOutput[hardwareConfigurationPosition]);

                    // update Omicron current amplifier ranges.
                    await IoC.Task.Run(() => SendOmicronCommand(string.Format(OmicronStringCmd.amp_range_i_0_1, 6 == amp_no ? 2 : 1, MaxCurrentMagnitude)));

                }

                // update the user about the Omicron Test Set voltage connection scheme.
                // 3x600V, 70VA @ 7.5V 10Arms
                IoC.Communication.Log = await IoC.Task.Run(() => $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff} Voltage Configuration: {(string.IsNullOrWhiteSpace(IoC.TestDetails.SelectedVoltageConfiguration.WiringDiagramString) ? "Not used." : IoC.TestDetails.SelectedVoltageConfiguration.WiringDiagramString)}");

                // update the user about the Omicron Test Set voltage connection scheme.
                //  Selecting CConfig, 3x12.5A, 140VA @ 15V, 10Arms
                IoC.Communication.Log = await IoC.Task.Run(() => $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff} Current Configuration: {(string.IsNullOrWhiteSpace(IoC.TestDetails.SelectedCurrentConfiguration.WiringDiagramString) ? "Not used." : IoC.TestDetails.SelectedCurrentConfiguration.WiringDiagramString)}");

                // change power mode.
                await IoC.Task.Run(() => SendOmicronCommand(string.Format(OmicronStringCmd.out_ana_pmode_0, "timeabs")));

            }
            catch (Exception)
            {
                //// update the log
                //IoC.Logger.Log($"InnerException: {ex.Message}");
                //IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tinitial setup::Exception InnerException is : {ex.Message}.";

                //// catch inner exceptions if exists
                //if (ex.InnerException != null)
                //{
                //    // inform the user about more details about error.
                //    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.";
                //}

                //// update Current Test File
                //IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);   

                // Trying to stop the app gracefully.
                await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrorsAsync(false));
            }
        }
    }

    #endregion
}
