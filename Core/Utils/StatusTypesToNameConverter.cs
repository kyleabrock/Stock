using System;
using System.Globalization;
using System.Windows.Data;

namespace Core.Utils
{
    [ValueConversion(typeof(StatusTypes), typeof(string))]
    public class StatusTypesToNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((StatusTypes)value)
            {
                case StatusTypes.AtStock:
                    return "На складе";
                case StatusTypes.InWork:
                    return "В работе";
                case StatusTypes.Waiting:
                    return "Отложено";
                case StatusTypes.Discarded:
                    return "Списано";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
