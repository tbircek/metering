using System.Collections.ObjectModel;

namespace metering.core
{
    /// <summary>
    /// Design time data for a <see cref="TestFileListItemViewModel"/>
    /// </summary>
    public class TestFileItemDesignModel : TestFileListItemViewModel
    {
        #region Singleton       

        /// <summary>
        /// Single instance of the design time model
        /// </summary>
        public static TestFileItemDesignModel Instance => new TestFileItemDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public TestFileItemDesignModel()
        {
            IsDeletable = true;
            TestToolTip = "Design time TestToolTip. This should show long version of file name and test status.";
            TestDeleteToolTip = "Design time TestDeleteToolTip.";
            TestStepBackgroundColor = "Transparent";
            ShortTestFileName = "ShortDesign-time filename";
        }

        #endregion
    }
}
