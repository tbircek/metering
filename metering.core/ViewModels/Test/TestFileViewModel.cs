using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;

namespace metering.core
{
    /// <summary>
    /// a view model for each test file in the Multi-Test view
    /// </summary>
    public class TestFileViewModel : BaseViewModel
    {
        #region Private Properties

        #endregion

        #region Public Properties

        /// <summary>
        /// Multi-Test files.
        /// </summary>
        public ObservableCollection<TestFileListItemViewModel> TestFileListItems { get; set; }
        #endregion

        #region Public Commands

        #endregion

        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public TestFileViewModel()
        {

            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
        }

        #endregion

    }
}
