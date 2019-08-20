
using System;
using System.Globalization;
using System.Windows;

namespace metering
{
    /// <summary>
    /// Converts a string value to a <see cref="ForegroundColor"/>
    /// </summary>
    public class StringToBrushConverter : BaseValueConverter<StringToBrushConverter>
    {        
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
            {
                return (bool)value ? Visibility.Hidden : Visibility.Visible;
            }
            else
            {
                return (bool)value ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
