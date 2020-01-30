
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// Provides asynchronous and multiple register reading capabilities to the application.
    /// </summary>
    public class ModbusCommunication
    {

        #region Private Methods
        
        /// <summary>
        /// Function to return a Tuple that holds <see cref="MinTestValue"/> and <see cref="MaxTestValue"/>
        /// </summary>
        /// <returns>Returns a Tuple with ramping signal properties</returns>       
        private (int MaxResponse, int MinResponse, double CurrentAverage, int Counter) GetServerResponseAsync(int serverResponse, int Index)
        {
            try
            {
                // load Tuple variables with default or previously stored values
                int MinResponse = (int)IoC.CMCControl.MinValues.GetValue(Index);
                int MaxResponse = (int)IoC.CMCControl.MaxValues.GetValue(Index);
                double CurrentAverage = (double)IoC.CMCControl.Averages.GetValue(Index);
                int Counter = (int)IoC.CMCControl.SuccessCounters.GetValue(Index);

                // server response must be between 0 and 65535
                if (ushort.MinValue <= Math.Abs(serverResponse) && ushort.MaxValue >= Math.Abs(serverResponse))
                {
                    // increment the counter
                    Interlocked.Increment(ref Counter);

                    // retrieve totals for the register
                    int totals = (int)IoC.CMCControl.Totals.GetValue(Index);

                    // update minimum value with new min value or not
                    MinResponse = Math.Min(MinResponse, serverResponse);

                    // update maximum value with new max value or not
                    MaxResponse = Math.Max(MaxResponse, serverResponse);

                    // Calculate server response average
                    CurrentAverage = (serverResponse + (double)totals) / Counter;

                    // update totals
                    IoC.CMCControl.Totals.SetValue(totals + serverResponse, Index);
                }

                //// inform the developer.
                //IoC.Logger.Log($"{nameof(GetServerResponseAsync)} Reads: {serverResponse} -- MinResponse: {MinResponse} -- MaxResponse: {MaxResponse} --- AveResponse: {AveResponse} --- SuccessfulResponse: {SuccessfulResponse}");

                // return server response
                return (MaxResponse, MinResponse, CurrentAverage, Counter);
            }
            catch (Exception ex)
            {
                // inform the developer about error
                IoC.Logger.Log($"Exception is : {ex.Message}");

                // update the user about the error.
                IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

                // return no server response
                return (default, default, default, default);
            }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Allows to read test register values separated with comma specified by "Measurement Interval".
        /// </summary>
        public void MeasurementIntervalCallback(object Register)
        {
            // generate register(s) list
            List<string> registerStrings = new List<string>();

            // add Register to the list
            registerStrings.AddRange(Register.ToString().Split(','));

            // Use ParallelOptions instance to store the CancellationToken
            ParallelOptions parallelingOptions = new ParallelOptions
            {
                // associate cancellation token.
                CancellationToken = IoC.Commands.Token,
                // set limit for the parallelism equivalent of hardware core count
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            // Partition the entire source array.
            var rangePartitioner = Partitioner.Create(0, registerStrings.Count);

            try
            {

                // inquire each register that specified by the user.
                Parallel.ForEach(rangePartitioner, parallelingOptions, async (registerString) =>
                {

                    try
                    {

                        // Was cancellation already requested?
                        if (IoC.Commands.Token.IsCancellationRequested)
                            // throw if the cancellation requested.
                            IoC.Commands.Token.ThrowIfCancellationRequested();

                        // Loop over each range element without a delegate invocation.
                        for (int i = registerString.Item1; i < registerString.Item2; i++)
                        {
                            // establish a new cancellation source with a timeout to automatically terminate communication threads
                            // CancellationTokenSource cancellation = new CancellationTokenSource(Convert.ToInt32(IoC.TestDetails.MeasurementInterval) * 2);
                            CancellationTokenSource cancellation = new CancellationTokenSource(new TimeSpan(0, 0, 0, Convert.ToInt32(IoC.TestDetails.MeasurementInterval)));

                            // start a task to read register address specified by the user.
                            await IoC.Task.Run(async () =>
                                    {

                                    // is the register is between modbus holding register range?
                                    if (ushort.TryParse(registerStrings[i].Trim(), out ushort register))
                                        {

                                        // lock the task
                                        await AsyncAwaiter.AwaitAsync(nameof(MeasurementIntervalCallback), async () =>
                                                {
                                                // start a task to read holding register (Function 0x03)
                                                int[] serverResponse = await IoC.Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(register - 1, 1), cancellation.Token);

                                                // decide if serverResponse is acceptable only criteria is the length of the response.
                                                if (serverResponse.Length > 0)
                                                    {

                                                    // save server response information as a Tuple
                                                    (int MaxRegisterValue, int MinRegisterValue, double AverageRegisterValue, int SuccessCounter) = GetServerResponseAsync(serverResponse[0], i);

                                                    // assign MaxTestValue
                                                    IoC.CMCControl.MaxValues.SetValue(MaxRegisterValue, i);

                                                    // assign MinTestValue
                                                    IoC.CMCControl.MinValues.SetValue(MinRegisterValue, i);

                                                    // assign AverageRegisterValue
                                                    IoC.CMCControl.Averages.SetValue(AverageRegisterValue, i);

                                                    // update counter.
                                                    IoC.CMCControl.SuccessCounters.SetValue(SuccessCounter, i);

                                                    //// inform the developer about register reading
                                                    //IoC.Logger.Log($"register: {register} \tminResponse : {MinRegisterValue} \tmaxResponse : {MaxRegisterValue} \taverage: {AverageRegisterValue} \tsuccess: {SuccessCounter}");
                                                }
                                                    else
                                                    {
                                                    // server failed to respond. Ignoring it until find a better option.
                                                    // inform the developer about error
                                                    IoC.Logger.Log($"register: {register} -- serverResponse : No server response");

                                                    }
                                                });
                                        }
                                        else
                                        {
                                        // illegal register address
                                        throw new ArgumentOutOfRangeException($"Register: {registerStrings[i].Trim()} is out of range");

                                        }
                                    // }, cancellation.Token);
                                });

                            cancellation.Dispose();
                        }

                        // throw if the cancellation requested.
                        IoC.Commands.Token.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException ex)
                    {
                        // inform the developer about error
                        IoC.Logger.Log($"OperationCanceledException is : {ex.Message}");

                        // update the user about the error.
                        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Operation canceled: {ex.Message}.";

                        // Trying to stop the app gracefully.
                        await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrorsAsync());
                    }
                    catch (System.IO.IOException ex)
                    {
                        // inform the developer about error
                        IoC.Logger.Log($"IOException is : {ex.Message}");

                        // update the user about the error.
                        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

                        // Trying to stop the app gracefully.
                        await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrorsAsync());
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
                        await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrorsAsync());
                    }

                });
            }
            catch (OperationCanceledException ex)
            {
                // inform the developer about error
                IoC.Logger.Log($"OperationCanceledException is : {ex.Message}");

                // update the user about the error.
                IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Operation canceled: {ex.Message}.";

                // Trying to stop the app gracefully.
                IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrorsAsync());
            }
        }

        #endregion
    }

}
