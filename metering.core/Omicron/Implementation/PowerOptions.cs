using System;
using System.Threading;
using System.Threading.Tasks;
using metering.core.Resources;

namespace metering.core
{
    /// <summary>
    /// Handles Omicron Power up, down and etc. options
    /// </summary>
    public class PowerOptions
    {
        #region Private Methods

        /// <summary>
        /// sends command to turn off Omicron Test Set.
        /// </summary>
        /// <param name="cancellationToken">token to cancel this task.</param>
        /// <returns>Returns <see cref="Task"/></returns>
        private async Task<string> CMCOffAsync(CancellationToken cancellationToken)
        {

            // generate a TaskCompletionSource.
            var taskCompletionSource = new TaskCompletionSource<string>();

            // Registering a lambda into the cancellationToken
            cancellationToken.Register(() =>
            {
                // received a cancellation message, cancel the TaskCompletionSource.Task
                taskCompletionSource.TrySetCanceled();
            });

            // generate CMEngine.Exec Task
            var task = IoC.Task.Run(() => IoC.StringCommands.SendStringCommandsAsync(omicronCommand: OmicronStringCmd.out_ana_off));

            // Wait for the task to finish.
            var completedTask = await Task.WhenAny(task, taskCompletionSource.Task);

            // return completed task
            return await completedTask;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Turns off outputs of Omicron Test Set and release it.
        /// </summary>
        public async Task TurnOffCMCAsync()
        {
            // enable cancellation token
            using (var cancellation = new CancellationTokenSource())
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

                    // update the log
                    IoC.Logger.Log($"{nameof(TurnOffCMCAsync)} started.", LogLevel.Informative);

                    // execute string command
                    var executeTask = CMCOffAsync(cancellationToken: cancellation.Token);

                    // wait for the result
                    await executeTask;

                    // update the log
                    IoC.Logger.Log($"{nameof(TurnOffCMCAsync)} stopped. result: {executeTask}", LogLevel.Informative);

                }
                catch (Exception)
                {
                    // re-throw error
                    throw;
                }

                // monitor if cancellation requested
                await cancellationTask;
            }
        }

        /// <summary>
        /// Turns on outputs of Omicron Test Set.
        /// </summary>
        public async Task TurnOnCMCAsync()
        {
            try
            {
                // lock the task
                await AsyncAwaiter.AwaitAsync(nameof(TurnOnCMCAsync), async () =>
                {
                    // update the developer
                    IoC.Logger.Log($"{nameof(TurnOnCMCAsync)} started.", LogLevel.Informative);

                    // Send command to Turn On Analog Outputs
                    await IoC.Task.Run(() => IoC.StringCommands.SendStringCommandsAsync(OmicronStringCmd.out_ana_on));

                    // update the developer
                    IoC.Logger.Log($"{nameof(TurnOnCMCAsync)} completed.", LogLevel.Informative);

                });
            }
            catch (Exception)
            {
                // re-throw error
                throw;
            }
        }

        #endregion

    }
}
