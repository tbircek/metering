
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
        /// Reads holding register(s)
        /// </summary>
        /// <returns>Returns holding register value(s)</returns>
        private async Task<int[]> ReadHoldingRegisterWithCancellationAsync(ushort register, CancellationToken cancellationToken)
        {
            // generate a TaskCompletionSource.
            var taskCompletionSource = new TaskCompletionSource<int[]>();

            // Registering a lambda into the cancellationToken
            cancellationToken.Register(() =>
            {
                // received a cancellation message, cancel the TaskCompletionSource.Task
                taskCompletionSource.TrySetCanceled();
            });

            // generate ReadHoldingRegisters Task
            var task = IoC.Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(register - 1, 1));

            // Wait for the task to finish.
            Task<int[]> completedTask = await Task.WhenAny(task, taskCompletionSource.Task);

            // return completed task.
            return await completedTask;
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

            // inquire each register that specified by the user.
            Parallel.ForEach(rangePartitioner, parallelingOptions, async (registerString) =>
            {

                using (CancellationTokenSource cancellation = new CancellationTokenSource(new TimeSpan(0, 0, 0, Convert.ToInt32(IoC.TestDetails.MeasurementInterval))))
                {
                    // Listening to the cancellation event either the user or test completed.
                    var cancellationTask = Task.Run(() =>
                    {
                        if (IoC.Commands.Token.IsCancellationRequested)
                        {
                            // Sending the cancellation message
                            cancellation.Cancel();
                        }
                    });

                    try
                    {
                        // Loop over each range element without a delegate invocation.
                        for (int i = registerString.Item1; i < registerString.Item2; i++)
                        {

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
                                        int[] serverResponse = await IoC.Task.Run(() => ReadHoldingRegisterWithCancellationAsync(register: register, cancellationToken: cancellation.Token));

                                        // decide if serverResponse is acceptable only criteria is the length of the response.
                                        if (serverResponse.Length > 0)
                                        {
                                            // server response must be between 0 and 65535
                                            if (ushort.MinValue <= Math.Abs(serverResponse[0]) && ushort.MaxValue >= Math.Abs(serverResponse[0]))
                                            {                                                
                                                // add new reading with the current time.
                                                IoC.CMCControl.RegisterReadingsWithTime[i].Add(DateTime.Now, serverResponse[0]);
                                            }
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
                            });

                        }
                    }
                    catch (Exception)
                    {
                        // re-throw error
                        throw;
                    }

                    // cancellation.Dispose();
                    await cancellationTask;
                }
            });
        }

        #endregion
    }

}
