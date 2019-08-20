using System.Windows.Input;

namespace metering.core
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
        public string AddNewTestCommandTitle { get; set; } = "New Test";


        /// <summary>
        /// The command handles cancelling New Test addition view and returns default view
        /// </summary>
        public ICommand CancelNewTestCommand { get; set; }
        
        /// <summary>
        /// The command to handle connecting associated Omicron Test Set
        /// and communication to the UUT
        /// </summary>
        public ICommand StartTestCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandsViewModel()
        {            
            // Show a new Test details page populated with the user specified/accepted values
            AddNewTestCommand = new RelayCommand(() => IoC.NominalValues.CopyNominalValues());

            // Start a new test
            StartTestCommand = new RelayCommand(() => IoC.TestDetails.StartTest());

            // navigate back to nominal values page.
            CancelNewTestCommand = new RelayCommand(() => CancelTestDetailsPageShowing());
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Navigate backwards to main view / shows default nominal values
        /// resets values specified in test step view to nominal values
        /// </summary>
        private void CancelTestDetailsPageShowing()
        {
            // clear Test details view model
            IoC.Application.CurrentPageViewModel = null;

            // Show NominalValues page
            IoC.NominalValues.GetSelectedRadioButton("Voltage.AllZero");
            IoC.NominalValues.GetSelectedRadioButton("Current.AllZero");
            IoC.Application.GoToPage(ApplicationPage.NominalValues);
        }

        #endregion
    }
}
