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

        /// <summary>
        /// Default value of Frequency amplifiers while testing non-frequency values and
        /// must be a non-zero value.
        /// </summary>
        private const double NominalFrequency = 60.0d;

        #endregion

        /// <summary>
        /// Omicron Test Set generator short signal names.
        /// </summary>
        public enum SignalType : short { a, f, p };

        /// <summary>
        /// Sends "ON" command to Omicron Test Set.
        /// </summary>
        /// <remarks>
        /// A harmonic is defined by the three parameters <harmonicX>,<amplitude_factorX>
        /// and<phaseX>.They define the order of the harmonic, its amplitude relative to the
        /// setting of the a() command and its phase relative to the fundamental.For the
        /// definition of every harmonic you must define its three parameters<harmonicX>,
        /// <amplitude_factorX> and<phaseX>.
        /// </remarks>
        /// <param name="generatorType">Triple list type: "v" for Voltage, "i" for current amplifier.</param>
        /// <param name="tripletNumber">This parameter is 1 or 2 and selects either signal component 1 or component 2. Ex: "1:1".</param>
        /// <param name="amplitude">Magnitude of analog signal.</param>
        /// <param name="phase">Phase of analog signal.</param>
        /// <param name="frequency">Frequency of analog signal.</param>
        /// <param name="amplitude_factor">Harmonics ramping only. Sets the amplitude of the fundamental relative to the setting of the a() command.</param>
        /// <param name="harmonicX">Harmonics ramping only. Defines the order of a harmonic. e.g 2 = 2nd harmonic</param>
        /// <param name="amplitudeFactorX">Harmonics ramping only. The amplitude set with the a() command is multiplied with <amplitude_factorX> to
        /// obtain the amplitude of the fundamental or harmonic respectively.</param>
        /// <param name="phaseX">Harmonics ramping only. Defines the phase of the harmonic.</param>
        /// <example>
        /// Output strings:
        /// ex 1: out:ana:v(1:1):a(120);p(0);f(60);wav(sin)
        /// ex 2: out:ana:v(1:1):a(120);p(0);f(60);wav(sum,1,2,0.10,0) --> 2nd harmonics @ 10%
        /// ex 3: out:ana:v(1:1):a(120);p(0);f(60);wav(sum,1,2,0.10,0,3,0.05,0,4,0.20,0) --> 2nd @ 10%, 3rd @ 5%, 4th @ 20%
        /// </example>
        public void SendAnalogOutputString(char generatorType, string tripletNumber, double amplitude, double phase, double frequency, double amplitude_factor, int harmonicX, double amplitudeFactorX, double phaseX)
        {
            try
            {
                // check if the user canceling test
                if (!IoC.Commands.Token.IsCancellationRequested)
                {
                    // default waveform to use is "sin"
                    string waveForm = "sin";
                    // only time this changes while the ramping signal is "Harmonics"
                    if (IoC.TestDetails.IsHarmonics)
                    {
                        // use waveform == "sum" 
                        // e.g. sum,1,2,0.10,0
                        waveForm = $"sum,{amplitude_factor},{harmonicX},{amplitudeFactorX / 100.0d:F6},{phaseX:F6}";

                    }

                    // is frequency zero? yes == use NominalFrequency no == frequency
                    double frequencyToApply = frequency.Equals(0) ? NominalFrequency : frequency;

                    // build a string to send to Omicron Test set
                    StringBuilder stringBuilder = new StringBuilder($"out:ana:{generatorType}({tripletNumber}):{nameof(SignalType.a)}({amplitude});{nameof(SignalType.p)}({phase});{nameof(SignalType.f)}({frequencyToApply});wav({waveForm})");

                    //// update the log
                    //IoC.Logger.Log($"device id: {IoC.CMCControl.DeviceID} -- command: {stringBuilder}", LogLevel.Informative);

                    // send newly generated string command to Omicron Test Set
                    IoC.Task.Run(async () => await SendStringCommandsAsync(omicronCommand: stringBuilder.ToString()));
                }
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
        /// <returns>a string that contains Omicron test response</returns>
        public async Task<string> SendStringCommandsAsync(string omicronCommand)
        {
            try
            {

                // update the log
                IoC.Logger.Log($"device id: {IoC.CMCControl.DeviceID} -- command: {omicronCommand}", LogLevel.Informative);

                // execute the user values.
                Task<string> result = IoC.Task.Run(() =>
                {
                    //try
                    //{
                        // send command string.
                        return IoC.CMCControl.CMEngine.Exec(DevID: IoC.CMCControl.DeviceID, Command: omicronCommand);

                    //}
                    //// Re-throw all exceptions.
                    //catch (Exception)
                    //{
                    //    throw;
                    //}
                }, IoC.Commands.Token);

                // wait for the result
                return await result;
            }
            catch (COMException ex)
            {
                // inform the developer about error.
                IoC.Logger.Log($"Exception: {ex.Message}\nPlease try to re-start the program.");

                // update Current Test File
                IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);

                // return an empty string.
                return string.Empty;
            }
            catch (OperationCanceledException ex)
            {
                if (!IoC.Commands.TokenSource.IsCancellationRequested)
                {
                    // inform the developer about error.
                    IoC.Logger.Log($"Exception: {ex.Message}\nPlease try to re-start the program.");

                    // update Current Test File
                    IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);

                }

                // return an empty string.
                return string.Empty;
            }
            catch (Exception err)
            {
                if (!IoC.Commands.TokenSource.IsCancellationRequested)
                {
                    // inform the developer about error.
                    IoC.Logger.Log($"Exception: {err.Message}");

                    // update Current Test File
                    IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);

                }

                // return an empty string.
                return string.Empty;
            }
        }
    }
}