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
        public async Task ShowFileDialogAsync (FileDialogOption option)
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
            else if(Equals(option, FileDialogOption.Open))
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
                // generate a new Test Steps Logger
                IoC.Task.Run(() => new TestStepsLogger(
                    // file location and name as specified by the user
                    filePath: dlg.FileName,
                    // don't need to save time
                    logTime: false,
                    // test details need to be saved
                    test: IoC.TestDetails
                    ));
            }
            // handles "OpenFileDialog"
            // TODO: this code should handle multiple selection as well.
            else if (Equals(dlg.Tag, FileDialogOption.Open))
            {
                // initialize a new TestDetailsViewModel
                TestDetailsViewModel test = new TestDetailsViewModel();

                // de-serialize a JSON file to a TestDetailsViewModel to show it the user 
                using (StreamReader file = File.OpenText(dlg.FileName))
                {
                    // initialize JsonSerializer to de-serialize directly from the file
                    JsonSerializer serializer = new JsonSerializer();

                    // convert a de-serialize json to TestDetailsViewModel
                    test = (TestDetailsViewModel)serializer.Deserialize(file, typeof(TestDetailsViewModel));

                    // Update values in the single instance of TestDetailsViewModel
                    // update AnalogSignals
                    IoC.TestDetails.AnalogSignals = test.AnalogSignals;

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

                    // Show TestDetails page
                    IoC.Application.GoToPage(ApplicationPage.TestDetails, IoC.TestDetails);
                }
            }
        }

        #endregion

    }
}
