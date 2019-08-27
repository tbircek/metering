
using System.ComponentModel;
using System.Windows.Controls;
using metering.core;

namespace metering.Controls
{
    /// <summary>
    /// The base class for content inside <see cref="TestDetailsPage"/>
    /// </summary>
    public class BaseUserControl: UserControl
    {
        #region Private Members

        /// <summary>
        /// The control
        /// </summary>
        private TestDetailsPage mUserControl;

        #endregion

        #region Public Properties


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseUserControl()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                mUserControl = new TestDetailsPage
                {
                    ViewModel = new TestDetailsViewModel()
                };
            }
        }

        #endregion
    }
}
