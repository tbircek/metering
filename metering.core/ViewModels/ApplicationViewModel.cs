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
        /// Navigate to specified page
        /// </summary>
        /// <param name="page">The page to navigate</param>
        public void GoToPage(ApplicationPage page)
        {
            // set the current page
            CurrentPage = page;
        }

        #endregion

    }
}
