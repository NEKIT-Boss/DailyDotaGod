using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DailyDotaGod.Views.Converters
{
    class ConnectionToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool connected = (bool)value;
            if (connected)
            {
                return '\uE701'.ToString();
            }
            else
            {
                return '\uEB5E'.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
