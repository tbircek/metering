using System.Windows;
using System.Windows.Controls;

namespace metering
{
    /// <summary>
    /// The NoFrameHistory attached property for creating a <see cref="Frame"/> that hides navigation
    /// and keeps the navigation history empty
    /// </summary>
    public class NoFrameHistory: BaseAttachedProperty<NoFrameHistory, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var frame = (sender as Frame);

            // hide navigation bar
            frame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;

            // clear nagivation history
            frame.Navigated += (ss, ee) => ((Frame)ss).NavigationService.RemoveBackEntry();
        }
    }
}
