using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using metering.core;

namespace metering
{
    /// <summary>
    /// Interaction logic for TestHost.xaml
    /// </summary>
    public partial class TestHost : UserControl
    {
        #region Dependency Properties

        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public ApplicationPage CurrentPage
        {
            get => (ApplicationPage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), 
                typeof(ApplicationPage), typeof(TestHost), 
                new UIPropertyMetadata(default(ApplicationPage), null, CurrentPagePropertyChanged));


        /// <summary>
        /// The current page to show in the page host
        /// </summary>
        public BaseViewModel CurrentPageViewModel
        {
            get => (BaseViewModel)GetValue(CurrentPageViewModelProperty);
            set => SetValue(CurrentPageViewModelProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPageViewModel"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageViewModelProperty =
            DependencyProperty.Register(nameof(CurrentPageViewModel), 
                typeof(BaseViewModel), typeof(TestHost), 
                new UIPropertyMetadata());

        #endregion

        #region Constructor

        /// <summary>
        /// Costructor
        /// </summary>
        public TestHost()
        {
            InitializeComponent();

            // Show the current page in design mode
            // otherwise design view would not show the current page in design mode.
            if (DesignerProperties.GetIsInDesignMode(this))
                NewTestHost.Content = (BasePage)new ApplicationPageValueConverter().Convert(IoC.Get<ApplicationViewModel>().CurrentPage);

        }

        #endregion

        #region Property Changed Events

        /// <summary>
        /// Called when the <see cref="CurrentPage"/> value has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static object CurrentPagePropertyChanged(DependencyObject d, object value)
        {
            // get current values
            var currentPage = (ApplicationPage)d.GetValue(CurrentPageProperty);
            var currentPageViewModel = d.GetValue(CurrentPageViewModelProperty);

            // get the Content Controls
            var newPageHost = (d as TestHost).NewTestHost;
            var oldPageHost = (d as TestHost).OldTestHost;

            // store the current page content as the old page
            var oldPageContent = newPageHost.Content;

            // Remove current page from new page host
            newPageHost.Content = null;

            // move the previous host into the old page host
            oldPageHost.Content = oldPageContent;
                        
            // remove old page
            Application.Current.Dispatcher.Invoke(() => oldPageHost.Content = null);

            // set the new page content
            newPageHost.Content = new ApplicationPageValueConverter().Convert(currentPage, null, currentPageViewModel);

            return value;
        }


        #endregion        
    }
}
