using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace metering.core
{

    /// <summary>
    /// Miscellaneous settings ViewModel.
    /// </summary>
    public class SettingsViewModel : BaseViewModel
    {

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SettingsViewModel()
        {
            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
        }

        #endregion

        #region Public Properties

        #endregion

        #region Public Commands

        /// <summary>
        /// Handles Omicron Hardware Configuration Settings
        /// </summary>
        /// <returns>Returns new Hardware Configuration</returns>
        public async Task HardwareConfiguration()
        {
            await IoC.Task.Run(()=> IoC.Logger.Log($"{nameof(HardwareConfiguration)} started."));
        }
        #endregion
    }
}
