using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using metering.core;

namespace metering
{
    /// <summary>
    /// The implementation of the <see cref="IVMContentManager"/>
    /// </summary>
    public class IContentManager : IVMContentManager
    {
        public Task ShowTestDetails(TestDetailsViewModel viewModel)
        {
            return Task.Run(()=> MessageBox.Show("Test message from TestDetailsViewModel only"));
        }

        public Task ShowTestDetails(TestDetailsViewModel testDetailsViewModel, NominalValuesViewModel nominalValuesViewModel)
        {
            return Task.Run(() => MessageBox.Show("Test message from TestDetailsViewModel & NominalValuesViewModel"));

        }
    }
}
