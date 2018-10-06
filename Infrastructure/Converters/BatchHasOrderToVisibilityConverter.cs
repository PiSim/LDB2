using LabDbContext;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Infrastructure.Converters
{
    public class BatchHasOrderToVisibilityConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!string.IsNullOrWhiteSpace((value as Batch).OrderFilePath))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}