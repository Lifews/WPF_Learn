using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System.Collections.ObjectModel;
using Toolkit_Message_app.Models;

namespace Toolkit_Message_app.ViewModels;

public partial class StudentListViewModel : ObservableRecipient, IRecipient<ValueChangedMessage<Student>>
{
    public ObservableCollection<Student> Students { get; } = new();


    [ObservableProperty]
    bool _allowNew;

    partial void OnAllowNewChanged(bool value)
    {
        WeakReferenceMessenger.Default.Send(new ValueChangedMessage<bool>(value));
    }

    public void Receive(ValueChangedMessage<Student> message)
    {
        Students.Add(message.Value);
    }


    #if false
     `??` 是null合并运算符。如果左边表达式为null，则返回右边的值（这里是0）。
     因此，这个属性的含义是：如果`Students`集合不为空，则返回集合中的元素个数；如果为空，则返回0。

     => 语法,这是表达式体属性(Expression-bodied property)，等效于：
     public int StudentCount
    {
        get { return Students?.Count ?? 0; }
    }
    #endif
    public int StudentCount => Students?.Count ?? 0;


    #if false
    Students.CollectionChanged, Students 是 ObservableCollection<T> 类型，当集合变化（增/删/清空）时触发此事件。
    (_, _) => ...    Lambda 表达式忽略事件参数（使用 _ 占位符）
    OnPropertyChanged(nameof(StudentCount)), 当集合变化时，手动触发 StudentCount 属性的变更通知
    #endif
    public StudentListViewModel()
    {
        Students.CollectionChanged += (_, _) => OnPropertyChanged(nameof(StudentCount));
    }
}
