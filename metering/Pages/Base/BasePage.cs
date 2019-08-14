using System.Threading.Tasks;
using System.Windows;
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
        public BasePage() // :base()
        {
            ViewModel = new VM();
        }

        #endregion

        //#region Public Methods
        
        ///// <summary>
        ///// Displays TestDetail page to the user
        ///// </summary>
        ///// <typeparam name="T">The view model type</typeparam>
        ///// <param name="viewModel">The viewmodel</param>
        ///// <returns></returns>
        //public Task ShowTest<T>(T viewModel)
        //    where T: TestDetailsViewModel
        //{
        //    // Create a task to wait until user cancels or test is over.
        //    // TODO: add a task token to cancel the this task
        //    var tcs = new TaskCompletionSource<bool>();

        //    // Keep view on the UI Thread
        //    Application.Current.Dispatcher.Invoke(() =>
        //    {
        //        try
        //        {
        //            DataContext = viewModel;
        //        }
        //        finally
        //        {
        //            // test completed (with/without error)
        //            tcs.TrySetResult(true);
        //        }
        //    });

        //    // return test completion.
        //    return tcs.Task;
        //}
        //#endregion
    }
}
