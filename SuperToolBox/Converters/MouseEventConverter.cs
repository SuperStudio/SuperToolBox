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
    public class MouseButtonConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return 0;
            Enum.TryParse(value.ToString(), out MouseButton type);
            return (int)type + 1;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return MouseButton.None;
            int.TryParse(value.ToString(), out int val);
            return (MouseButton)(val - 1);
        }
    }
}
