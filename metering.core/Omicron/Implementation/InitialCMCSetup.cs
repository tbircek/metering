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

            // initialize extract parameters function
            ExtractParameters extract = new ExtractParameters();

            try
            {
                // Switches OFF all generators of a CMC immediately. 
                SendOmicronCommand(OmicronStringCmd.out_ana_off);

                // Clears the routings of all triples and the amplifier’s power supply that is turned off (clr) or not (clrnooff).
                SendOmicronCommand(OmicronStringCmd.amp_route_clr);

                // command to undefine an amplifier.. 
                SendOmicronCommand(OmicronStringCmd.amp_def_clr);

                // route Omicron voltage amplifiers
                // Selecting CConfig, 3x300V, 85VA@85V, wiring: 0, CMC256plus A == 9
                // TODO: Implement a method to allow the user change the hardware configurations.
                int hardwareConfiguration = 9;
                SendOmicronCommand(string.Format(OmicronStringCmd.amp_route_v_0_1, 1, hardwareConfiguration));

                // record response to for future use
                // "1,int,v,A,3.000000e+02,3.000000e+02,0.000000e+00,1.000000e+03,0.000000e+00,0.000000e+00,0.000000e+00,CMC256plus,KH186P;"
                string definition = SendOmicronCommandWithResponse(string.Format(OmicronStringCmd.amp_def_param_0_1, 1, "int"));

                // record response to for future use
                // example: "1,17;"
                string configurationNumber = SendOmicronCommandWithResponse(OmicronStringCmd.amp_cfg);

                // ex: "1,17,3,2.500000e+01,1.400000e+02,1.500000e+01,1.000000e+01,par3,5,amp_no,2,amp_no,6;"
                string configurationInformation = SendOmicronCommandWithResponse(string.Format(OmicronStringCmd.amp_cfg_0, 1));
                
                // prepare Voltage Amplifier hardware configuration
                string value = extract.Parameters(5, definition);
                string typeOfAmplifier = extract.Parameters(3, definition);
                int multiplier = int.Parse(extract.Parameters(3, configurationInformation));
                double maxValue = double.Parse(extract.Parameters(4, configurationInformation));
                double va = double.Parse(extract.Parameters(5, configurationInformation));
                double voltage = double.Parse(extract.Parameters(6, configurationInformation));
                double current = double.Parse(extract.Parameters(7, configurationInformation));

                MaxVoltageMagnitude = double.Parse(value);

                // update the user about the Omicron Test Set voltage connection scheme.
                // 3x600V, 70VA @ 7.5V 10Arms
                IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff} Selecting CConfig, {multiplier}x{double.Parse(value)}{("v" == typeOfAmplifier ? typeOfAmplifier.ToUpper() : "A")}, {va}VA @ {voltage}V, {current}Arms";

                // route Omicron current amplifiers
                // Selecting CConfig, 3x25A, 140VA@15A, wiring: 5, CMC256plus A, CMC256plus B == 17
                // Selecting CConfig, 3x12.5A, 70VA@7.5A, wiring: 5, CMC256plus A, CMC256plus B == 14
                // TODO: Implement a method to allow the user change the hardware configurations.
                hardwareConfiguration = 17;
                SendOmicronCommand(string.Format(OmicronStringCmd.amp_route_i_0_1, 1, hardwareConfiguration));

                // record response to for future use
                // "1,int,i,A,1.250000e+01,1.250000e+01,0.000000e+00,1.000000e+03,0.000000e+00,0.000000e+00,0.000000e+00,CMC256plus,LG472W;"
                definition = SendOmicronCommandWithResponse(string.Format(OmicronStringCmd.amp_def_param_0_1, 2, "int"));

                //  1,17,3,2.500000e+01,1.400000e+02,1.500000e+01,1.000000e+01,par3,5,amp_no,2,amp_no,6; == amp:cfg?(9)
                configurationInformation = SendOmicronCommandWithResponse(string.Format(OmicronStringCmd.amp_cfg_0, 9));

                // prepare Voltage Amplifier hardware configuration
                value = extract.Parameters(5, definition);
                typeOfAmplifier = extract.Parameters(3, definition);
                multiplier = int.Parse(extract.Parameters(3, configurationInformation));
                maxValue = double.Parse(extract.Parameters(4, configurationInformation));
                va = double.Parse(extract.Parameters(5, configurationInformation));
                current = double.Parse(extract.Parameters(6, configurationInformation));
                voltage = double.Parse(extract.Parameters(7, configurationInformation));
                string mode = extract.Parameters(8, configurationInformation);

                int wiringIDMultiplier = "par3" == mode ? 2 : 1;

                MaxCurrentMagnitude = double.Parse(value);

                // update the user about the Omicron Test Set voltage connection scheme.
                //  Selecting CConfig, 3x12.5A, 140VA @ 15V, 10Arms
                IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff} Selecting CConfig, {multiplier}x{double.Parse(value)* wiringIDMultiplier}{("v" == typeOfAmplifier ? typeOfAmplifier.ToUpper() : "A")}, {va}VA @ {current}A, {voltage}Vrms";

                // update Omicron voltage amplifier ranges.
                SendOmicronCommand(string.Format(OmicronStringCmd.amp_range_v_0_1, 1, MaxVoltageMagnitude));

                // update Omicron current amplifier ranges.
                SendOmicronCommand(string.Format(OmicronStringCmd.amp_range_i_0_1, 1, MaxCurrentMagnitude));

                // change power mode.
                SendOmicronCommand(string.Format(OmicronStringCmd.out_ana_pmode_0, "timeabs"));

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
