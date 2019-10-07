
using System;
using System.Collections.Generic;

namespace metering.core
{
    /// <summary>
    ///
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

                List<Tuple<int>> registerList = new List<Tuple<int>>();


                int register = default(int);

                // does Register object contains any commas?
                IoC.Logger.Log($"Number of commas : {Register.ToString().IndexOf(',')}");

                // does Register object contains any commas?
                if (Register.ToString().IndexOf(',') > -1)
                {
                    string[] registers = Register.ToString().Split(',');

                    foreach (var item in registers)
                    {
                        // yes. create a Tuple List
                        registerList.Add(new Tuple<int>(Convert.ToInt16(item)));
                    }

                }
                // no
                else
                {
                    // convert register string to integer.
                    register = Convert.ToInt32(Register);
                }


                // verify the register is a legit
                if (register >= 0 && register <= 65535)
                {

                    // start a task to read register address specified by the user.
                    await IoC.Task.Run(async () =>
                    {
                        // start a task to read holding register (Function 0x03)
                        int[] serverResponse = await IoC.Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(register - 1, 1), IoC.Commands.Token);

                        // decide if serverResponse is acceptable only criteria is the length of the response.
                        if (serverResponse.Length > 0)
                        {
                            // establish minimum and maximum values.
                            for (int i = 0; i < serverResponse.Length; i++)
                            {
                                // update minimum value with new value if new value is less or minimum value was 0
                                if (IoC.CMCControl.MinTestValue > serverResponse[i] || IoC.CMCControl.MinTestValue == 0)
                                {
                                    // update minimum value
                                    IoC.CMCControl.MinTestValue = serverResponse[i];
                                }

                                // update maximum value with new value if new value is less or maximum value was 0
                                if (IoC.CMCControl.MaxTestValue < serverResponse[i] || IoC.CMCControl.MaxTestValue == 0)
                                {
                                    // update maximum value
                                    IoC.CMCControl.MaxTestValue = serverResponse[i];
                                }
                            }
                        }
                        else
                        {
                            //TODO: server failed to respond. Ignoring it until find a better option.
                        }

                    }, IoC.Commands.Token);
                }
                else
                {
                    // illegal register address
                    throw new ArgumentOutOfRangeException($"Register: {register} is out of range");

                    // await IoC.Task.Run(() => ProcessErrors(false));
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
        public (int MinResponse, int MaxResponse) GetServerResponseAsync(object Registers)
        {
            try
            {
                // initialize Tuple variables with default values
                int MinResponse = default(int);
                int MaxResponse = default(int);

                // send query for each register specified by the user.
                foreach (AnalogSignalListItemViewModel signal in IoC.TestDetails.AnalogSignals)
                {
                    // scan TestDetailsViewModel and return all signal properties where From and To values are not same
                    if (!Convert.ToDouble(signal.From).Equals(Convert.ToDouble(signal.To)))
                    {
                        // server response values
                        MinResponse = default(int);
                        MaxResponse = default(int);

                        // return minimum and maximum values per Register specified.
                        return (MinResponse, MaxResponse);
                    }
                }

                // return no server response
                return (default(int), default(int));
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
