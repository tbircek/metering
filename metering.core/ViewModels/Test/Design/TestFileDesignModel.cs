using System.Collections.ObjectModel;

namespace metering.core
{
    /// <summary>
    /// Design time data for a <see cref="TestFileListItemViewModel"/>
    /// </summary>
    public class TestFileDesignModel : TestFileViewModel
    {
        #region Singleton       

        /// <summary>
        /// Single instance of the design time model
        /// </summary>
        public static TestFileDesignModel Instance => new TestFileDesignModel();

        #endregion

        #region Constructor

        /// <summary>
        /// default constructor
        /// </summary>
        public TestFileDesignModel()
        {
            TestFileListItems = new ObservableCollection<TestFileListItemViewModel>
            {
                new TestFileListItemViewModel
                {
                  IsDeletable = false,
                  TestToolTip = "Design time TestToolTip. This should show long version of file name and test status.",
                  TestDeleteToolTip = "Design time TestDeleteToolTip.",
                  TestStepBackgroundColor = "DarkBlue",
                  ShortTestFileName = "PhaseTest",
                },
                new TestFileListItemViewModel
                {
                  IsDeletable = false,
                  TestToolTip = "Design time TestToolTip. This should show long version of file name and test status.",
                  TestDeleteToolTip = "Design time TestDeleteToolTip.",
                  TestStepBackgroundColor = "DarkBlue",
                  ShortTestFileName = "FrequencyTest",
                },
                new TestFileListItemViewModel
                {
                  IsDeletable = false,
                  TestToolTip = "Design time TestToolTip. This should show long version of file name and test status.",
                  TestDeleteToolTip = "Design time TestDeleteToolTip.",
                  TestStepBackgroundColor = "DarkSlateBlue",
                  ShortTestFileName = "VoltageTest",
                },
                new TestFileListItemViewModel
                {
                  IsDeletable = true,
                  TestToolTip = "Design time TestToolTip. This should show long version of file name and test status.",
                  TestDeleteToolTip = "Design time TestDeleteToolTip.",
                  TestStepBackgroundColor = "Transparent",
                  ShortTestFileName = "CurrentTest",
                },

                new TestFileListItemViewModel
                {
                  IsDeletable = true,
                  TestToolTip = "Design time TestToolTip. This should show long version of file name and test status.",
                  TestDeleteToolTip = "Design time TestDeleteToolTip.",
                  TestStepBackgroundColor = "Transparent",
                  ShortTestFileName = "2ndHarmonicsTest",
                },

                new TestFileListItemViewModel
                {
                  IsDeletable = true,
                  TestToolTip = "Design time TestToolTip. This should show long version of file name and test status.",
                  TestDeleteToolTip = "Design time TestDeleteToolTip.",
                  TestStepBackgroundColor = "Transparent",
                  ShortTestFileName = "3rdHarmonicsTest",
                },

            };
        }

        #endregion
    }
}
