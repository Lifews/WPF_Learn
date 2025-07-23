using System.Windows.Markup;

namespace Serial_Learn;

/// <summary>
/// 这段代码定义了一个名为 EnumBindingSourceExtension 的 WPF 标记扩展（Markup Extension），
/// 主要用于在 XAML 中为枚举类型（Enum）生成可绑定的数据源。
/// 它的核心功能是将枚举值转换为可在 UI 控件（如 ComboBox）中直接绑定的集合，并支持可空枚举（Nullable Enum）。
/// </summary>
class EnumBindingSourceExtension : MarkupExtension
{
    private Type? _enumType;

    public Type? EnumType
    {
        get => _enumType;
        set
        {
            if (value != _enumType)
            {
                if (value != null)
                {
                    Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType.IsEnum)
                        throw new ArgumentException("Type must be for an Enum.");
                }

                _enumType = value;
            }
        }
    }

    public EnumBindingSourceExtension() { }

    public EnumBindingSourceExtension(Type enumType)
    {
        EnumType = enumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (_enumType == null)
            throw new InvalidOperationException("The EnumType must be specified.");

        var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
        var enumValues = Enum.GetValues(actualEnumType);

        if (actualEnumType == _enumType)
            return enumValues;

        var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
        enumValues.CopyTo(tempArray, 1);
        return tempArray;
    }
}
