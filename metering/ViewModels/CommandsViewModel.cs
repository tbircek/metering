using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace metering
{
    public class CommandsViewModel : BaseViewModel
    {

        #region Public Commands

        /// <summary>
        /// The command to handle change view to test plan detail view
        /// and populate items with nominal values
        /// </summary>
        public ICommand AddNewTestCommand { get; set; }

        /// <summary>
        /// Title of AddNewTestCommand
        /// </summary>
        public string AddNewTestCommandTitle { get; set; } = "Add New Voltage Test";


        /// <summary>
        /// The command handles cancelling New Test addition view and returns default view
        /// </summary>
        public ICommand CancelNewTestCommand { get; set; }

        ///// <summary>
        ///// NominalValuesViewModel to access its values.
        ///// </summary>
        //private NominalValuesViewModel NominalValues { get; }

        ///// <summary>
        ///// TestStepListViewModel to access its values.
        ///// </summary>
        //private TestStepListViewModel TestDetail { get; }
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandsViewModel()
        {
            AddNewTestCommand = new RelayParameterizedCommand(async (parameter) => await CopyNominalValues(parameter));
            CancelNewTestCommand = new RelayParameterizedCommand(async (parameter) => await CancelNominalValues(parameter));
        }

        #endregion

        #region Helper Methods


        /// <summary>
        /// Shows test steps with values reset to nominal values
        /// </summary>
        /// <param name="parameter">Button content attached property in the view</param>
        /// <returns>void</returns>
        private async Task CopyNominalValues(object parameter)
        {
            // Simulate the page creation.
            await Task.Delay(100);

            Debug.WriteLine("CopyNominalValues is running:");
        }


        /// <summary>
        /// Navigate backwards to main view / shows default nominal values
        /// resets values specified in test step view to nominal values
        /// </summary>
        /// <param name="parameter">not used at the moment</param>
        /// <returns>void</returns>
        private async Task CancelNominalValues(object parameter)
        {
            // simulate clearing test steps and returning main page
            await Task.Delay(100);

            Debug.WriteLine("CancelNominalValues is running:");
        }

        #endregion
    }
}
