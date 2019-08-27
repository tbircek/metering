namespace metering.core
{

    /// <summary>
    /// Application level ViewModel.
    /// </summary>
    public class ApplicationViewModel: BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// The current application page
        /// </summary>
        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.NominalValues;

        /// <summary>
        /// The view model to user for the current page when the CurrentPage changes
        /// This is not a live up-to-date view model of the current page
        /// it is used to set the view model of the current page at time of change
        /// </summary>
        public BaseViewModel CurrentPageViewModel { get; set; }

        /// <summary>
        /// Navigate to specified page
        /// </summary>
        /// <param name="page">The page to navigate</param>
        /// <param name="viewModel">The view model to set explicitly to the new page if exists </param>
        public void GoToPage(ApplicationPage page, BaseViewModel viewModel = null)
        {
            // Set the view model
            CurrentPageViewModel = viewModel;

            // set the current page
            CurrentPage = page;

            // Force property changed event
            OnPropertyChanged(nameof(CurrentPage));

        }

        #endregion

    }
}
