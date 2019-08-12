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
        /// Passes Nominal values specified by the user to TestDetailsViewModel
        /// </summary>
        /// <param name="destination">The viewModel values to transfer to</param>
        /// <returns></returns>
        void PassNominalValues(ObservableCollection<AnalogSignalListItemViewModel> destination);

    }
}
