using System;
using System.Windows.Input;

namespace metering
{
    /// <summary>
    /// basic command
    /// </summary>
    public class RelayCommand : ICommand 
    {
        #region Private Members
        /// <summary>
        /// action ( with parameters) to run
        /// </summary>
        private Action<object> actions;

        /// <summary>
        /// action (without any parameter) to run
        /// </summary>
        private Action action;

        /// <summary>
        /// function's eligibility to run
        /// </summary>
        private readonly Func<object, bool> canExecuteActions;

        #endregion

        #region Constructor
        /// <summary>
        /// Default command with action anf function of canexecute
        /// </summary>
        /// <param name="executeActions"></param>
        /// <param name="canExecuteAction"></param>
        public RelayCommand(Action<object> executeActions, Func<object, bool> canExecuteAction)
        {
            actions = executeActions;
            canExecuteActions = canExecuteAction;
        }

        /// <summary>
        /// Default command with action with parameter
        /// </summary>
        /// <param name="executeActions"></param>
        public RelayCommand(Action<object> executeActions)
        {
            actions = executeActions;
        }

        /// <summary>
        /// Default command with action only
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action)
        {
            this.action = action;
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Executes the command action
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            actions(parameter);
        }

        public void Execute()
        {
            action();
        }

        /// <summary>
        /// a command can always executes.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            // canExecuteActions?.Invoke(parameter) ?? true;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
       // public void InvokeCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #endregion

        #region Public Events
        /// <summary>
        /// when <see cref="CanExecute(object)"/> value changed this event fires.
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion               
    }
}
