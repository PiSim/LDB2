using System;
using System.Windows.Data;

namespace Infrastructure.Converters
{
    public class BooleanNegationConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException();

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(value,
                            targetType,
                            parameter,
                            culture);
        }

        #endregion Methods
    }
}