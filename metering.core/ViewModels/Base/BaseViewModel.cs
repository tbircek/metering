using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// Base view model provides comman functionality across viewmodels
    /// </summary>
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// the event when any property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// call this to fire <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
            Debug.WriteLine($"{DateTime.Now.ToLocalTime()}: SetProperty: (name: {name}) processed successfull.");
        }

        #region Helpers

        /// <summary>
        /// Runs a command if the updating flag is not set
        /// If the flag is true (its indicating the command is running) then the action won't start again
        /// If the flag is false (its indicating the command is not running) then the action starts
        /// When the action completed the flag set to false
        /// </summary>
        /// <param name="updatingFlag">defines if command is running</param>
        /// <param name="action">the action to be run while the command is alread not running </param>
        /// <returns></returns>
        protected async Task RunCommand (Expression<Func<bool>> updatingFlag, Func<Task> action)
        {
            // check if the flag is true (the command is already running)
            if (updatingFlag.GetPropertyValue())
                return;

            // set the property flag is true to indicate task is running
            updatingFlag.SetPropertyValue(true);

            try
            {
                // run the action
                await action();
            }
            finally
            {
                updatingFlag.SetPropertyValue(false);
            }
        }
        #endregion
    }
}
