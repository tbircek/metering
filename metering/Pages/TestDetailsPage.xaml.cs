using System;
using System.Threading.Tasks;
using metering.core;

namespace metering
{
    /// <summary>
    /// Interaction logic for TestDetailsPage.xaml
    /// </summary>
    public partial class TestDetailsPage: BasePage<TestDetailsViewModel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public TestDetailsPage(): base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with a view model
        /// </summary>
        /// <param name="specificViewModel">The specific view model to use for the page</param>
        public TestDetailsPage(TestDetailsViewModel specificViewModel): base(specificViewModel)
        {
            InitializeComponent();             
        }
    }
}
