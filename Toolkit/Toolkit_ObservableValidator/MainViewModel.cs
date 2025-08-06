using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Toolkit_ObservableValidator.Properties;

namespace Toolkit_ObservableValidator;

public partial class MainViewModel : ObservableValidator
{
    // 这里利用.resx做界面本地化，多看几遍，留意一些规则
    [ObservableProperty]
    [Required(ErrorMessageResourceName = "UserName_Required", ErrorMessageResourceType = typeof(Language))]
    [MinLength(6, ErrorMessageResourceName = "UserName_MinLength", ErrorMessageResourceType = typeof(Language))]
    [MaxLength(20, ErrorMessage = "用户名长度不能大于20位")]
    string? userName;

    [ObservableProperty]
    [Required(ErrorMessage = "邮件不能为空")]
    [EmailAddress]
    string? email;

    [ObservableProperty]
    [Required(ErrorMessage = "年龄不能为空")]
    [Range(18, 150)]
    int? age;
    partial void OnAgeChanged(int? value)
    {
        //前端校验
        ValidateProperty(value, nameof(Age));
    }

#if false
    int? age;
    [Required]
    [Range(18, 150)]
    public int? Age
    {
        get { return age; }
        set
        {   
            //[ObservableProperty]特性手动实现如下
            //SetProperty的工作流程
            //值比较：检查新值value是否与当前字段 age相等
            //更新字段：若值不同，更新 age = value
            //发送通知：触发PropertyChanged事件，通知绑定到Age属性的UI元素更新
            SetProperty(ref age, value);
            //前端校验
            ValidateProperty(value, nameof(Age));
        }
    }
#endif

    [ObservableProperty]
    string? errorMessage;

    [RelayCommand]
    void Register()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            ErrorMessage = string.Join(Environment.NewLine, GetErrors());
            MessageBox.Show(                    // 1. 弹出消息框
                string.Join(                    // 2. 将字符串数组拼接成单个字符串
                Environment.NewLine,            // 3. 使用系统换行符作为分隔符
                GetErrors()                     // 4. 获取错误信息集合
                )
            );
            return;
        }
        ErrorMessage = "";
        MessageBox.Show("Register successful");
    }




}
