using System.Windows.Controls;
using metering.core;

namespace metering
{
    /// <summary>
    /// A base page for all pages
    /// </summary>
    public class BasePage : UserControl
    {

        // Animation and other stuff here
    }


    /// <summary>
    /// A base page with added ViewModel support
    /// </summary>
    public class BasePage<VM> : BasePage
        where VM: BaseViewModel, new()
    {

        #region Private Member

        /// <summary>
        /// View Model associated with this page
        /// </summary>
        private VM mViewModel;

        #endregion

        #region Public Properties

        /// <summary>
        /// View Model associated with this page
        /// </summary>
        public VM ViewModel
        {
            get => mViewModel;
            set
            {
                // nothing changed return
                if (mViewModel == value)
                    return;

                // update value
                mViewModel = value;

                // set data context for the page
                DataContext = mViewModel;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage() :base()
        {            
            // Create a default view model
            ViewModel = IoC.Get<VM>();
        }

        /// <summary>
        /// the constructor with specific view model
        /// </summary>
        /// <param name="specificViewModel">The view model to use if exists</param>
        public BasePage(VM specificViewModel = null) : base()
        {
            // set the specific view model
            if (specificViewModel != null)
            {
                ViewModel = specificViewModel;
            }
            else
            {
                // Create a default view model
                ViewModel = IoC.Get<VM>();
            }
        }

        #endregion
    }
}
