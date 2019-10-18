using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using Dna;
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

        ///// <summary>
        ///// holds squirrel update manager instance to dispose appropriately
        ///// </summary>
        //private static Task<UpdateManager> updateManager = null;

        #endregion

        #region Private Methods

        /// <summary>
        /// Checks for the updates if there is one updates application for next start up
        /// </summary>
        /// <returns>AsyncStateMachine of Squirrel</returns>
        private async Task CheckForUpdates()
        {
            // specify the location of update
            using (UpdateManager updateManager = new UpdateManager(@"\\volta\Eng_Lab\Software Updates\metering"))
            {
                // check if there is an update
                UpdateInfo updateInfo = await updateManager.CheckForUpdate();

                // prevent error in development computer
                if (updateInfo.CurrentlyInstalledVersion != null)
                    // log the current installed version of the application
                    IoC.Logger.Log($"Current version: {updateInfo.CurrentlyInstalledVersion.Version}", LogLevel.Informative);

                // if update location contains update for this application
                if (updateInfo.ReleasesToApply.Count > 0)
                {
                    // log the current installed version of the application
                    IoC.Logger.Log($"Update version: {updateInfo.FutureReleaseEntry.Version}", LogLevel.Informative);

                    // update this application
                    await updateManager.UpdateApp();

                }
                // no update available
                else
                {
                    // log application update message
                    IoC.Logger.Log($"No updates: Update version: {updateInfo.FutureReleaseEntry.Version}", LogLevel.Informative);
                }
            }
        }

        /// <summary>
        /// Configures Dependency Injection for this application
        /// </summary>
        private void ApplicationSetup()
        {
            // set up dna framework
            new DefaultFrameworkConstruction()
                .AddFileLogger()
                .Build();

            // Setup IoC
            IoC.Setup();

            // Bind the commander
           IoC.Kernel.Bind<ICommandManager>().ToConstant(new SaveNewManager());

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
        protected override async void OnStartup(StartupEventArgs e)
        {
            // Allow the base start
            base.OnStartup(e);

            // Setup the main application
            ApplicationSetup();

            // log application start message
            IoC.Logger.Log("Starting the application", LogLevel.Informative);

            // check for the updates
            await IoC.Task.Run(async () =>
            {
                // log application update message
                IoC.Logger.Log("Checking for updates", LogLevel.Informative);

                // await for application update
                await CheckForUpdates();
            });
            
            // Show the main window
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        /// <summary>
        /// Overrides application Exit event to dispose update manager and logs information to the logger
        /// </summary>
        /// <param name="e">exit event arguments</param>
        protected override void OnExit(ExitEventArgs e)
        {
            //// dispose updateManager appropriately
            //updateManager?.Dispose();

            // log application exits message
            IoC.Logger.Log("Exiting the application", LogLevel.Informative);

            // end application
            base.OnExit(e);
        }

        #endregion

    }
}
