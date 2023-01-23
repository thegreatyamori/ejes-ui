using System;
using System.Windows.Data;

namespace EjesUI.Helpers
{
    public class ComboBoxItemToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string itemType = "System.Windows.Controls.ComboBoxItem";
            string splitValue = (string)value;
            int index = splitValue.IndexOf(itemType);
            
            if (index >= 0)
                return splitValue.Split(":")[1];
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string itemType = "System.Windows.Controls.ComboBoxItem";

            return $"{itemType}: {value}";
        }
    }
}
