using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace metering.core
{
    /// <summary>
    /// The ViewModel manager that handles transferring content between viewmodels
    /// </summary>
    public interface IVMContentManager
    {
        /// <summary>
        /// Displays TestDetails page with user specified values in NominalValues page
        /// </summary>
        /// <param name="testDetailsViewModel">The view model</param>
        /// <returns></returns>
        Task ShowTestDetails(TestDetailsViewModel testDetailsViewModel);

        /// <summary>
        /// Displays TestDetails page with user specified values in NominalValues page
        /// </summary>
        /// <param name="testDetailsViewModel">The test details viewmodel</param>
        /// <param name="nominalValuesViewModel">The nominal values viewmodel</param>
        /// <returns></returns>
        Task ShowTestDetails(TestDetailsViewModel testDetailsViewModel, NominalValuesViewModel nominalValuesViewModel);
    }
}
