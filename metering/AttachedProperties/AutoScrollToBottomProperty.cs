using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// Scroll to bottom of the control to show the most up to date information
    /// </summary>
    public class AutoScrollToBottomProperty: BaseAttachedProperty<AutoScrollToBottomProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //// don't show if it in design mode
            //if (DesignerProperties.GetIsInDesignMode(sender))
            //    return;

            // verify sender is a ScrollViewer
            if (!(sender is ScrollViewer control))
                 return;

            // Remove older handle
            control.ScrollChanged -= Control_ScrollChanged;

            // Scroll content to bottom
            control.ScrollChanged += Control_ScrollChanged;
        }

        private void Control_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scroll = sender as ScrollViewer;

            // If it close to the bottom
            if (scroll.ScrollableHeight - scroll.VerticalOffset < 5)
                // Scroll to the bottom
                scroll.ScrollToEnd();
        }
    }
}
