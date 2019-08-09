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
        public BasePage CurrentPage
        {
            get => (BasePage)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        /// <summary>
        /// Registers <see cref="CurrentPage"/> as a dependency property
        /// </summary>
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(nameof(CurrentPage), typeof(BasePage), typeof(TestHost), new UIPropertyMetadata(CurrentPagePropertyChanged));

        #endregion

        #region Property Changed Events

        /// <summary>
        /// Called when the <see cref="CurrentPage"/> value has changed
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
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
            newPageHost.Content = e.NewValue;
        }


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
    }
}
