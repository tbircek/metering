
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// handles anything to do with Tasks
    /// </summary>
    public class TaskManager : ITaskManager
    {
        #region Task Methods

        public async Task Run(Func<Task> function, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // try and run the task
                await Task.Run(function);
            }
            catch (Exception ex)
            {
                // log the error
                LogError(ex, origin, filePath, lineNumber);

                // throw it as normal
                throw;
            }
        }

        public async Task<TResult> Run<TResult>(Func<Task<TResult>> function, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // try and run the task
                return await Task.Run(function, cancellationToken);
            }
            catch (Exception ex)
            {
                // log the error
                LogError(ex, origin, filePath, lineNumber);

                // throw it as normal
                throw;
            }
        }

        public async Task<TResult> Run<TResult>(Func<Task<TResult>> function, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // try and run the task
                return await Task.Run(function);
            }
            catch (Exception ex)
            {
                // log the error
                LogError(ex, origin, filePath, lineNumber);

                // throw it as normal
                throw;
            }
        }

        public async Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // try and run the task
                return await Task.Run(function, cancellationToken);
            }
            catch (Exception ex)
            {
                // log the error
                LogError(ex, origin, filePath, lineNumber);

                // throw it as normal
                throw;
            }
        }

        public async Task<TResult> Run<TResult>(Func<TResult> function, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // try and run the task
                return await Task.Run(function);
            }
            catch (Exception ex)
            {
                // log the error
                LogError(ex, origin, filePath, lineNumber);

                // throw it as normal
                throw;
            }
        }

        public async Task Run(Func<Task> function, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // try and run the task
                await Task.Run(function,cancellationToken);
            }
            catch (Exception ex)
            {
                // log the error
                LogError(ex, origin, filePath, lineNumber);

                // throw it as normal
                throw;
            }
        }

        public async Task Run(Action action, CancellationToken cancellationToken, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // try and run the task
                await Task.Run(action, cancellationToken);
            }
            catch (Exception ex)
            {
                // log the error
                LogError(ex);

                // throw it as normal
                throw;
            }
        }

        public async Task Run(Action action, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            try
            {
                // try and run the task
                await Task.Run(action);
            }
            catch (Exception ex)
            {
                // log the error
                LogError(ex);

                // throw it as normal
                throw;
            }
        }

        #endregion

        #region Private Helper Methods

        /// <summary>
        /// logs the given error to the log factory
        /// </summary>
        /// <param name="ex">the exception to log</param>
        private void LogError(Exception ex, [CallerMemberName] string origin = "", [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        {
            IoC.Logger.Log($"An unexpected error has occurred while running IoC.Task.Run. {ex.Message}", LogLevel.Debug, origin, filePath, lineNumber);
        }
        #endregion
    }
}
