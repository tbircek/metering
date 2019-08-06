using System;
using System.Diagnostics;
using System.Text;
using metering.Resources;
using OMICRON.CMEngAL;

namespace metering
{
    public class StringCommands
    {
        // ExtractParameters result = new ExtractParameters();

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
        /// <param name="engine">Omicron CM Engine to use.</param>
        /// <param name="deviceID">Associated Omicron Test Set ID. Assigned by CM Engine.</param>
        /// <param name="generator">Triple list type: "v" for Voltage, "i" for current amplifier.</param>
        /// <param name="generatorNumber">This parameter is 1 or 2 and selects either signal component 1 or component 2. Ex: "1:1".</param>
        /// <param name="amplitude">Magnitude of analog signal.</param>
        /// <param name="phase">Phase of analog signal.</param>
        /// <param name="frequency">Frequency of analog signal.</param>
        public void SendOutAna(CMEngine engine, int deviceID, int generator, string generatorNumber, double amplitude, double phase, double frequency)
        {
            string generatorType = (string)Enum.GetName(typeof(GeneratorList), generator);

            try
            {
                StringBuilder stringBuilder = new StringBuilder(string.Format(OmicronStringCmd.out_analog_setOutput, generatorType, generatorNumber, amplitude, phase, frequency));
                //result.Parameters(engine.Exec(deviceID, stringBuilder.ToString()), stringBuilder.ToString());
                engine.Exec(deviceID, stringBuilder.ToString());
            }
            catch (Exception err)
            {
                Debug.WriteLine(string.Format("sendOutAna_2::InnerException is : {0}", err.Message));
            }
        }

        /// <summary>
        /// Sends string command to Omicron Test Set.
        /// </summary>
        /// <param name="engine">Omicron CM Engine to use.</param>
        /// <param name="deviceID">Associated Omicron Test Set ID. Assigned by CM Engine.</param>
        /// <param name="omicronCommand">This is the command to send Omicron Test Set.</param>
        public void SendStringCommand(CMEngine engine, int deviceID, string omicronCommand)
        {
            try
            {
                //result.Parameters(engine.Exec(deviceID, omicronCommand), omicronCommand);
                engine.Exec(deviceID, omicronCommand);
            }
            catch (Exception err)
            {
                Debug.WriteLine(String.Format("sendStringCommand::InnerException is : {0}", err.Message));
            }
        }
    }
}