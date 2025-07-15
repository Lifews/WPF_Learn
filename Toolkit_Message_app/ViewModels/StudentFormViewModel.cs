using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Toolkit_Message_app.Models;

namespace Toolkit_Message_app.ViewModels;

public partial class StudentFormViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<bool>>
{
    bool allowNew;



#if false
string?  这里的问号?表示这个字符串变量是可空的（nullable）。
在C# 8.0及更高版本中，引入了可为空引用类型（nullable reference types）的特性。
这个特性允许开发者明确表示一个引用类型是否可以包含null值。
string   是一个不可为空的字符串类型，意味着这个变量应该始终包含一个字符串实例，不能为null。
string?  表示一个可为空的字符串类型，意味着这个变量可以包含一个字符串实例，也可以是null。
#endif
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    string? name;
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    string? _class;
    [ObservableProperty]
    [NotifyPropertyChangedRecipients]
    string? phone;

    public string SqlQuery() =>
    $"INSERT INTO Students VALUES ('{Name}', '{Class}', '{Phone}')";


    [RelayCommand(CanExecute = nameof(CanAddNew))]
    void AddNew()
    {
        WeakReferenceMessenger.Default.Send(
                    new ValueChangedMessage<Student>(new(Name, Class, Phone))
                );
    }
    bool CanAddNew => allowNew;


    public void Receive(ValueChangedMessage<bool> message)
    {
        allowNew = message.Value;
        AddNewCommand.NotifyCanExecuteChanged();
    }
}
