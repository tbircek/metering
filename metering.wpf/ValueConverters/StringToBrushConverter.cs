
using System;
using System.Globalization;
using System.Windows.Media;

namespace metering
{
    /// <summary>
    /// Converts a string value to a <see cref="ForegroundColor"/>
    /// </summary>
    public class StringToBrushConverter : BaseValueConverter<StringToBrushConverter>
    {        
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFromString($"#{value}");  // .ConvertFrom($"#{value}"));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
