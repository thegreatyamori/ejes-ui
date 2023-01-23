using System;
using System.Windows.Data;

namespace EjesUI.Helpers
{
    public class StringToDoubleConverter: IValueConverter
    {
        private string _strCache = string.Empty;
        private double _dCache;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (_dCache == (double)value)
                return _strCache;
            if ((double)value == 0)
                return string.Empty;
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            _strCache = (string)value;
            double defaultZero = 0;
            if (double.TryParse(_strCache, out defaultZero))
                _dCache = double.Parse(_strCache);
            return _dCache;
        }
    }
}
