using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using metering.core;

namespace metering
{
    /// <summary>
    /// The implementation of the <see cref="IUIManager"/>
    /// </summary>
    public class UIManager : IUIManager
    {

        /// <summary>
        /// Displays TestDetails page with user specified values in NominalValues page
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <returns></returns>
        public Task ShowTestDetails(TestDetailsViewModel viewModel)
        {          

            // create a task to wait in dialog closing
            var tcs = new TaskCompletionSource<bool>();

            // run on UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    // show test details
                    //TestDetailsPage testDetailsPage = new TestDetailsPage();
                    //testDetailsPage.DataContext = viewModel;

                    //AnalogSignalViewModel analogSignalViewModel = new AnalogSignalViewModel
                    //{
                    //    AnalogSignals = viewModel.AnalogSignals
                    //};

                    AnalogSignalListItemControl analogSignalListItemControl = new AnalogSignalListItemControl
                    {
                        DataContext = viewModel.AnalogSignals
                    };

                    // Show TestDetails page
                    Debug.WriteLine("CopyNominalValues() is running:");
                    Task.Factory.StartNew(() => IoC.Get<ApplicationViewModel>().GoToPage(ApplicationPage.TestDetails));
                }
                finally
                {
                    // Task completed
                    tcs.TrySetResult(true);
                }
            });

            return tcs.Task;
        }
    }
}
