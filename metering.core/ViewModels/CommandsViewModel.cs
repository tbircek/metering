using System.Diagnostics;
using System.Threading.Tasks;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public CommandsViewModel()
        {
            AddNewTestCommand = new RelayParameterizedCommand((parameter) => CopyNominalValues(parameter));
            CancelNewTestCommand = new RelayCommand(() => CancelTestDetailsPageShowing());
        }

        #endregion

        #region Helper Methods


        /// <summary>
        /// Shows test steps with values reset to nominal values
        /// </summary>
        /// <param name="parameter">Button content attached property in the view</param>
        /// <returns>void</returns>
        private async void CopyNominalValues(object parameter)
        {
            // Simulate the page creation.
            // await Task.Delay(100);

            // TODO: Pass NominalValues page values to the TestDetails page;
           
            // Show TestDetails page
            await Task.Factory.StartNew(()=> IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.TestDetails));
            Debug.WriteLine("CopyNominalValues(object parameter) is running:");
        }


        /// <summary>
        /// Navigate backwards to main view / shows default nominal values
        /// resets values specified in test step view to nominal values
        /// </summary>
        private async void CancelTestDetailsPageShowing()
        {
            // Show NominalValues page
            await Task.Factory.StartNew(() => IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.NominalValues));

            Debug.WriteLine("CancelTestDetailsPageShowing is running:");
        }

        #endregion
    }
}
