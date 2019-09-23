using System;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// Handles Omicron Power up, down and etc. options
    /// </summary>
    public class PowerOptions
    {

        /// <summary>
        /// Turns off outputs of Omicron Test Set and release it.
        /// </summary>
        public void TurnOffCMC()
        {
            try
            {
                
                // send Turn off command to Omicron Test Set
                IoC.StringCommands.SendStringCommand(OmicronStringCmd.out_analog_outputOff);

                // update the developer
                IoC.Logger.Log($"turnOffCMC setup: started", LogLevel.Informative);

                // release Omicron Test Set.
                IoC.CMCControl.ReleaseOmicron();
            }
            catch (Exception ex)
            {
                // inform the developer about error.
                IoC.Logger.Log($"InnerException: {ex.Message}");

                // inform the user about error.
                IoC.Communication.Log += $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\tturnOffCMC setup: error detected\n";

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
