using System;
using System.Diagnostics;
using System.Globalization;
using metering.core;

namespace metering
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType = null, object parameter = null, CultureInfo culture = null)
        {
            // Find the appropriate page
            switch ((ApplicationPage)value)
            {
                // nominal values page
                case ApplicationPage.NominalValues:
                    return new NominalValuesPage();

                // test details page
                case ApplicationPage.TestDetails:
                    return new TestDetailsPage();

                // settings page
                case ApplicationPage.Settings:
                    return new SettingsViewPage(); 

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
