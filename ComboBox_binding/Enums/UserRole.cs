using System.ComponentModel;

namespace ComboBox_binding;

public enum UserRole
{
    // 微软自带有一个 DayOfWeek 枚举，但为了演示目的，这里使用自定义枚举

    [Description("管理员")]
    Administrator,

    [Description("普通用户")]
    RegularUser,

    [Description("访客")]
    Guest,

    [Description("审核员")]
    Moderator
}
