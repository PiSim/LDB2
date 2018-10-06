using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Infrastructure.Converters
{
    public class CalibrationDueDateToStatusLightConverter : IValueConverter
    {
        #region Constructors

        public CalibrationDueDateToStatusLightConverter()
        {
        }

        #endregion Constructors

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dueDate = (DateTime)value;

            if (DateTime.Now > dueDate)
                return Brushes.Red;
            else if (DateTime.Now.AddDays(45) > dueDate)
                return Brushes.Yellow;
            else
                return Brushes.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}