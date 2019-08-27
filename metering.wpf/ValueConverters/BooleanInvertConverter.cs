
using System;
using System.Globalization;

namespace metering
{
    /// <summary>
    /// Converts a boolean value to invert value
    /// </summary>
    public class BooleanInvertConverter: BaseValueConverter<BooleanInvertConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(bool)value;

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
