using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
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
        /// Holds full path of the test file. 
        /// </summary>
        public string FullFileName { get; set; } = string.Empty;

        /// <summary>
        /// Holds visibility information of "X" delete button of the control.
        /// </summary>
        public bool IsDeletable { get; set; } = true;

        /// <summary>
        /// Holds background color information of the control.
        /// </summary>
        /// <value>
        /// Background colors:
        /// Test status is completed: DarkBlue
        /// Test status is running: DarkSlateBlue
        /// Test status is enqueue: Transparent
        /// Test status is interrupted: DimGray
        /// Test status is unknown: DarkSlateGray
        /// </value>
        public string TestStepBackgroundColor { get; set; } = "Transparent";

        /// <summary>
        /// Holds file name information of a test.
        /// </summary>
        public string ShortTestFileName { get; set; } = "DesignTime Short";

        /// <summary>
        /// Holds file name information with file extension of a test.
        /// </summary>
        public string TestFileNameWithExtension { get; set; } = "DesignTime FileNameWithExtension";

        /// <summary>
        /// Holds long version file name and test progress status.
        /// </summary>
        public string TestToolTip { get; set; } = "File name and location, Test is enqueue/completed/running/interrupted/unknown";

        /// <summary>
        /// Holds tool tip information of the "X" delete button of the control.
        /// </summary>
        public string TestDeleteToolTip { get; set; } = "Remove from multi-test step";
        #endregion

        #region Public Commands

        /// <summary>
        /// Gets the user selected test and shows details.
        /// </summary>
        public ICommand ShowTestStepCommand { get; set; }

        /// <summary>
        /// Removes the user selected test from the multi-test scheme.
        /// </summary>
        public ICommand RemoveTestStepCommand { get; set; }

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

            // initialize commands
            RemoveTestStepCommand = new RelayParameterizedCommand(async (parameter) => await RemoveTestStepAsync(parameter));
            ShowTestStepCommand = new RelayParameterizedCommand(async (parameter) => await ShowTestStepAsync(parameter));
        }

        #endregion

        #region Public Method

        #endregion


        #region Private Methods

        /// <summary>
        /// Shows the user selected file's <see cref="TestDetailsViewModel"/>
        /// </summary>
        /// <param name="parameter"><see cref="FullFileName"/> of the currently selected <see cref="TestFileListItemViewModel"/></param>
        /// <returns>Returns no value</returns>
        private async Task ShowTestStepAsync(object parameter)
        {
            // generate new awaitable task
            var showTestStepTask = IoC.Task.Run(() =>
            {
                // scan the collection to generate the new collection
                foreach (TestFileListItemViewModel testFileListItem in IoC.Communication.TestFileListItems)
                {
                    // is this item match?
                    if (Equals(testFileListItem.FullFileName, parameter))
                    {
                        // update view to first file
                        IoC.Task.Run(() => IoC.Commander.LoadTestFile(IoC.Communication.TestFileListItems.IndexOf(testFileListItem)));
                        // exit this loop
                        break;
                    }
                }
            });

            // wait for the task to completed.
            await showTestStepTask;
        }

        /// <summary>
        /// Removes the user selected file's <see cref="TestDetailsViewModel"/> from the <see cref="TestFileViewModel"/>
        /// </summary>
        /// <param name="parameter"><see cref="FullFileName"/> of the currently selected <see cref="TestFileListItemViewModel"/></param>
        /// <returns>Returns no value</returns>
        private async Task RemoveTestStepAsync(object parameter)
        {
            // generate a new collection to hold the collection with the user removed collection
            ObservableCollection<TestFileListItemViewModel> newTestFileListItemViewModels = new ObservableCollection<TestFileListItemViewModel>() { };

            // generate new awaitable task
            var removeTestStepTask = IoC.Task.Run(() =>
                {
                    // scan the collection to generate the new collection
                    foreach (TestFileListItemViewModel testFileListItem in IoC.Communication.TestFileListItems)
                    {
                        // is this item match?
                        if (Equals(testFileListItem.FullFileName, parameter))
                        {
                            // this item removed. do not add to new collection and skip next item in the old collection.
                            continue;
                        }

                        // this item is not removed. add it to the new collection.
                        newTestFileListItemViewModels.Add(testFileListItem);
                    }

                    //// is the first item removed?
                    //if (!IoC.Communication.TestFileListItems[0].Equals(newTestFileListItemViewModels[0]))
                    //{
                    // update the collection with removed test file collection
                    IoC.Communication.TestFileListItems = newTestFileListItemViewModels;
                    // always update view to first file
                    IoC.Task.Run(() => IoC.Commander.LoadTestFile(0));
                    //}

                    //// the first item is not removed so just update the collection
                    //// but don't reload first item.
                    //IoC.Communication.TestFileListItems = newTestFileListItemViewModels;
                });

            // wait for the task to completed.
            await removeTestStepTask;
        }

        #endregion
    }
}
