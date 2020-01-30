using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;
using metering.core;
using metering.core.Resources;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace metering
{
    /// <summary>
    /// saves the test step to the user specified location.
    /// </summary>
    public class SaveNewManager : ICommandManager
    {

        #region Private Properties

        /// <summary>
        /// a generic <see cref="FileDialog"/> that will change per the user selection to 
        /// either <see cref="SaveFileDialog"/> or <see cref="OpenFileDialog"/>
        /// </summary>
        private FileDialog Dlg { get; set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the user non-saved test file.
        /// </summary>
        /// <returns>Returns no value.</returns>
        private async Task LoadDummyTestFileAsync(string currentFile)
        {

            await IoC.Task.Run(() => 
            {
                // store file names to TestFileListItemViewModel
                // initialize a new test file list
                TestFileListItemViewModel testFile = new TestFileListItemViewModel
                {
                    // since this is the first loading...
                    IsDeletable = false, // so the user have chance to fix their mistakes and such...
                    ShortTestFileName = Path.GetFileNameWithoutExtension(currentFile), // the file name only...
                    TestDeleteToolTip = $"{Strings.tooltips_remove_file}",
                    TestStepBackgroundColor = $"{Strings.color_test_enqueued}",
                    TestToolTip = $"{Path.GetFileName(currentFile)}.{Environment.NewLine}{Strings.test_status_enqueued}",
                    FullFileName = $"{currentFile}",
                };

                // add the new test file to the multi-test list.
                IoC.Communication.TestFileListItems.Add(testFile);

                // always show multiple test user interface.
                IoC.Communication.IsMultipleTest = true; // (IoC.Communication.TestFileListItems.Count > 0) ? true : false;

                // fill TestDetails view with first file == 0
                LoadTestFile(0);

            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves a non-saved test details view model as a test file.
        /// </summary>
        /// <returns>Returns no value.</returns>
        public async Task SaveDummyTestFileAsync()
        {
            // make a test file name.
            string safeFileName = Strings.temp_test_file_name;
            // update TestFileName
            IoC.TestDetails.TestFileName = safeFileName;

            // generate a new Test Steps Logger
            var fileOkTask =
                IoC.Task.Run(() => new TestStepsLogger(
                    // file location and name as specified by the user
                    filePath: Path.Combine(IoC.CMCControl.TestsFolder, safeFileName),
                    // don't need to save time
                    logTime: false,
                    // test details need to be saved
                    test: IoC.TestDetails
                    ));

            // save non-user saved file
            await fileOkTask;

            // load non-user saved file
            await LoadDummyTestFileAsync(Path.Combine(IoC.CMCControl.TestsFolder, safeFileName));
        }

        /// <summary>
        /// Loads multiple tests in order
        /// </summary>
        public void LoadTestFile(int testFileNumber)
        {
            // initialize multiple test view model.
            TestFileListItemViewModel currentTestFile = new TestFileListItemViewModel();
            // retrieve the first test
            currentTestFile = IoC.Communication.TestFileListItems[testFileNumber];

            // convert a JSON file to a TestDetailsViewModel to show it the user.
            using (StreamReader file = File.OpenText(currentTestFile.FullFileName))
            {
                // initialize a new TestDetailsViewModel
                TestDetailsViewModel test = new TestDetailsViewModel();

                // initialize JsonSerializer
                JsonSerializer serializer = new JsonSerializer();

                // convert the JsonSerializer to TestDetailsViewModel
                test = (TestDetailsViewModel)serializer.Deserialize(file, typeof(TestDetailsViewModel));

                // Update values in the single instance of TestDetailsViewModel
                // update AnalogSignals
                IoC.TestDetails.AnalogSignals = test.AnalogSignals;

                // Select Ramping Signal property
                // Ramping Signal property is Magnitude. 
                // This is also default setting for this property.
                IoC.TestDetails.IsMagnitude = string.Equals(test.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Magnitude)) || string.IsNullOrWhiteSpace(test.SelectedRampingSignal);
                // Ramping Signal property is Phase.
                IoC.TestDetails.IsPhase = string.Equals(test.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Phase));
                // Ramping Signal property is Frequency.
                IoC.TestDetails.IsFrequency = string.Equals(test.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Frequency));
                // Ramping Signal property is Harmonics.
                IoC.TestDetails.IsHarmonics = string.Equals(test.SelectedRampingSignal, nameof(TestDetailsViewModel.RampingSignals.Harmonics));

                // update Harmonics Order
                IoC.TestDetails.HarmonicsOrder = test.HarmonicsOrder;

                // update Register
                IoC.TestDetails.Register = test.Register;

                // update DwellTime
                IoC.TestDetails.DwellTime = test.DwellTime;

                // update StartDelayTime
                IoC.TestDetails.StartDelayTime = test.StartDelayTime;

                // update MeasurementInterval
                IoC.TestDetails.MeasurementInterval = test.MeasurementInterval;

                // update StartMeasurementDelay
                IoC.TestDetails.StartMeasurementDelay = test.StartMeasurementDelay;

                // update SelectedRampingSignal
                IoC.TestDetails.SelectedRampingSignal = test.SelectedRampingSignal;

                // update Link Ramping Signals status
                // If SelectedRampingSignal == "Frequency" or If SelectedRampingSignal == "Harmonics"
                if (Equals(nameof(TestDetailsViewModel.RampingSignals.Frequency), test.SelectedRampingSignal) || Equals(nameof(TestDetailsViewModel.RampingSignals.Harmonics), test.SelectedRampingSignal))
                {
                    // frequencies are linked.
                    IoC.TestDetails.IsLinked = test.IsLinked;
                }
                else
                {
                    // frequencies are not linked
                    IoC.TestDetails.IsLinked = false;
                }

                // update Hardware Configuration = Voltage
                IoC.TestDetails.SelectedVoltageConfiguration = test.SelectedVoltageConfiguration;

                // update Hardware Configuration = Current
                IoC.TestDetails.SelectedCurrentConfiguration = test.SelectedCurrentConfiguration;

                // update TestFileName
                IoC.TestDetails.TestFileName = test.TestFileName;

                // update Settings view model
                IoC.Settings.SelectedCurrent = test.SelectedCurrentConfiguration.WiringDiagramString;
                IoC.Settings.SelectedVoltage = test.SelectedVoltageConfiguration.WiringDiagramString;

                // if the test file is old,
                // add a new fileName attribute,
                // add some missing attributes to "SelectedVoltageConfiguration",
                // add some missing attributes to "SelectedCurrentConfiguration".
                if (string.IsNullOrWhiteSpace(IoC.TestDetails.TestFileName))
                {
                    // add saved file name
                    IoC.TestDetails.TestFileName = Path.GetFileName(currentTestFile.FullFileName);

                    // add new "WiringDiagramFileLocation"
                    test.SelectedVoltageConfiguration.WiringDiagramFileLocation = "../Images/Omicron/not used voltage.png";
                    test.SelectedCurrentConfiguration.WiringDiagramFileLocation = "../Images/Omicron/not used current.png";

                    // add new SelectedCurrent and SelectedVoltage
                }

                // update Settings view model
                IoC.Settings.CurrentDiagramLocation = test.SelectedCurrentConfiguration.WiringDiagramFileLocation;
                IoC.Settings.VoltageDiagramLocation = test.SelectedVoltageConfiguration.WiringDiagramFileLocation;

                // change CancelForegroundColor to Red
                IoC.Commands.CancelForegroundColor = "ff0000";

                // set Command buttons
                IoC.Commands.StartTestAvailable = true;
                IoC.Commands.NewTestAvailable = false;
                IoC.Commands.Cancellation = true;
                IoC.Commands.ConfigurationAvailable = true;
                IoC.Commands.IsConfigurationAvailable = false;

                // Show TestDetails page
                IoC.Application.GoToPage(ApplicationPage.TestDetails, IoC.TestDetails);
            }

            // dispose dialog box
            Dlg = null;

            // dispose temp multiple test view model.
            currentTestFile = null;

        }

        /// <summary>
        /// Shows a <see cref="SaveFileDialog"/> or <see cref="OpenFileDialog"/> per the user selection.
        /// </summary>
        /// <param name="option"></param>
        public async Task ShowFileDialogAsync(FileDialogOption option)
        {
            // check if the Results directory exists... 
            if (!Directory.Exists(IoC.CMCControl.ResultsFolder))
                // if not create the Result directory...
                Directory.CreateDirectory(IoC.CMCControl.ResultsFolder);

            // if the user wants to save test file...
            if (Equals(option, FileDialogOption.Save))
            {
                // Configure save file dialog box
                Dlg = new SaveFileDialog
                {
                    // Default file name
                    FileName = "NewMeteringTest",
                    // sets the text that appears in the title bar of a file dialog.
                    Title = "Save your test step...",
                };
            }
            // if the user wants to open test file(s)...
            else if (Equals(option, FileDialogOption.Open))
            {
                // Configure open file dialog box
                Dlg = new OpenFileDialog
                {
                    // Default file name
                    FileName = "",
                    // sets the text that appears in the title bar of a file dialog.
                    Title = "Please select test file(s)...",
                    // allow the users to select multiple files
                    Multiselect = true,
                };
            }

            // associate with the correct dialog.
            Dlg.Tag = option;

            // Default file extension
            Dlg.DefaultExt = ".bmtf";

            // Filter files by extension
            Dlg.Filter = "Beckwith metering test files (.bmtf)|*.bmtf";

            // automatically add an extension to a file name
            Dlg.AddExtension = true;

            // sets the initial directory that is displayed by a file dialog.
            Dlg.InitialDirectory = IoC.CMCControl.TestsFolder;

            // check if the InitialDirectory exists... 
            if (!Directory.Exists(Dlg.InitialDirectory))
                // if not create the InitialDirectory...
                Directory.CreateDirectory(Dlg.InitialDirectory);

            // add the list of custom places for file dialog boxes.
            Dlg.CustomPlaces.Add(
                new FileDialogCustomPlace(IoC.CMCControl.ResultsFolder)
                );

            // add the list of custom places for file dialog boxes.
            Dlg.CustomPlaces.Add(
                new FileDialogCustomPlace(IoC.CMCControl.TestsFolder)
                );

            // hook up to the dialog event
            Dlg.FileOk += Dlg_FileOk;

            // Show save file dialog box
            await Dispatcher.CurrentDispatcher.BeginInvoke(() => Dlg.ShowDialog());
        }

        #endregion

        #region Private Helper

        /// <summary>
        /// Occurs when the user selects a file name by either clicking the Open button of the OpenFileDialog 
        /// or the Save button of the SaveFileDialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dlg_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Sender is a FileDialog
            var dlg = sender as FileDialog;

            // handles "SaveFileDialog"
            if (Equals(dlg.Tag, FileDialogOption.Save))
            {
                // update TestFileName
                IoC.TestDetails.TestFileName = dlg.SafeFileName;

                // generate a new Test Steps Logger
                var fileOkTask =
                    IoC.Task.Run(() => new TestStepsLogger(
                        // file location and name as specified by the user
                        filePath: dlg.FileName,
                        // don't need to save time
                        logTime: false,
                        // test details need to be saved
                        test: IoC.TestDetails
                        ));

                // saving completed successfully.
                if (TaskStatus.RanToCompletion == fileOkTask.Status)
                {

                    // dispose dialog box
                    dlg = null;
                }
                // saving the file failed.
                else if (TaskStatus.Faulted == fileOkTask.Status)
                {
                    // inform the developer about error
                    IoC.Logger.Log(fileOkTask.Exception.GetBaseException().Message);

                    // update the user about the error.
                    IoC.Communication.Log = fileOkTask.Exception.GetBaseException().Message;
                }
            }
            // handles "OpenFileDialog"
            else if (Equals(dlg.Tag, FileDialogOption.Open))
            {
                // grab all the file name(s) selected by the user.
                foreach (var currentFile in dlg.FileNames)
                {
                    // store file names to TestFileListItemViewModel
                    // initialize a new test file list
                    TestFileListItemViewModel testFile = new TestFileListItemViewModel
                    {
                        // since this is the first loading...
                        IsDeletable = true, // so the user have chance to fix their mistakes and such...
                        ShortTestFileName = Path.GetFileNameWithoutExtension(currentFile), // the file name only...
                        TestDeleteToolTip = $"{Strings.tooltips_remove_file}",
                        TestStepBackgroundColor = $"{Strings.color_test_enqueued}",
                        TestToolTip = $"{Path.GetFileName(currentFile)}.{Environment.NewLine}{Strings.test_status_enqueued}",
                        FullFileName = $"{currentFile}",
                    };

                    // add the new test file to the multi-test list.
                    IoC.Communication.TestFileListItems.Add(testFile);
                }

                // always show multiple test user interface.
                IoC.Communication.IsMultipleTest = true; // (IoC.Communication.TestFileListItems.Count > 0) ? true : false;

                // clear previous test values.
                IoC.TestDetails.AnalogSignals.Clear();

                // dispose dialog box
                dlg = null;

                // fill TestDetails view with first file == 0
                LoadTestFile(0);
            }
        }

        #endregion

    }
}
