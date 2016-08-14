using System;
using System.Globalization;
using System.Windows.Data;
using Stock.Core;

namespace Stock.UI.Utils
{
    [ValueConversion(typeof(Genders), typeof(string))]
    public class GendersToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Genders)value)
            {
                case Genders.Unknown:
                    return "Themes/Genders/Unknown.png";
                case Genders.Male:
                    return "Themes/Genders/Male.png";
                case Genders.Female:
                    return "Themes/Genders/Female.png";
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
