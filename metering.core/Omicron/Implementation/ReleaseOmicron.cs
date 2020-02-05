using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// Disconnects and releases associated Omicron Test Set.
    /// </summary>
    public class ReleaseOmicron
    {
        #region Private Methods

        /// <summary>
        /// Provides disconnect and release associated Omicron Test Set.
        /// </summary>
        /// <returns>Returns void</returns>
        private async Task ReleaseOmicronAsync(CancellationToken cancellationToken)
        {

            // generate a TaskCompletionSource.
            var taskCompletionSource = new TaskCompletionSource<int>();

            // Registering a lambda into the cancellationToken
            cancellationToken.Register(() =>
            {
                // received a cancellation message, cancel the TaskCompletionSource.Task
                taskCompletionSource.TrySetCanceled();
            });

            // generate ReadHoldingRegisters Task
            var task = IoC.Task.Run(() => IoC.CMCControl.CMEngine.DevUnlock(IoC.CMCControl.DeviceID));

            // Wait for the task to finish.
            Task completedTask = await Task.WhenAny(task, taskCompletionSource.Task);

            // Destruct Omicron Test set
            IoC.CMCControl.CMEngine = null;

            // return completed task.
            await completedTask;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Disconnects and releases associated Omicron Test Set.
        /// </summary>
        public async Task ReleaseAsync()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                // generate Task to cancel
                var releaseTask = IoC.Task.Run(() =>
                {
                    // cancel main Task
                    IoC.Commands.TokenSource.Cancel(true);
                });

                try
                {
                    //// execute the Release command.
                    //await IoC.Task.Run(async () =>
                    //{
                    //    try
                    //    {

                    // inform the developer
                    IoC.Logger.Log($"{nameof(ReleaseAsync)}: started", LogLevel.Informative);

                    //// unlock attached Omicron Test Set                    
                    //await IoC.Task.Run(() => IoC.CMCControl.CMEngine.DevUnlock(IoC.CMCControl.DeviceID));
                    //// wait some make sure release Omicron
                    //await Task.Delay(2000);

                    // unlock attached Omicron Test Set    
                    var runningTask = ReleaseOmicronAsync(cancellationTokenSource.Token);

                    await runningTask;

                    //// Destruct Omicron Test set
                    //IoC.CMCControl.CMEngine = null;

                    // inform the developer
                    IoC.Logger.Log($"{nameof(ReleaseAsync)}: completed", LogLevel.Informative);

                    //    }
                    //    // Re-throw all exceptions.
                    //    catch (Exception)
                    //    {
                    //        throw;
                    //    }
                    //}, IoC.Commands.Token);

                }
                catch (COMException ex)
                {
                    // inform the developer about error.
                    IoC.Logger.Log($"Exception: {ex.Message}\nPlease try to re-start the program.");

                    // update Current Test File
                    IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);

                    //// return this task.
                    //return;
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

                }
                catch (Exception ex)
                {
                    if (!IoC.Commands.TokenSource.IsCancellationRequested)
                    {
                        // inform the developer about error
                        IoC.Logger.Log($"InnerException is : {ex.Message}");

                        // inform the user about error
                        IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\trelease Omicron: error detected.";

                        // catch inner exceptions if exists
                        if (ex.InnerException != null)
                        {
                            // inform the user about more details about error.
                            IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.";
                        }

                        // update Current Test File
                        IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);
                    }
                }

                await releaseTask;
            }
        }

        /// <summary>
        /// Handles errors and stops the app gracefully.
        /// </summary>
        /// <param name="userRequest">true if test interrupt requested by the user
        /// true if test completed itself</param>
        public async Task ProcessErrorsAsync(bool userRequest = true)
        {
            try
            {
                await AsyncAwaiter.AwaitAsync(nameof(ProcessErrorsAsync), async () =>
                {
                    try
                    {
                        // update developer
                        IoC.Logger.Log($"Test {(userRequest ? "interrupted" : "completed")}", LogLevel.Informative);

                        // update the user
                        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}: Test { (userRequest ? "interrupted by the user." : "completed.")}";

                        // Turn off outputs of Omicron Test Set.
                        // await IoC.Task.Run(() => IoC.PowerOptions.TurnOffCMCAsync().ConfigureAwait(continueOnCapturedContext: false));
                        await IoC.PowerOptions.TurnOffCMCAsync();

                        // if the user wants to stop the test
                        if (userRequest)
                        {
                            // try to cancel thread running Omicron Test Set
                            // IoC.Commands.TokenSource.Cancel(true);

                            // update In Progress test file
                            IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);
                        }

                        // Disconnect Modbus Communication.
                        await IoC.Task.Run(() => IoC.Communication.EAModbusClient.Disconnect()).ConfigureAwait(continueOnCapturedContext: false); ;

                        // check if timer is initialized then dispose it.
                        await IoC.Task.Run(() => IoC.CMCControl.MdbusTimer?.Dispose()).ConfigureAwait(continueOnCapturedContext: false);

                        // Progress bar is invisible
                        IoC.CMCControl.IsTestRunning = IoC.Commands.IsConnectionCompleted = IoC.Commands.IsConnecting = IoC.Communication.EAModbusClient.Connected;

                        // change color of Cancel Command button to Red
                        IoC.Commands.CancelForegroundColor = "ff0000";

                        // release omicron test set.
                        // await IoC.Task.Run(() => ReleaseAsync().ConfigureAwait(continueOnCapturedContext: false));
                        await ReleaseAsync();

                    }
                    catch (Exception)
                    {
                        // recast
                        throw;
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
        /// Disconnects and releases associated Omicron Test Set.
        /// </summary>
        [Obsolete("Use async version", true)]
        public async void Release()
        {
            try
            {
                // lock the task
                await AsyncAwaiter.AwaitAsync(nameof(Release), async () =>
                {
                    // inform the developer
                    IoC.Logger.Log($"{nameof(Release)}: started", LogLevel.Informative);

                    // unlock attached Omicron Test Set                    
                    await IoC.Task.Run(() => IoC.CMCControl.CMEngine.DevUnlock(IoC.CMCControl.DeviceID));
                    // wait some make sure release Omicron
                    await Task.Delay(2000);

                    // Destruct Omicron Test set
                    IoC.CMCControl.CMEngine = null;

                    // inform the developer
                    IoC.Logger.Log($"{nameof(Release)}: completed", LogLevel.Informative);

                });
            }
            catch (Exception ex)
            {
                // inform the developer about error
                IoC.Logger.Log($"InnerException is : {ex.Message}");

                // inform the user about error
                IoC.Communication.Log = $"Time: {DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}\trelease Omicron: error detected.";

                // catch inner exceptions if exists
                if (ex.InnerException != null)
                {
                    // inform the user about more details about error.
                    IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Inner exception: {ex.InnerException}.";
                }
            }
        }

    }


    #endregion
}
