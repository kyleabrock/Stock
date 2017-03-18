using System;
using System.Globalization;
using System.Windows.Data;
using Stock.Core;

namespace Stock.UI.Utils
{
    [ValueConversion(typeof(StatusTypes), typeof(string))]
    public class StatusTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((StatusTypes)value)
            {
                case StatusTypes.AtStock:
                    return "#2D95BF";
                case StatusTypes.InWork:
                    return "#F0C419";
                case StatusTypes.Waiting:
                    return "#404040";
                case StatusTypes.Discarded:
                    return "#4EBA6F";
                default:
                    return null;
            }

            //return Enum.GetValues(typeof (UserAcc.Genders), value) + ".jpg";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
