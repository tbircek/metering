using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace metering
{
    /// <summary>
    /// Focuses (keyboard) to this element on load
    /// </summary>
    public class IsFocusedProperty : BaseAttachedProperty<IsFocusedProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // is it a control?
            if (!(sender is Control control))
                return;

            // focus the control
            control.Loaded += (s, se) => control.Focus();
        }
    }

    /// <summary>
    /// Focuses (keyboard) to this element if true
    /// </summary>
    public class FocusProperty : BaseAttachedProperty<FocusProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // is it a control?
            if (!(sender is Control control))
                return;

            // focus the control
            if ((bool)e.NewValue)
                control.Focus();
        }
    }


    /// <summary>
    /// Focuses (keyboard) to this element and selects all text if this element is a textbox
    /// </summary>
    public class FocusAndSelectProperty : BaseAttachedProperty<FocusAndSelectProperty, bool>
    {

        /// <summary>
        /// Handles LeftDoubleClick property value changes
        /// </summary>
        /// <param name="sender">a text box control that the user left double clicked</param>
        /// <param name="e">value of the state of left double click</param>
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // is it a text box control?
            // if not do nothing
            if (sender is TextBoxBase control)
            {                
                // the control has focus and has a new value that is true
                if ((bool)e.NewValue && control.IsFocused)
                {
                    // control has focus already.
                    // select all text 
                    control.SelectAll();
                }
                else
                {
                    // else do nothing since the control has no focus or new value is false.
                }
            }
        }
    }
}
