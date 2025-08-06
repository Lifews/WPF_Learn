using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ComboBox_BindToEnum;

/// <summary>
/// 这段代码定义了值转换器（Value Converter）。
/// 它的主要功能是将枚举值（Enum） 转换为其对应的描述文本（Description），通常用于在UI界面中显示更友好的枚举值名称。
/// </summary>
public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return DependencyProperty.UnsetValue;

        return GetEnumDescription(value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return string.Empty;
    }

    private string GetEnumDescription(object enumObj)
    {
        var fi = enumObj.GetType().GetField(enumObj.ToString());

        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Description;
        else
            return enumObj.ToString();
    }
}