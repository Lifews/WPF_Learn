using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ComboBox_binding;

public partial class MainViewModel : ObservableObject
{
    // 使用源生成器简化属性
    [ObservableProperty]
    private UserRole _selectedRole = UserRole.RegularUser;

    // 使用只读属性初始化枚举选项
    public ObservableCollection<KeyValuePair<UserRole, string>> Roles { get; }
        = new(Enum.GetValues<UserRole>()
            .Select(e => new KeyValuePair<UserRole, string>(
                e,
                e.GetAttribute<DescriptionAttribute>()?.Description ?? e.ToString()
            )));
}

// 扩展方法获取特性
public static class EnumExtensions
{
    public static T? GetAttribute<T>(this Enum value) where T : Attribute
    {
        var field = value.GetType().GetField(value.ToString());
        return field?.GetCustomAttributes(typeof(T), false)?.FirstOrDefault() as T;
    }
}