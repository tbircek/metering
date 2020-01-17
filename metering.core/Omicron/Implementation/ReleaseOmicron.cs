
using System;

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
        public async void Release()
        {
            try
            {
                // lock the task
                await AsyncAwaiter.AwaitAsync(nameof(Release), async () =>
                {
                    // inform the developer
                    IoC.Logger.Log($"Release: started", LogLevel.Informative);

                    // unlock attached Omicron Test Set                    
                    await IoC.Task.Run(() => IoC.CMCControl.CMEngine.DevUnlock(IoC.CMCControl.DeviceID));

                    // Destruct Omicron Test set
                    IoC.CMCControl.CMEngine = null;
                    
                    // inform the developer
                    IoC.Logger.Log($"Release: completed", LogLevel.Informative);

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

        /// <summary>
        /// Handles errors and stops the app gracefully.
        /// </summary>
        /// <param name="userRequest">true if test interrupt requested by the user
        /// true if test completed itself</param>
        public void ProcessErrors(bool userRequest = true)
        {
            // if the user wants to stop the test
            if (userRequest)
                // try to cancel thread running Omicron Test Set
                IoC.Commands.TokenSource.Cancel(true);
            
            // update developer
            IoC.Logger.Log($"Test {(userRequest ? "interrupted" : "completed")}", LogLevel.Informative);

            // update the user
            IoC.Communication.Log = $"{DateTime.Now.ToLocalTime():MM/dd/yy HH:mm:ss.fff}: Test { (userRequest ? "interrupted by the user." : "completed.")}";

            // Disconnect Modbus Communication and wait 200msec
            // IoC.Task.Run(() => IoC.Communication.EAModbusClient.Disconnect());
            var disconnectTask = IoC.Task.Run(() => IoC.Communication.EAModbusClient.Disconnect());
            disconnectTask.Wait(200);

            // check if timer is initialized then dispose it.
            IoC.CMCControl.MdbusTimer?.Dispose();

            // Turn off outputs of Omicron Test Set and wait 200msec.
            // IoC.Task.Run(() => IoC.PowerOptions.TurnOffCMC());
            var turnOffTask = IoC.Task.Run(() => IoC.PowerOptions.TurnOffCMC());
            turnOffTask.Wait(500);

            // release omicron test set and wait 200msec
            // IoC.Task.Run(() => Release());
            var releaseTask = IoC.Task.Run(() => Release());
            releaseTask.Wait(500);

            // Progress bar is invisible
            IoC.CMCControl.IsTestRunning = IoC.Commands.IsConnectionCompleted = IoC.Commands.IsConnecting = IoC.Communication.EAModbusClient.Connected;

            // change color of Cancel Command button to Red
            IoC.Commands.CancelForegroundColor = "ff0000";

        }
    }


    #endregion
}
