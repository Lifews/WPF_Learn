using System.ComponentModel;

namespace ComboBox_BindToEnum;

public enum DateOfWeek
{
    //微软官方定义有DayOfWeek枚举,此处为了演示，使用自定义枚举

    [Description("星期日")]
    Sunday,

    [Description("星期一")]
    Monday,

    [Description("星期二")]
    Tuesday,

    [Description("星期三")]
    Wednesday,

    [Description("星期四")]
    Thursday,

    [Description("星期五")]
    Friday,

    [Description("星期六")]
    Saturday
}
