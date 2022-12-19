using SuperToolBox.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static SuperUtils.Framework.Hooks.HookManager;

namespace SuperToolBox.Converters
{
    public class MouseActionConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return 0;
            Enum.TryParse(value.ToString(), out MouseAction type);
            return (int)type;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return MouseAction.None;
            int.TryParse(value.ToString(), out int val);
            return (MouseAction)val;
        }
    }
}
