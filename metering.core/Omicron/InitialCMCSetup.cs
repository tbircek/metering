using System;
using System.Diagnostics;
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

        private void SendOmicronCommand (string CommandToSend)
        {
            // send commands to Omicron Test Set
            IoC.StringCommands.SendStringCommand(omicronCommand: CommandToSend);
        }

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

                //// set voltage amplifiers default values.
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:1", nominalVoltage, phase, nominalFrequency);
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:2", nominalVoltage, phase, nominalFrequency);
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.v, "1:3", nominalVoltage, phase, nominalFrequency);

                //// set current amplifiers default values.
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:1", nominalCurrent, phase, nominalFrequency);
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:2", nominalCurrent, phase, nominalFrequency);
                //omicron.SendOutAna(CMEngine, DeviceID, (int)StringCommands.GeneratorList.i, "1:3", nominalCurrent, phase, nominalFrequency);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"initial setup::Exception InnerException is : {ex.Message}");
                IoC.Communication.Log += $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tinitial setup::Exception InnerException is : {ex.Message}\n";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the user about more details about error.
                    IoC.Communication.Log += $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.\n";
                }
            }
        }

    }
}
