using System;
using System.Diagnostics;
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
        /// The command handles cancelling New Test addition view and returns default view
        /// </summary>
        public ICommand CancelNewTestCommand { get; set; }

        /// <summary>
        /// NominalValuesViewModel to access its values.
        /// </summary>
        private NominalValuesViewModel NominalValues { get; }

        ///// <summary>
        ///// TestDetailViewModel to access its values.
        ///// </summary>
        //private TestDetailViewModel TestDetail { get; }
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandsViewModel()
        {
            AddNewTestCommand = new RelayCommand(param => CopyNominalValues());
            CancelNewTestCommand = new RelayCommand(param => CancelNominalValues());
        }

        #endregion
        
        #region Helper Methods

        /// <summary>
        /// Shows test steps with values reset to nominal values
        /// </summary>
        private void CopyNominalValues()
        {
            Debug.WriteLine("CopyNominalValues is running:");
        }


        /// <summary>
        /// Navigate backwards to main view / shows default nominal values
        /// resets values specified in test step view to nominal values
        /// </summary>
        private void CancelNominalValues()
        {           
            Debug.WriteLine("CancelNominalValues is running:");
            // testDetailsModel.TestDetail.Clear();
        }

        #endregion
    }
}
