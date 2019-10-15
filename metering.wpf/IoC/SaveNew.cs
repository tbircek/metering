using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using metering.core;
using Microsoft.Win32;

namespace metering
{
    /// <summary>
    /// saves the test step to the user specified location.
    /// </summary>
    public class SaveNewManager : ICommandManager
    {
        #region Constructor

        public SaveNewManager() { }

        /// <summary>
        /// normalizes a path based on the current operating system
        /// </summary>
        /// <param name="path">the path to normalize</param>
        /// <returns></returns>
        public string NormalizePath(string path)
        {
            // if on Windows
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                // replace any / with \\ 
                return path?.Replace('/', '\\').Trim();
            // if on Linux/Mac
            else
                // replace any \\ with /
                return path?.Replace('\\', '/').Trim();

        }


        /// <summary>
        /// resolves any relative elements of the path to absolute
        /// </summary>
        /// <param name="path">the path to resolve</param>
        /// <returns></returns>
        public string ResolvePath(string path)
        {
            // resolve the path
            return Path.GetFullPath(path);
        }


        #endregion

        public Task<string> SaveNewTestAsync()
        {
            // Configure save file dialog box
            SaveFileDialog dlg = new SaveFileDialog
            {
                // Default file name
                FileName = "NewMeteringTest",
                // Default file extension
                DefaultExt = ".bmtf",
                // Filter files by extension
                Filter = "Beckwith metering test files (.bmtf)|*.bmtf",
                // automatically add an extension to a file name
                AddExtension = true,
                // sets the initial directory that is displayed by a file dialog.
                InitialDirectory = IoC.CMCControl.TestsFolder,
                // sets the text that appears in the title bar of a file dialog.
                Title = "Save your test step...",
            };

            // check if the Results directory exists... 
            if (!Directory.Exists(IoC.CMCControl.ResultsFolder))
                // if not create the Result directory...
                Directory.CreateDirectory(IoC.CMCControl.ResultsFolder);

            // check if the InitialDirectory exists... 
            if (!Directory.Exists(dlg.InitialDirectory))
                // if not create the InitialDirectory...
                Directory.CreateDirectory(dlg.InitialDirectory);

            // add the list of custom places for file dialog boxes.
            dlg.CustomPlaces.Add(
                new FileDialogCustomPlace(IoC.CMCControl.ResultsFolder)
                );

            // add the list of custom places for file dialog boxes.
            dlg.CustomPlaces.Add(
                new FileDialogCustomPlace(IoC.CMCControl.TestsFolder)
                );

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
                // return the user specified file name
                return Task.FromResult(dlg.FileName);
            else
                // the user canceled the task
                return Task.FromResult(string.Empty);
        }

    }
}
