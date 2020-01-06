using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="generatorType">Triple list type: "v" for Voltage, "i" for current amplifier.</param>
        /// <param name="tripletNumber">This parameter is 1 or 2 and selects either signal component 1 or component 2. Ex: "1:1".</param>
        /// <param name="amplitude">Magnitude of analog signal.</param>
        /// <param name="phase">Phase of analog signal.</param>
        /// <param name="frequency">Frequency of analog signal.</param>
        /// <example>Output string ex: out:ana:v(1:1):a(120);p(0);f(60);wav(sin)</example>
        public async void SendOutAnaAsync(char generatorType, string tripletNumber, double amplitude, double phase, double frequency)
        {
            try
            {
                // lock the task
                await AsyncAwaiter.AwaitAsync(nameof(SendOutAnaAsync), async () =>
                {
                    // check if the user canceling test
                    if (!IoC.Commands.Token.IsCancellationRequested)
                    {
                        // build a string to send to Omicron Test set
                        StringBuilder stringBuilder = new StringBuilder($"out:ana:{generatorType}({tripletNumber}):{nameof(SignalType.a)}({amplitude});{nameof(SignalType.p)}({phase});{nameof(SignalType.f)}({frequency});wav(sin)");

                        // inform developer about string command send to omicron test set
                        IoC.Logger.Log($"device ID: {IoC.CMCControl.DeviceID}\tcommand: {stringBuilder}", LogLevel.Informative);

                        // send newly generated string command to Omicron Test Set
                        await IoC.Task.Run(() =>
                        {
                            try
                            {
                                // execute the user values.
                                IoC.CMCControl.CMEngine.Exec(
                                   DevID: IoC.CMCControl.DeviceID,
                                   Command: stringBuilder.ToString());
                            }
                            catch (COMException ex)
                            {
                                // inform the developer about error.
                                IoC.Logger.Log($"Exception: {ex.Message}\nPlease try to re-start the program.");
                            }
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                // inform the developer about error.
                IoC.Logger.Log($"Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends string command to Omicron Test Set.
        /// </summary>
        /// <param name="omicronCommand">This is the command to send Omicron Test Set.</param>
        public async void SendStringCommandAsync(string omicronCommand)
        {
            try
            {
                // lock the task
                await AsyncAwaiter.AwaitAsync(nameof(SendStringCommandAsync), async () =>
                {

                    // check if the user canceling test
                    if (!IoC.Commands.Token.IsCancellationRequested)
                    {
                        // pass received string command to Omicron Test set
                        await IoC.Task.Run(() =>
                        {
                            try
                            {

                                // record configuration string command
                                IoC.Logger.Log($"Command: {omicronCommand}");

                                // send string command
                                IoC.CMCControl.CMEngine.Exec(IoC.CMCControl.DeviceID, omicronCommand);
                            }
                            catch (COMException ex)
                            {
                                // inform the developer about error.
                                IoC.Logger.Log($"Exception: {ex.Message}\nPlease try to re-start the program.");
                            }
                        });
                    }
                });
            }
            catch (Exception err)
            {
                // inform the developer about error.
                IoC.Logger.Log($"Exception: {err.Message}");
            }
        }

        /// <summary>
        /// Sends string command to Omicron Test Set.
        /// </summary>
        /// <param name="omicronCommand">This is the command to send Omicron Test Set.</param>
        /// <returns>Returns Omicron Test Set response to the most recent executed command.</returns>
        public async Task<string> SendStringCommandWithResponseAsync(string omicronCommand)
        {
            try
            {
                // lock the task
                return await AsyncAwaiter.AwaitResultAsync(nameof(SendStringCommandWithResponseAsync), async () =>
                {
                    // set default response
                    string response = string.Empty;

                    // check if the user canceling test
                    if (!IoC.Commands.Token.IsCancellationRequested)
                    {
                        // pass received string command to Omicron Test set
                        response = await IoC.Task.Run(() => IoC.CMCControl.CMEngine.Exec(IoC.CMCControl.DeviceID, omicronCommand));
                    }

                    // return Omicron Test Set response
                    return response;
                });
            }
            catch (Exception err)
            {
                // inform the developer about error.
                IoC.Logger.Log($"Exception: {err.Message}");

                // return an empty string.
                return string.Empty;
            }
        }
    }
}