using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace metering
{
    /// <summary>
    /// A base value converter that allows direct XAML usage
    /// </summary>
    /// <typeparam name="T">The type of this value converter</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T: class, new()
    {

        #region Private Members
        /// <summary>
        /// statuc ubstance if this value converter
        /// </summary>
        private static T mConverter = null;

        #endregion

        #region Markup Extension Methods
        /// <summary>
        ///  Provides a static instance of the value converter
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // if mConverter is null return new mConverter
            return mConverter ?? (mConverter = new T());
        }

        #endregion 

        #region Value Converter Methods

        /// <summary>
        /// the method to convert one type to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// the method to convert a value back to it's source type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
