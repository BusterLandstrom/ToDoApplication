using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ToDoApplication.Converters
{
    public class ColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 0)
            {
                // Assuming the first value is the color value
                string colorValue = values[0] as string;

                if (!string.IsNullOrEmpty(colorValue))
                {
                    // Convert color string to SolidColorBrush
                    return new SolidColorBrush((Color)System.Windows.Media.ColorConverter.ConvertFromString(colorValue));
                }
            }

            // Return default color if values are not valid
            return Brushes.White; // Or any other default color
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
