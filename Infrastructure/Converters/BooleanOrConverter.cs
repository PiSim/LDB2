using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Infrastructure.Converters
{
    public class BooleanOrConverter : IMultiValueConverter
    {
        #region Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Any(b => (bool)b);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}