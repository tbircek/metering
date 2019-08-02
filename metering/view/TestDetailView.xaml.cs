using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// Interaction logic for TestDetailView.xaml
    /// </summary>
    public partial class TestDetailView : UserControl
    {
        public TestDetailView()
        {
            InitializeComponent();
            DataContext = new TestDetailList();

        }
    }

    public class TestDetailList : ObservableCollection<TestDetailViewModel>
    {
        public TestDetailList() : base()
        {
            Add(new TestDetailViewModel("v1", "100.0", "120.0", "2.345", "0", "59.999"));
            Add(new TestDetailViewModel("v2", "100.0", "120.0", "2.345", "-120", "59.999"));
            Add(new TestDetailViewModel("v3", "100.0", "120.0", "2.345", "120", "59.999"));
            Add(new TestDetailViewModel("v4", "100.0", "120.0", "2.345", "0", "59.999"));
            Add(new TestDetailViewModel("i1", "25.0", "35.0", "0.100", "0", "59.999"));
            Add(new TestDetailViewModel("i2", "25.0", "35.0", "0.100", "-120", "59.999"));
            Add(new TestDetailViewModel("i3", "25.0", "35.0", "0.100", "120", "59.999"));
            Add(new TestDetailViewModel("i4", "25.0", "35.0", "0.100", "0", "59.999"));
            Add(new TestDetailViewModel("i5", "25.0", "35.0", "0.100", "-120", "59.999"));
            Add(new TestDetailViewModel("i6", "25.0", "35.0", "0.100", "1200", "59.999"));
        }
    }
}
