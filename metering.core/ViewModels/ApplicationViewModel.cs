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
        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.NominalValues;

        #endregion

    }
}
