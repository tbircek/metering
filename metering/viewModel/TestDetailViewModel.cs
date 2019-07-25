using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using metering.model;

namespace metering.viewModel
{
    public class TestDetailViewModel : ViewModelBase
    {
        private static TestDetailModel model = new TestDetailModel();

        public TestDetailViewModel()
        {

        }

        public TestDetailViewModel(string signalName, string from, string to, string delta, string phase, string frequency)
        {
            SignalName = signalName;
            From = from;
            To = to;
            Delta = delta;
            Phase = phase;
            Frequency = frequency;
        }

        public string SignalName
        {
            get => model.SignalName;
            set
            {
                if (SetProperty(model.SignalName, value))
                {
                    model.SignalName = value;
                }
            }
        }

        public string From
        {
            get => model.From;
            set
            {
                if (SetProperty(model.From, value))
                {
                    model.From = value;
                }
            }
        }
        
        public string To
        {
            get => model.To;
            set
            {
                if (SetProperty(model.To, value))
                {
                    model.To = value;
                }
            }
        }

        public string Delta
        {
            get => model.Delta;
            set
            {
                if (SetProperty(model.Delta, value))
                {
                    model.Delta = value;
                }
            }
        }
        
        public string Phase
        {
            get => model.Phase;
            set
            {
                if (SetProperty(model.Phase, value))
                {
                    model.Phase = value;
                }
            }
        }
        public string Frequency
        {
            get => model.Frequency;
            set
            {
                if (SetProperty(model.Frequency, value))
                {
                    model.Frequency = value;
                }
            }
        }
    }
}
