using System.Collections.ObjectModel;

namespace metering.core
{
    /// <summary>
    /// a viewmodel for  analog signals in the TestDetailsPage
    /// </summary>
    public class AnalogSignalViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// Omicron Analog Output Signals.
        /// </summary>
        public ObservableCollection<AnalogSignalListItemViewModel> AnalogSignals { get; set; }
        
        #endregion

    }
}
