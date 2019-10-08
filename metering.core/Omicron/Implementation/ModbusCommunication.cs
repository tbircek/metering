
using System;
using System.Collections.Generic;

namespace metering.core
{
    /// <summary>
    /// Provides asynchronous and multiple register reading capabilities to the application.
    /// </summary>
    public class ModbusCommunication
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModbusCommunication()
        {

        }

        #endregion
        
        /// <summary>
        /// Allows to read test register values separated with comma specified by "Measurement Interval".
        /// </summary>
        public async void MeasurementIntervalWithCommaCallbackAsync(object Register)
        {
            // read Modbus register in interval that specified by the user 
            try
            {

                // if a cancellation requested stop reading register
                if (IoC.Commands.Token.IsCancellationRequested)
                    return;
                
                // initialize a default register value
                ushort register = default(ushort);

                // generate register(s) list
                List<string> registerStrings = new List<string>();

                // add Register to the list
                registerStrings.AddRange(Register.ToString().Split(','));

                // inquire each register that specified by the user.
                foreach (var registerString in registerStrings)
                {
                    // is the register is between modbus holding register range?
                    if (ushort.TryParse(registerString.Trim(), out register))
                    {

                        int Index = registerStrings.IndexOf(registerString);

                        // start a task to read register address specified by the user.
                        await IoC.Task.Run(async () =>
                        {
                            // start a task to read holding register (Function 0x03)
                            int[] serverResponse = await IoC.Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(register - 1, 1), IoC.Commands.Token);

                            // decide if serverResponse is acceptable only criteria is the length of the response.
                            if (serverResponse.Length > 0)
                            {

                                // save server response information as a Tuple
                                var (MaxRegisterValue, MinRegisterValue) = GetServerResponseAsync(serverResponse[0], Index);


                                // assign MaxTestValue
                                IoC.CMCControl.MaxValues.SetValue(MaxRegisterValue, Index);

                                // assign MinTestValue
                                IoC.CMCControl.MinValues.SetValue(MinRegisterValue, Index);
                                                                
                                // inform the developer about error
                                IoC.Logger.Log($"register: {register} -- serverResponse : {serverResponse[0]} -- MinResponse: {MinRegisterValue} -- MaxResponse: {MaxRegisterValue}");

                            }
                            else
                            {
                                // server failed to respond. Ignoring it until find a better option.
                                // inform the developer about error
                                IoC.Logger.Log($"register: {register} -- serverResponse : No server response");

                            }

                        }, IoC.Commands.Token);
                    }
                    else
                    {
                        // illegal register address
                        throw new ArgumentOutOfRangeException($"Register: {registerString} is out of range");
                    }


                }
            }
            catch (Exception ex)
            {
                // inform the developer about error
                IoC.Logger.Log($"Exception is : {ex.Message}");

                // update the user about the error.
                IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the developer about error
                    IoC.Logger.Log($"InnerException is : {ex.Message}");

                    // update the user.
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.";
                }

                // Trying to stop the app gracefully.
                await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrors());
            }
        }

        /// <summary>
        /// Function to return a Tuple that holds <see cref="MinTestValue"/> and <see cref="MaxTestValue"/>
        /// </summary>
        /// <returns>Returns a Tuple with ramping signal properties</returns>       
        private (int MaxResponse, int MinResponse) GetServerResponseAsync(int serverResponse, int Index)
        {
            try
            {
                // initialize Tuple variables with default values
                int MinResponse = (int)IoC.CMCControl.MinValues.GetValue(Index);
                int MaxResponse = (int)IoC.CMCControl.MaxValues.GetValue(Index);

                //if new value is less or minimum value was 0
                // if ( Math.Min(MinResponse, serverResponse)) //  MinResponse > serverResponse || (int)IoC.CMCControl.MinValues.GetValue(Index) == 0)
                // update minimum value with new value 
                MinResponse = Math.Min(MinResponse, serverResponse); // serverResponse;

                // if new value is less or maximum value was 0
                // if (MaxResponse < serverResponse || (int)IoC.CMCControl.MaxValues.GetValue(Index) == 0)
                // update maximum value with new value 
                MaxResponse = Math.Max(MaxResponse, serverResponse); // serverResponse;

                // return no server response
                return (MaxResponse, MinResponse);
            }
            catch (Exception ex)
            {
                // inform the developer about error
                IoC.Logger.Log($"Exception is : {ex.Message}");

                // update the user about the error.
                IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

                // return no server response
                return (default(int), default(int));
            }
        }

    }

}
