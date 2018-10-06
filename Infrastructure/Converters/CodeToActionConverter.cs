using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Infrastructure.Converters
{
    public class CodeToActionConverter : IValueConverter
    {
        #region Fields

        private readonly Dictionary<string, string> _converterDictionary = new Dictionary<string, string>()
                                                                {
                                                                    {"A", "Arrivato" },
                                                                    {"F", "Finito" },
                                                                    {"B", "Buttato" },
                                                                    {"C", "Portato in Cotex" },
                                                                    {"R", "Ritornato da Cotex" }
                                                                };

        #endregion Fields

        #region Constructors

        public CodeToActionConverter()
        {
        }

        #endregion Constructors

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _converterDictionary[(string)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}