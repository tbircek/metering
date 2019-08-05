using System.Windows.Controls;
using System.Windows;
using System;
using System.Threading.Tasks;

namespace metering
{
    /// <summary>
    /// A base page for all pages
    /// </summary>
    public class BasePage : Page
    {
        #region Public Properties

        /// <summary>
        /// Animation to play the page is first loaded.
        /// </summary>
        public PageAnimation PageLoadAnimation { get; set; } = PageAnimation.SlideAndFadeInFromRight;

        /// <summary>
        /// Animation to play the page is unloaded.
        /// </summary>
        public PageAnimation PageUnloadAnimation { get; set; } = PageAnimation.SlideAndFadeOutToRight;

        /// <summary>
        /// Time for the animation
        /// </summary>
        public float SlideSeconds { get; set; } = 0.8f;

        #endregion


        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public BasePage()
        {
            // Start animation while invisible
            if (this.PageLoadAnimation != PageAnimation.None)
                Visibility = Visibility.Collapsed;


            // listen page load.
            this.Loaded += BasePage_Loaded;
        }

        #endregion

        #region Animation Load/Unload

        /// <summary>
        /// After page loaded perform animations 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
           await AnimateIn();
        }

        public async Task AnimateIn()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
