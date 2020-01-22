using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace metering.core
{
    /// <summary>
    /// a view model for each analog signal in the SettingsPage
    /// </summary>
    public class TestFileListItemViewModel : BaseViewModel
    {
        #region Private Properties

        #endregion

        #region Public Properties

        /// <summary>
        /// Holds visibility information of "X" delete button of the control.
        /// </summary>
        public bool IsDeletable { get; set; } = true;

        /// <summary>
        /// Holds background color information of the control.
        /// </summary>
        /// <value>
        /// Background colors:
        /// Test is completed: DarkBlue
        /// Test is running: DarkSlateBlue
        /// Test is enqueue: Transparent
        /// </value>
        public string TestStepBackgroundColor { get; set; } = "Transparent";
        
        /// <summary>
        /// Holds file name information of a test.
        /// </summary>
        public string ShortTestFileName { get; set; } = "DesignTime Short";

        /// <summary>
        /// Holds long version file name and test progress status.
        /// </summary>
        public string TestToolTip { get; set; } = "File name and location, Test is enqueue/completed/running";

        /// <summary>
        /// Holds tool tip information of the "X" delete button of the control.
        /// </summary>
        public string TestDeleteToolTip { get; set; } = "Remove from multi-test step";
        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the user selected test and shows details.
        /// </summary>
        public ICommand LoadTestStepCommand { get; set; }

        /// <summary>
        /// Removes the user selected test from the multi-test scheme.
        /// </summary>
        public ICommand DeleteTestStepCommand { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// default constructor
        /// </summary>
        public TestFileListItemViewModel()
        {

            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

        }

        #endregion

        #region Public Method

        #endregion
    }
}
