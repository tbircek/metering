using System.Windows;
using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// Scroll to bottom of the control to show the most up to date information
    /// </summary>
    public class ScrollToBottomProperty: BaseAttachedProperty<ScrollToBottomProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // verify sender is a ScrollViewer
            if (!(sender is ScrollViewer control))
                 return;

            // Remove older handle
            control.DataContextChanged -= Control_DataContextChanged;

            // Scroll content to bottom
            control.DataContextChanged += Control_DataContextChanged;
        }

        private void Control_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // Scroll to bottom
            (sender as ScrollViewer).ScrollToBottom();
        }
    }
}
