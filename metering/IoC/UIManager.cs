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
            return Task.Run(() => MessageBox.Show("Test"));
        }
    }
}
