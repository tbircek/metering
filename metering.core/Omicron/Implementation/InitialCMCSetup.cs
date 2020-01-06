using System;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// Initializes Omicron Test Set control engine
    /// </summary>
    public class InitialCMCSetup
    {
        #region Private Properties

        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private double MaxVoltageMagnitude { get; set; } = 300.0f;

        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private double MaxCurrentMagnitude { get; set; } = 12.50f;

        #endregion

        #region Private Variables

        /// <summary>
        /// Default value of Voltage amplifiers while testing non-voltage values.
        /// </summary>
        const double nominalVoltage = 120.0f;

        /// <summary>
        /// Default value of Current amplifiers while testing non-current values.
        /// </summary>
        const double nominalCurrent = 0.02f;

        /// <summary>
        /// Default value of amplifiers phase while testing non-phase values.
        /// </summary>
        const double phase = 0.0f;

        /// <summary>
        /// Default value of Frequency amplifiers while testing non-frequency values and
        /// must be a non-zero value.
        /// </summary>
        const double nominalFrequency = 60.0f;

        #endregion

        #region Private Methods

        /// <summary>
        /// Sends Omicron commands to CMEngine
        /// </summary>
        /// <param name="CommandToSend">Omicron String Command</param>
        private void SendOmicronCommand(string CommandToSend)
        {
            // send commands to Omicron Test Set
            IoC.StringCommands.SendStringCommandAsync(omicronCommand: CommandToSend);
        }

        private string SendOmicronCommandWithResponse(string CommandToSend)
        {
            return IoC.StringCommands.SendStringCommandWithResponseAsync(omicronCommand: CommandToSend).Result;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets Omicron Test Set default values and limits.
        /// </summary>
        public void InitialSetup()
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

                    // record configuration string command
                    IoC.Logger.Log($"Configuring amp_no {amp_no}: {string.Format(OmicronStringCmd.amp_route_v_0_1, 5 == amp_no ? 2 : amp_no, Convert.ToInt32(config))}");
                    
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
                IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff} Voltage Configuration: {(string.IsNullOrWhiteSpace(IoC.TestDetails.SelectedVoltageConfiguration.WiringDiagramString) ? "Not used.": IoC.TestDetails.SelectedVoltageConfiguration.WiringDiagramString)}";
              
                // update the user about the Omicron Test Set voltage connection scheme.
                //  Selecting CConfig, 3x12.5A, 140VA @ 15V, 10Arms
                IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff} Current Configuration: {(string.IsNullOrWhiteSpace(IoC.TestDetails.SelectedCurrentConfiguration.WiringDiagramString) ? "Not used." : IoC.TestDetails.SelectedCurrentConfiguration.WiringDiagramString)}";

                // change power mode.
                SendOmicronCommand(string.Format(OmicronStringCmd.out_ana_pmode_0, "timeabs"));
            }
            catch (Exception ex)
            {
                IoC.Logger.Log($"InnerException: {ex.Message}");
                IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tinitial setup::Exception InnerException is : {ex.Message}.";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the user about more details about error.
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.";
                }
            }
        }

        #endregion
    }
}
