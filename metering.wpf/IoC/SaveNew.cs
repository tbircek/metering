using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;
using metering.core;
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

        #region Public Method

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
            // Dlg.ShowDialog();
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
            // TODO: this code should handle multiple selection as well.
            else if (Equals(dlg.Tag, FileDialogOption.Open))
            {
                // convert a JSON file to a TestDetailsViewModel to show it the user 
                using (StreamReader file = File.OpenText(dlg.FileName))
                {
                    // initialize a new TestDetailsViewModel
                    TestDetailsViewModel test = new TestDetailsViewModel();

                    // initialize JsonSerializer
                    JsonSerializer serializer = new JsonSerializer();

                    // convert the JsonSerializer to TestDetailsViewModel
                    test = (TestDetailsViewModel)serializer.Deserialize(file, typeof(TestDetailsViewModel));

                    // clear previous test values.
                    IoC.TestDetails.AnalogSignals.Clear();

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
                    // If SelectedRampingSignal == "Frequency"
                    if (Equals(nameof(TestDetailsViewModel.RampingSignals.Frequency), test.SelectedRampingSignal))
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
                        IoC.TestDetails.TestFileName = dlg.SafeFileName;

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

                    // dispose the JsonSerializer
                    serializer = null;

                    // dispose test
                    test = null;

                    // dispose dialog box
                    dlg = null;
                }
            }

        }

        #endregion

    }
}
