
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// Disconnects and releases associated Omicron Test Set.
    /// </summary>
    public class ReleaseOmicron
    {
        #region Public Method

        /// <summary>
        /// Disconnects and releases associated Omicron Test Set.
        /// </summary>
        public async Task ReleaseAsync()
        {
            try
            {
                // execute the Release command.
                await IoC.Task.Run(async() =>
                {
                    try
                    {

                        // inform the developer
                        IoC.Logger.Log($"{nameof(ReleaseAsync)}: started", LogLevel.Informative);

                        // unlock attached Omicron Test Set                    
                        await IoC.Task.Run(() => IoC.CMCControl.CMEngine.DevUnlock(IoC.CMCControl.DeviceID));
                        // wait some make sure release Omicron
                        await Task.Delay(2000);

                        // Destruct Omicron Test set
                        IoC.CMCControl.CMEngine = null;

                        // inform the developer
                        IoC.Logger.Log($"{nameof(ReleaseAsync)}: completed", LogLevel.Informative);

                    }
                    // Re-throw all exceptions.
                    catch (Exception)
                    {
                        throw;
                    }
                }, IoC.Commands.Token);

            }
            catch (COMException ex)
            {
                // inform the developer about error.
                IoC.Logger.Log($"Exception: {ex.Message}\nPlease try to re-start the program.");

                // update Current Test File
                IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);

                // return this task.
                return;
            }
            catch (OperationCanceledException ex)
            {
                // inform the developer about error.
                IoC.Logger.Log($"Exception: {ex.Message}\nPlease try to re-start the program.");

                // update Current Test File
                IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);

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

                // update Current Test File
                IoC.Communication.UpdateCurrentTestFileListItem(CommunicationViewModel.TestStatus.Interrupted);

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

        /// <summary>♥M
        /// Handles errors and stops the app gracefully.
        /// </summary>
        /// <param name="userRequest">true if test interrupt requested by the user
        /// true if test completed itself</param>
        public async Task<int> ProcessErrorsAsync(bool userRequest = true)
        {
            try
            {
                return await AsyncAwaiter.AwaitResultAsync(nameof(ProcessErrorsAsync), async () =>
                {
                    try
                    {
                        // if the user wants to stop the test
                        if (userRequest)
                        {
                            // try to cancel thread running Omicron Test Set
                            IoC.Commands.TokenSource.Cancel(true);

                            // update In Progress test file
                            IoC.Communication.CurrentTestFileListItem.TestStepBackgroundColor = Strings.color_test_interrupted;
                            IoC.Communication.CurrentTestFileListItem.TestToolTip = $"{Path.GetFileName(IoC.Communication.CurrentTestFileListItem.FullFileName)}.{Environment.NewLine}{Strings.test_status_interrupted}";
                            IoC.Communication.CurrentTestFileListItem.IsDeletable = true;
                        }

                        // update developer
                        IoC.Logger.Log($"Test {(userRequest ? "interrupted" : "completed")}", LogLevel.Informative);

                        // update the user
                        IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy hh:mm:ss.fff}: Test { (userRequest ? "interrupted by the user." : "completed.")}";

                        // Disconnect Modbus Communication.
                        await IoC.Task.Run(() => IoC.Communication.EAModbusClient.Disconnect());

                        // check if timer is initialized then dispose it.
                        await IoC.Task.Run(() => IoC.CMCControl.MdbusTimer?.Dispose());

                        // Turn off outputs of Omicron Test Set.
                        //await IoC.Task.Run(() => IoC.PowerOptions.TurnOffCMC());
                        await IoC.PowerOptions.TurnOffCMCAsync();

                        // release omicron test set.
                        await ReleaseAsync();
                        //await IoC.Task.Run(() => Release());

                        // Progress bar is invisible
                        IoC.CMCControl.IsTestRunning = IoC.Commands.IsConnectionCompleted = IoC.Commands.IsConnecting = IoC.Communication.EAModbusClient.Connected;

                        // change color of Cancel Command button to Red
                        IoC.Commands.CancelForegroundColor = "ff0000";

                        return 0;
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

                // return an empty string.
                return -1;
            }
        }
    }


    #endregion
}
