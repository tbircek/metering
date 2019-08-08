using System;
using System.Windows;
using System.Windows.Controls;

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
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void CurrentPagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newPage = (d as TestHost).CurrentPage;
            
        }


        #endregion

        #region Constructor

        /// <summary>
        /// Costructor
        /// </summary>
        public TestHost()
        {
            InitializeComponent();
        } 

        #endregion
    }
}
