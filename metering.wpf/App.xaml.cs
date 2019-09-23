using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using metering.core;
using Squirrel;

namespace metering
{
    /// <summary>
    /// Interaction logic for WpfApp.xaml
    /// </summary>
    public partial class WpfApp : Application
    {

        #region Private Variables

        /// <summary>
        /// holds squirrel update manager instance to dispose appropriately
        /// </summary>
        private static Task<UpdateManager> _updateManager = null;

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks for the updates if there is one updates application for next start up
        /// </summary>
        /// <returns></returns>
        private async Task CheckForUpdates()
        {
            // specify the location of update
            using (UpdateManager _updateManager = new UpdateManager(@"\\volta\Eng_Lab\Software Updates\metering"))
            {
                // if update location contains update for this application
                if (_updateManager.IsInstalledApp)
                    // update this application
                    await _updateManager.UpdateApp();
            }
        }

        /// <summary>
        /// Configures Dependency Injection for this application
        /// </summary>
        private void ApplicationSetup()
        {
            // Setup IoC
            IoC.Setup();

            // Bind a IUIManager
            // IoC.Kernel.Bind<IUIManager>().ToConstant(new UIManager());
        }

        #endregion

        #region WpfApp startup

        /// <summary>
        /// Main entry point for this application
        /// </summary>
        static WpfApp()
        {
            // This code is used to test the app when using other cultures.
            //
            //System.Threading.Thread.CurrentThread.CurrentUICulture =
            //    System.Threading.Thread.CurrentThread.CurrentUICulture =
            //    new System.Globalization.CultureInfo ( "it-IT" );

            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            //
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

        } 

        #endregion

        #region Protected Methods

        /// <summary>
        /// Override Startup event to show default view and view model.
        /// </summary>
        /// <param name="e">start up event arguments</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // Allow the base start
            base.OnStartup(e);

            // Setup the main application
            ApplicationSetup();

            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();

            // check for the updates
            IoC.Task.Run(async () =>
            {
                // await for application update
                await CheckForUpdates();
            });
        }

        /// <summary>
        /// Overrides application Exit event to dispose update manager and logs information to the logger
        /// </summary>
        /// <param name="e">exit event arguments</param>
        protected override void OnExit(ExitEventArgs e)
        {
            // dispose updateManager appropriately
            _updateManager?.Dispose();

            // log application exits message
            IoC.Logger.Log("Exiting the application", LogLevel.Informative);

            // end application
            base.OnExit(e);
        }

        #endregion

    }
}
