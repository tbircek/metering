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
        // public Task<ObservableCollection<AnalogSignalListItemViewModel>> AnalogSignals { get; set; }

        ///// <summary>
        ///// Passes Nominal values specified by the user to TestDetailsViewModel
        ///// </summary>
        ///// <param name="AnalogSignals">The viewModel values to transfer to</param>
        //public Task PassNominalValues(ObservableCollection<AnalogSignalListItemViewModel> AnalogSignals)
        //{
        //    // Create a task to await
        //    var tcs = new TaskCompletionSource<bool>();

        //    Application.Current.Dispatcher.Invoke(() =>
        //    {
        //        try
        //        {
        //            MessageBox.Show("TEst");
        //            AnalogSignals = new ObservableCollection<AnalogSignalListItemViewModel>
        //            {
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "v1",
        //                    From = "100.4",
        //                    To = "134.6",
        //                    Delta = "4.333",
        //                    Phase = "0.000",
        //                    Frequency = "59.999"
        //                },
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "v2",
        //                    From = "100.4",
        //                    To = "134.6",
        //                    Delta = "4.333",
        //                    Phase = "0.000",
        //                    Frequency = "59.999"
        //                },
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "v3",
        //                    From = "100.4",
        //                    To = "134.6",
        //                    Delta = "4.333",
        //                    Phase = "0.000",
        //                    Frequency = "59.999"
        //                },
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "v4",
        //                    From = "100.4",
        //                    To = "134.6",
        //                    Delta = "4.333",
        //                    Phase = "0.000",
        //                    Frequency = "59.999"
        //                },
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "i1",
        //                    From = "40",
        //                    To = "50",
        //                    Delta = "0.010",
        //                    Phase = "0.000",
        //                    Frequency = "59.999"
        //                },
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "i2",
        //                    From = "40",
        //                    To = "50",
        //                    Delta = "0.010",
        //                    Phase = "-120.000",
        //                    Frequency = "59.999"
        //                },
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "i3",
        //                    From = "40",
        //                    To = "50",
        //                    Delta = "0.010",
        //                    Phase = "120.000",
        //                    Frequency = "59.999"
        //                },
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "i4",
        //                    From = "40",
        //                    To = "50",
        //                    Delta = "0.010",
        //                    Phase = "0.000",
        //                    Frequency = "59.999"
        //                },
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "i5",
        //                    From = "40",
        //                    To = "50",
        //                    Delta = "0.010",
        //                    Phase = "-120.000",
        //                    Frequency = "59.999"
        //                },
        //                new AnalogSignalListItemViewModel
        //                {
        //                    SignalName = "i6",
        //                    From = "40",
        //                    To = "50",
        //                    Delta = "0.010",
        //                    Phase = "120.000",
        //                    Frequency = "59.999"
        //                },
        //            };
        //        }
        //        finally
        //        {
        //            tcs.TrySetResult(true);
        //        }

                  
        //    });

        //    return tcs.Task;
        //    // return Task.Run(() => MessageBox.Show("Test"));
        //}

        public void PassNominalValues(ObservableCollection<AnalogSignalListItemViewModel> destination)
        {
            throw new System.NotImplementedException();
        }
    }
}
