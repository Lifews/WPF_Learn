using System.Windows.Data;
using System.Windows;

namespace Toolkit_ObservableValidator.Converters;

internal class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return string.IsNullOrEmpty(value as string) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        // Not needed
        throw new NotImplementedException();
    }
}
