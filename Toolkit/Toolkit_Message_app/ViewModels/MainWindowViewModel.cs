using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Toolkit_Message_app.ViewModels;


#if false
简述一下PropertyChangedMessage和ValueChangedMessage的区别
1、PropertyChangedMessage<T>继承自ValueChangedMessage<T>
2、使用PropertyChangedMessage<T>：需要明确属性来源时，用于通知特定对象的特定属性发生了值变化（例如MVVM中ViewModel的属性变更）。
   使用ValueChangedMessage<T>：只需广播值的变化本身（例如全局状态更新），无需关联具体属性。
#endif

public partial class MainWindowViewModel : ObservableRecipient, IRecipient<PropertyChangedMessage<string>>
{
    // 写这里的时候用了 vs的自动生成代码
    // 把StudentFormViewModel StudentFormViewModel写成了StudentFormViewModel studentFormViewModel
    // 导致前端绑定时，一直绑定不上......这里跟着敲的时候一定要注意思考，这里的StudentFormViewModel是属性
    // 还有这个 IsActive记得要打开，需要激活视图模型
    public StudentFormViewModel StudentFormViewModel { get;} = new StudentFormViewModel() { IsActive = true };
    public StudentListViewModel StudentListViewModel { get;} = new StudentListViewModel() { IsActive = true };
    public MainWindowViewModel()
    {
        IsActive = true;
    }

    [ObservableProperty]
    string information = "INSERT INTO Students ...";

    public void Receive(PropertyChangedMessage<string> message)
    {
        Information = StudentFormViewModel.SqlQuery();
    }
}
