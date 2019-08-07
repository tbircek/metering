using System.Windows.Controls;
using metering.core;

namespace metering
{
    /// <summary>
    /// A base page for all pages
    /// </summary>
    public class BasePage<VM> : Page
        where VM: BaseViewModel, new()
    {

        #region Private Member

        /// <summary>
        /// View Model associated with this page
        /// </summary>
        private VM mViewModel;

        #endregion

        #region Public Properties

        ///// <summary>
        ///// Animation to play the page is first loaded.
        ///// </summary>
        //public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

        ///// <summary>
        ///// Animation to play the page is unloaded.
        ///// </summary>
        //public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToRight;

        ///// <summary>
        ///// Time for the animation
        ///// </summary>
        //public float SlideSeconds { get; set; } = 0.8f;

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
        public BasePage()
        {
            // Start animation while invisible
            //if (this.PageLoadAnimation != PageAnimation.None)
            //    Visibility = Visibility.Collapsed;


            //// listen page load.
            //this.Loaded += BasePage_Loaded;

            ViewModel = new VM();
        }

        #endregion

        #region Animation Load/Unload

        /// <summary>
        /// After page loaded perform animations 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        //{
        //   await AnimateIn();
        //}

        //public async Task AnimateIn()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
