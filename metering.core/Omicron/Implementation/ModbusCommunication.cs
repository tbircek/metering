
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        public void MeasurementIntervalCallback_New(object Register)
        {
            if (string.Equals(Register.ToString().Trim(), string.Empty))
            {
                return;
            }

            // inform the developer
            IoC.Logger.Log($"Task {nameof(MeasurementIntervalCallback_New)} starts.");

            // generate register(s) list
            List<string> registerStrings = new List<string>();

            // add Register to the list
            registerStrings.AddRange(Register.ToString().Split(',').AsParallel());

            if (registerStrings.Count < 0)
                return;

            try
            {
                DoMoreWork(registerStrings, (f) =>
                {

                    Console.WriteLine("I am here ------------------------------->");

                    //int[] serverResponse = default;


                    // Exceptions are no-ops.
                    try
                    {
                        // Do nothing with the data except read it.
                        // serverResponse = await IoC.Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(f.Result - 1, 1));

                        // generate a token with timeout of MeasurementInterval * 2
                        CancellationTokenSource tokenSource = new CancellationTokenSource(Convert.ToInt32(IoC.TestDetails.MeasurementInterval) * 2);

                        // Start communication task with a timeout.
                        return IoC.Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(f.Result - 1, 1), tokenSource.Token);

                        //int returnValue = (int)task.Result.GetValue(0);

                        //// inform the developer.
                        //IoC.Logger.Log($"register: {f.Result - 1} -- Value: {(int)serverResponse.GetValue(0)}");

                        //// return first response
                        //return task;
                    }
                    // the user canceled the task
                    catch (OperationCanceledException) { return null; }
                    // the communication failed
                    catch (System.IO.IOException) { return null; }
                    // everything else
                    catch (Exception) { return null; }

                });
            }
            catch (ArgumentException)
            {
                Console.WriteLine(@"The directory 'C:\Program Files' does not exist.");
            }

            //try
            //{


            //    //{
            //    //    DoMoreWork(registerStrings);
            //    //}
            //    //else
            //    //{
            //    //    return;
            //    //}
            //}
            //catch (AggregateException ag)
            //{
            //    foreach (var ex in ag.InnerExceptions)
            //    {
            //        // inform the developer about error
            //        IoC.Logger.Log($"Exception is : {ex.Message}");

            //        // update the user about the error.
            //        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

            //        // Trying to stop the app gracefully.
            //        IoC.ReleaseOmicron.ProcessErrors();
            //    }

            //}

            //// Use ParallelOptions instance to store the CancellationToken
            //ParallelOptions parallelingOptions = new ParallelOptions
            //{
            //    // associate cancellation token.
            //    CancellationToken = IoC.Commands.Token,
            //    // set limit for the parallelism equivalent of hardware core count
            //    MaxDegreeOfParallelism = registerStrings.Count > Environment.ProcessorCount ? Environment.ProcessorCount : registerStrings.Count
            //};

            ////// Partition the entire source array.
            ////var rangePartitioner = Partitioner.Create(0, registerStrings.Count);

            //// Use ConcurrentQueue to enable safe enqueue from multiple threads.
            //var exceptions = new ConcurrentQueue<Exception>();

            //try
            //{
            //    Parallel.For(fromInclusive: 0, // from value
            //                toExclusive: registerStrings.Count, // to value
            //                parallelOptions: parallelingOptions, // paralleling options
            //                localInit: () => 0, // local value initialization
            //                body: (int i, ParallelLoopState loopState, int response) => // function to retrieve register values from the unit under test.
            //                {
            //                    try
            //                    {
            //                        // is the register is between modbus holding register range?
            //                        if (ushort.TryParse(registerStrings[i].Trim(), out ushort register))
            //                        {
            //                            // start a task to read holding register (Function 0x03)
            //                            int[] serverResponse = IoC.Communication.EAModbusClient.ReadHoldingRegisters(register - 1, 1);

            //                            // decide if serverResponse is acceptable only criteria is the length of the response.
            //                            if (serverResponse.Length > 0)
            //                            {
            //                                // server response
            //                                response = serverResponse[0];

            //                                // save server response information as a Tuple
            //                                var (MaxRegisterValue, MinRegisterValue) = GetServerResponseAsync(serverResponse[0], i);

            //                                // assign MaxTestValue
            //                                IoC.CMCControl.MaxValues.SetValue(MaxRegisterValue, i);

            //                                // assign MinTestValue
            //                                IoC.CMCControl.MinValues.SetValue(MinRegisterValue, i);

            //                                return response;
            //                            }
            //                            else
            //                            {
            //                                // server failed to respond. Ignoring it until find a better option.
            //                                // inform the developer about error
            //                                IoC.Logger.Log($"register: {register} -- serverResponse : No server response");

            //                                return int.MaxValue;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            return int.MinValue;

            //                            // illegal register address
            //                            throw new ArgumentOutOfRangeException($"Register: {registerStrings[i].Trim()} is a illegal value.");
            //                        }
            //                    }
            //                    // Store the exception and continue with the loop.                    
            //                    catch (Exception e)
            //                    {
            //                        exceptions.Enqueue(e);

            //                        return int.MinValue;
            //                    }

            //                },
            //                localFinally: (value) =>
            //                {
            //                    // inform the developer.
            //                    IoC.Logger.Log($"register: {value} read");

            //                   // parallelingOptions.CancellationToken.ThrowIfCancellationRequested();
            //                }
            //                );

            //    // Throw the exceptions here after the loop completes.
            //    if (exceptions.Count > 0)
            //    {
            //        throw new AggregateException(exceptions);
            //    }
            //}
            //catch (OperationCanceledException ex)
            //{
            //    // inform the developer about error
            //    IoC.Logger.Log($"Exception is : {ex.Message}");

            //    // update the user about the error.
            //    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

            //    // Trying to stop the app gracefully.
            //    IoC.ReleaseOmicron.ProcessErrors();
            //}
            //catch (System.IO.IOException ex)
            //{
            //    // inform the developer about error
            //    IoC.Logger.Log($"Exception is : {ex.Message}");

            //    // update the user about the error.
            //    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

            //    // Trying to stop the app gracefully.
            //    IoC.ReleaseOmicron.ProcessErrors();
            //}
            //catch (AggregateException ag)
            //{
            //    foreach (var ex in ag.InnerExceptions)
            //    {
            //        // inform the developer about error
            //        IoC.Logger.Log($"Exception is : {ex.Message}");

            //        // update the user about the error.
            //        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

            //        // Trying to stop the app gracefully.
            //        IoC.ReleaseOmicron.ProcessErrors();
            //    }

            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registers"></param>
        /// <param name="action"></param>
        public void DoMoreWork(List<string> registers, Func<Task<ushort>, Task<int[]>> action)
        {
            Console.WriteLine("<------------------------------- Hey there");

            // Use ParallelOptions instance to store the CancellationToken
            ParallelOptions parallelingOptions = new ParallelOptions
            {
                // associate cancellation token.
                CancellationToken = IoC.Commands.Token,
                // set limit for the parallelism equivalent of hardware core count
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            //// Partition the entire source array.
            //var rangePartitioner = Partitioner.Create(0, registerStrings.Count);

            // Use ConcurrentQueue to enable safe enqueue from multiple threads.
            var exceptions = new ConcurrentQueue<Exception>();

            try
            {
                //if (Environment.ProcessorCount > registers.Count)
                //{

                //}

                Parallel.For(fromInclusive: 0, // from value
                            toExclusive: registers.Count, // to value
                            parallelOptions: parallelingOptions, // paralleling options
                            localInit: () =>
                            {
                                var tuple = (register: 0, MaxRegisterValue: int.MinValue, MinRegisterValue: int.MaxValue, Index: 0);
                                return tuple;
                            }, // int.MinValue,  // local value initialization                           
                            body: (int i, ParallelLoopState loopState, ValueTuple<int, int, int, int> response) => // function to retrieve register values from the unit under test.
                            {
                                try
                                {
                                    // is the register is between modbus holding register range?
                                    if (ushort.TryParse(registers[i].Trim(), out ushort register))
                                    {

                                        // start a task to read holding register (Function 0x03)
                                        // response = action(register);

                                        // current register to read
                                        response.Item1 = register;

                                        Task<ushort> number = Task.FromResult(register);

                                        // save server response information as a Tuple
                                        var (MaxRegisterValue, MinRegisterValue) = GetServerResponseAsync((int)action(number).Result.GetValue(0), i);

                                        // value of MaxRegisterValue
                                        response.Item2 = MaxRegisterValue;

                                        // value of MaxRegisterValue
                                        response.Item3 = MinRegisterValue;

                                        // pass Index value to next process.
                                        response.Item4 = i;

                                        // pass Tuple to localFinally process.
                                        return response;
                                    }
                                    else
                                    {

                                        // request stop the loop
                                        if (!loopState.IsStopped)
                                            loopState.Stop();

                                        return response; // int.MinValue;
                                        // illegal register address
                                        throw new ArgumentOutOfRangeException($"Register: {registers[i].Trim()} is a illegal value.");

                                    }
                                }
                                catch (OperationCanceledException)
                                {
                                    // request stop the loop
                                    loopState.Stop();

                                    return response; // int.MinValue;
                                }
                                // the communication failed
                                catch (System.IO.IOException)
                                {
                                    // request stop the loop
                                    loopState.Stop();

                                    return response; // int.MinValue;
                                }
                                // Store the exception and continue with the loop.                    
                                catch (Exception e)
                                {
                                    // enqueue exception and quit the loop
                                    exceptions.Enqueue(e);
                                    // request stop the loop
                                    loopState.Stop();
                                    // set response to lowest integer.
                                    // might need to find some other value/logic here.

                                    return response; // int.MinValue;
                                }

                            },
                            localFinally: (value) =>
                            {
                                // inform the developer.
                                IoC.Logger.Log($"Index: {value.Item4} -- register: {value.Item1} -- Max value read {value.Item2} --- MinValue: {value.Item3}");


                                // save server response information as a Tuple
                                // var (MaxRegisterValue, MinRegisterValue) = GetServerResponseAsync(value.Item2, value.Item3);

                                // assign MaxTestValue
                                IoC.CMCControl.MaxValues.SetValue(value.Item2, value.Item4);

                                // assign MinTestValue
                                IoC.CMCControl.MinValues.SetValue(value.Item3, value.Item4);
                                // parallelingOptions.CancellationToken.ThrowIfCancellationRequested();
                            }
                            );

                // Throw the exceptions here after the loop completes.
                if (exceptions.Count > 0)
                {
                    throw new AggregateException(exceptions);
                }
            }
            catch (OperationCanceledException ex)
            {
                // inform the developer about error
                IoC.Logger.Log($"Exception is : {ex.Message}");

                // update the user about the error.
                IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

                // Trying to stop the app gracefully.
                IoC.ReleaseOmicron.ProcessErrors();

            }
            catch (System.IO.IOException ex)
            {
                // inform the developer about error
                IoC.Logger.Log($"Exception is : {ex.Message}");

                // update the user about the error.
                IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

                // Trying to stop the app gracefully.
                IoC.ReleaseOmicron.ProcessErrors();
            }
            catch (AggregateException ag)
            {
                foreach (var ex in ag.InnerExceptions)
                {
                    // inform the developer about error
                    IoC.Logger.Log($"Exception is : {ex.Message}");

                    // update the user about the error.
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

                    // Trying to stop the app gracefully.
                    IoC.ReleaseOmicron.ProcessErrors();
                }

            }
        }

        /// <summary>
        /// Allows to read test register values separated with comma specified by "Measurement Interval".
        /// </summary>
        public async void DoSomeWork(ushort register, int i)
        {
            // Was cancellation already requested?
            if (IoC.Commands.Token.IsCancellationRequested)
            {
                IoC.Logger.Log("Task was canceled before it got started.");

                // throw if the cancellation requested.
                IoC.Commands.Token.ThrowIfCancellationRequested();
            }

            IoC.Logger.Log($"Task {nameof(DoSomeWork)} starts.");

            // lock the task
            await AsyncAwaiter.AwaitAsync(nameof(DoSomeWork), async () =>
            {

                // start a task to read holding register (Function 0x03)
                int[] serverResponse = await IoC.Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(register - 1, 1), IoC.Commands.Token);

                // decide if serverResponse is acceptable only criteria is the length of the response.
                if (serverResponse.Length > 0)
                {

                    // save server response information as a Tuple
                    var (MaxRegisterValue, MinRegisterValue) = GetServerResponseAsync(serverResponse[0], i);

                    // assign MaxTestValue
                    IoC.CMCControl.MaxValues.SetValue(MaxRegisterValue, i);

                    // assign MinTestValue
                    IoC.CMCControl.MinValues.SetValue(MinRegisterValue, i);

                    //// inform the developer about register reading
                    //IoC.Logger.Log($"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: register: {register} -- maxResponse : {MaxRegisterValue} -- minResponse : {MinRegisterValue}");

                    if (IoC.Commands.Token.IsCancellationRequested)
                    {
                        IoC.Logger.Log("Task canceled.");
                        // throw if the cancellation requested.
                        IoC.Commands.Token.ThrowIfCancellationRequested();
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
                        CancellationTokenSource cancellation = new CancellationTokenSource(Convert.ToInt32(IoC.TestDetails.MeasurementInterval) * 2);

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
                                            int[] serverResponse = await IoC.Task.Run(() => IoC.Communication.EAModbusClient.ReadHoldingRegisters(register - 1, 1));
                                            
                                            // decide if serverResponse is acceptable only criteria is the length of the response.
                                            if (serverResponse.Length > 0)
                                            {

                                                // save server response information as a Tuple
                                                var (MaxRegisterValue, MinRegisterValue) = GetServerResponseAsync(serverResponse[0], i);

                                                // assign MaxTestValue
                                                IoC.CMCControl.MaxValues.SetValue(MaxRegisterValue, i);

                                                // assign MinTestValue
                                                IoC.CMCControl.MinValues.SetValue(MinRegisterValue, i);

                                                //// inform the developer about register reading
                                                //IoC.Logger.Log($"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: register: {register} -- maxResponse : {MaxRegisterValue} -- minResponse : {MinRegisterValue}");
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
                            }, cancellation.Token);
                    }

                    // throw if the cancellation requested.
                    IoC.Commands.Token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException ex)
                {
                    // inform the developer about error
                    IoC.Logger.Log($"Exception is : {ex.Message}");

                    // update the user about the error.
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Operation canceled: {ex.Message}.";

                    // Trying to stop the app gracefully.
                    await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrors());
                }
                catch (System.IO.IOException ex)
                {
                    // inform the developer about error
                    IoC.Logger.Log($"Exception is : {ex.Message}");

                    // update the user about the error.
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Modbus Communication failed: {ex.Message}.";

                    // Trying to stop the app gracefully.
                    await IoC.Task.Run(() => IoC.ReleaseOmicron.ProcessErrors());
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

            });
        }

        /// <summary>
        /// Function to return a Tuple that holds <see cref="MinTestValue"/> and <see cref="MaxTestValue"/>
        /// </summary>
        /// <returns>Returns a Tuple with ramping signal properties</returns>       
        private (int MaxResponse, int MinResponse) GetServerResponseAsync(int serverResponse, int Index)
        {
            try
            {
                // load Tuple variables with default or previously stored values
                int MinResponse = (int)IoC.CMCControl.MinValues.GetValue(Index);
                int MaxResponse = (int)IoC.CMCControl.MaxValues.GetValue(Index);

                // update minimum value with new min value or not
                MinResponse = Math.Min(MinResponse, serverResponse);

                // update maximum value with new max value or not
                MaxResponse = Math.Max(MaxResponse, serverResponse);

                // return server response
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
