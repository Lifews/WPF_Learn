// 源代码：https://github.com/CommunityToolkit/dotnet

#if false
toolkit自带的ValueChangedMessage用法
以及如何用toolkit实现自定义Message类型的消息传递

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

var rec = new ViewModel();
var sdr = new ViewModel2();
WeakReferenceMessenger.Default.Send(new StringMessage("Hello, world!"));
sdr.Number = 10;
sdr.Number = 20;
sdr.Number = 30;
sdr.Number = 40;

record StringMessage(string Message);

public class ViewModel
{
    public ViewModel()
    {
        WeakReferenceMessenger.Default.Register<StringMessage>(this, Receive);
        WeakReferenceMessenger.Default.Register<ValueChangedMessage<int>>(this, Receive2);
    }
    void Receive(object recipient, StringMessage m)
    {
        Console.WriteLine($"string message received: \n{m.Message}");
    }
    void Receive2(object recipient, ValueChangedMessage<int> m)
    {
        Console.WriteLine($"New Number value received: \n{m.Value}");
    }
}

public class ViewModel2 : ObservableObject
{
    int number;
    public int Number
    {
        get { return number; }
        set
        {
            if (SetProperty(ref number, value))
            {
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<int>(value));
            }
        }
    }
}
#endif



#if false
⭐有条件尽量直接用ObservableRecipient。真有特殊需求再考虑接口
官方定义如下：
ObservableRecipient 类型旨在用作视图模型的基础，
这些模型还使用 IMessenger 功能，因为它对其提供内置支持。 具体而言：

1、它有一个无参数构造函数和一个采用 IMessenger 实例的构造函数，用于依赖关系注入。
   它还公开一个 Messenger 属性，该属性可用于在视图模型中发送和接收消息。
   如果使用无参数构造函数，WeakReferenceMessenger.Default 实例将分配给 Messenger 属性。
2、它公开 IsActive 属性来激活/停用视图模型。
   在此上下文中，“激活”意味着给定视图模型被标记为正在使用中，这样它将开始侦听已注册的消息、执行其他设置操作等。
   属性更改值时，调用了两个相关方法 - OnActivated 和 OnDeactivated。
   默认情况下，OnDeactivated 会自动从所有已注册的消息中注销当前实例。
   为了获得最佳结果并避免内存泄漏，建议使用 OnActivated 注册到消息，并使用 OnDeactivated 执行清理操作。
   此模式支持多次启用/禁用视图模型，同时在每次停用时可以安全地收集，没有内存泄漏的风险。
   默认情况下，OnActivated 会自动注册通过 IRecipient<TMessage> 接口定义的所有消息处理程序。
3、它公开一个 Broadcast<T>(T, T, string) 方法，该方法通过 Messenger 属性提供的 IMessenger 实例发送 PropertyChangedMessage<T> 消息。
   这可用于轻松广播视图模型属性中的更改，而无需手动检索要使用的 Messenger 实例。
   此方法由各种 SetProperty 方法的重载使用，这些方法具有额外的 bool broadcast 属性来指示是否也发送消息。
#endif

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;

var rec = new ReceiverViewModel();
rec.IsActive = true;//它公开 IsActive 属性来激活/停用视图模型。
var sdr = new SenderViewModel();
rec.IsActive = false;

public class ReceiverViewModel : ObservableRecipient, IRecipient<RequestMessage<string>>
{
    public void Receive(RequestMessage<string> message)
    {
        Console.WriteLine($"Request Message Received");
        message.Reply("Over, sir!");
    }
}

public class SenderViewModel : ObservableObject
{
    public SenderViewModel()
    {
        var res = WeakReferenceMessenger.Default.Send(new RequestMessage<string>());//如果没激活视图模型会报错
        Console.WriteLine("Response Received");
        Console.WriteLine(res.Response);//如果有多个订阅者，且这些订阅者都reply了，也会报错
    }
}