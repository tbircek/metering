using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// The ViewModel manager that handles transferring content between viewmodels
    /// </summary>
    public interface IUIManager
    {

        /// <summary>
        /// Displays TestDetails page with user specified values in NominalValues page
        /// </summary>
        /// <param name="viewModel">The view model</param>
        /// <returns></returns>
        Task ShowTestDetails(TestDetailsViewModel viewModel);
    }
}
