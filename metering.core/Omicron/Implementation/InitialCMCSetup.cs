using System;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// Initializes Omicron Test Set control engine
    /// </summary>
    public class InitialCMCSetup
    {

        #region Private Variables

        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private const double maxVoltageMagnitude = 8.0f;

        /// <summary>
        /// Omicron Test Set maximum voltage output limit.
        /// </summary>
        private const double maxCurrentMagnitude = 2.0f;

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets Omicron Test Set default values and limits.
        /// </summary>
        public void InitialSetup()
        {
            try
            {

                // initialize Omicron amplifier route
                SendOmicronCommand(OmicronStringCmd.amp_route_init);

                // initialize Omicron amplifier definitions
                SendOmicronCommand(OmicronStringCmd.amp_def_init);

                // route Omicron voltage amplifiers
                SendOmicronCommand(OmicronStringCmd.amp_route_voltage);

                // route Omicron current amplifiers
                SendOmicronCommand(OmicronStringCmd.amp_route_current);

                // update Omicron voltage amplifier ranges.
                SendOmicronCommand(string.Format(OmicronStringCmd.amp_range_voltage, maxVoltageMagnitude));

                // update Omicron current amplifier ranges.
                SendOmicronCommand(string.Format(OmicronStringCmd.amp_range_current, maxCurrentMagnitude));

                // change power mode.
                SendOmicronCommand(OmicronStringCmd.out_analog_pmode);

                // TODO: need to communicate attached Omicron earlier to obtain more information about it.
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
