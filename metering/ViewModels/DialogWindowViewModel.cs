
using System.Windows;
using System.Windows.Controls;

namespace metering
{
    /// <summary>
    ///The view model for the test details
    /// </summary>
   public class DialogWindowViewModel: WindowViewModel
    {
        #region Public Properties

        /// <summary>
        /// The title of this dialog
        /// </summary> 
        public string Title { get; set; }

        /// <summary>
        /// The content to host inside dialog
        /// </summary>
        public Control Content { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DialogWindowViewModel(Window window): base(window) { }

        #endregion
    }
}
