using System;
using System.Diagnostics;
using System.Text;
using metering.core.Resources;
using OMICRON.CMEngAL;

namespace metering.core
{
    /// <summary>
    /// Generates necessary strings to control Omicron Test Set.
    /// </summary>
    public class StringCommands
    {        
        #region Private Members

        #endregion

        /// <summary>
        /// Omicron Test Set generator short names.
        /// </summary>
        public enum GeneratorList : short { v, i };

        /// <summary>
        /// Omicron Test Set generator short signal names.
        /// </summary>
        public enum SignalType : short { a, f, p };

        /// <summary>
        /// Sends "ON" command to Omicron Test Set.
        /// </summary>
        /// <remarks>
        /// Command format:
        /// "out:[ana:]v|i(generator_list):[sig(no):]signalType(fValue) with omitted optional "step" parameter.
        /// </remarks>
        /// <param name="generator">Triple list type: "v" for Voltage, "i" for current amplifier.</param>
        /// <param name="generatorNumber">This parameter is 1 or 2 and selects either signal component 1 or component 2. Ex: "1:1".</param>
        /// <param name="amplitude">Magnitude of analog signal.</param>
        /// <param name="phase">Phase of analog signal.</param>
        /// <param name="frequency">Frequency of analog signal.</param>
        public void SendOutAna(int generator, string generatorNumber, double amplitude, double phase, double frequency)
        {
            // obtain appropriate generator short name 
            string generatorType = Enum.GetName(typeof(GeneratorList), generator);

            try
            {
                // check if the user canceling test
                if (!IoC.Commands.Token.IsCancellationRequested)
                {
                    // build a string to send to Omicron Test set
                    StringBuilder stringBuilder = new StringBuilder(string.Format(OmicronStringCmd.out_analog_setOutput, generatorType, generatorNumber, amplitude, phase, frequency));

                    // send newly generated string command to Omicron Test Set
                    IoC.CMCControl.CMEngine.Exec(IoC.CMCControl.DeviceID, stringBuilder.ToString());

                    // inform developer about string command send to omicron test set
                    Debug.WriteLine($"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: device ID: {IoC.CMCControl.DeviceID}\tcommand: {stringBuilder}");
                }
            }
            catch (Exception err)
            {
                // inform the developer about error.
                Debug.WriteLine($"sendOutAna::Exception is : {err.Message}");
            }
        }

        /// <summary>
        /// Sends string command to Omicron Test Set.
        /// </summary>
        /// <param name="omicronCommand">This is the command to send Omicron Test Set.</param>
        public void SendStringCommand(string omicronCommand)
        {
            try
            {
                // check if the user canceling test
                if(!IoC.Commands.Token.IsCancellationRequested)
                {
                    // pass received string command to Omicron Test set
                    IoC.CMCControl.CMEngine.Exec(IoC.CMCControl.DeviceID, omicronCommand);
                }
            }
            catch (Exception err)
            {
                // inform the developer about error.
                Debug.WriteLine($"sendStringCommand::Exception is : {err.Message}");
            }
        }
    }
}