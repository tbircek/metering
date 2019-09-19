using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading;
using System.Windows.Input;

namespace metering.core
{
    /// <summary>
    /// a view model for  analog signals in the TestDetailsPage
    /// </summary>
    public class AnalogSignalViewModel : BaseViewModel
    {
        #region Public Properties

        /// <summary>
        /// indicates if the current text double left clicked to highlight the text
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Omicron Analog Output Signals.
        /// </summary>
        public ObservableCollection<AnalogSignalListItemViewModel> AnalogSignals { get; set; }

        #endregion

        #region Public Commands

        /// <summary>
        /// Selects all text on left clicked text box.
        /// </summary>
        public ICommand SelectAllTextCommand { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AnalogSignalViewModel()
        {

            // make aware of culture of the computer
            // in case this software turns to something else.
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;

            // create command
            SelectAllTextCommand = new RelayCommand(SelectAll);
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Selects all text on the text box
        /// </summary>
        public void SelectAll()
        {
            // simulate property change briefly to select all text in the text box
            // as selecting all text should be last until the user leaves the control or types something

            // Sets FocusAndSelectProperty to true
            Selected = true;

            // Sets FocusAndSelectProperty to false
            Selected = false;
        }

        #endregion
    }
}
