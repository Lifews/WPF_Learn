using System.Windows.Data;
using System.Windows;

namespace Toolkit_ObservableValidator.Converters;

internal class StringToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// <see langword="如何写注释？看看下面的例子"/><br/>
    /// This is a converter function which converts <see langword="string?"/> into a <see cref="Visibility"/>
    /// </summary>
    /// <remarks>
    /// <para>
    /// 这是一个转换器，用于判断一个 <see langword="string?"/> 类型的参数是否为空，<br/>
    /// 如果参数为空，那么返回值为<see cref="Visibility.Visible"/>，如果参数不为空，返回值为<see cref="Visibility.Collapsed"/>
    /// </para>
    /// </remarks>
    /// <example>
    /// 在XAML中使用该转换器，示例如下：
    /// <code lang="XAML">
    /// <![CDATA[
    /// <Windows.Resources>
    ///     <local:StringToVisibilityConverter x:Key="StringToVisibilityConverter"/>
    /// </Windows.Resources>
    /// <TextBlock Text="{Binding ErrorMessage}" Visibility="{Binding ErrorMessage, Converter={StaticResource StringToVisibilityConverter}}" />
    /// ]]>
    /// </code>
    /// </example>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        return string.IsNullOrEmpty(value as string) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        // Not needed
        throw new NotImplementedException();
    }
}
