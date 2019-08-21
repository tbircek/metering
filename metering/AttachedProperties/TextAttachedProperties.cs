using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace metering
{
    /// <summary>
    /// Focuses (keyboard) to this element on load
    /// </summary>
    public class IsFocusedProperty: BaseAttachedProperty<IsFocusedProperty, bool>        
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
    /// Focuses (keyboard) to this element and selects all text if true
    /// </summary>
    public class FocusAndSelectProperty : BaseAttachedProperty<FocusAndSelectProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // is it a control?
            if (!(sender is TextBoxBase control))
                return;
                        
            if ((bool)e.NewValue)
            {
                // focus the control
                control.Focus();

                // select all text 
                control.SelectAll();
            }
        }
    }
}
