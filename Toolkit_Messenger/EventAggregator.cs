
// 若不使用Toolkit，应按照下面的方法实现EventAggregator

#if false  
基础写法，参考下面写法
var rec = new MessageReceiver();
class MessageReceiver
{
    // 构造函数快捷ctor
    public MessageReceiver()
    {
        EventAggregator.Instance.Register<StringMessage>(this, Receiver1);
        EventAggregator.Instance.Register<NumberMessage>(this, Receiver2);
    }
    void Receiver1(StringMessage m)
    {
        Console.WriteLine($"string message received: {m.Message}");
    }
    void Receiver2(NumberMessage m)
    {
        Console.WriteLine($"string message received: {m.Number}");
    }
}
#endif


// 顶级语句必须位于类型声明和空间声明之前
var rec1 = new StringMessageReceiver();
var rec2 = new NumberMessageReceiver();


EventAggregator.Instance.Send(new StringMessage("Hello,world!"));
EventAggregator.Instance.Send(new StringMessage("labubu"));
EventAggregator.Instance.Send(new NumberMessage(28));



// 此处使用抽象类不太合适，应该是一个接口
abstract class MessageReceiver<TMessage>
{
    protected MessageReceiver()
    {
        EventAggregator.Instance.Register<TMessage>(this, Receive);
    }
    public abstract void Receive(TMessage m);
}


// 写成了接口形式，但感觉还是不够简洁，先不去研究了，这不是目前的重点
public interface IMessageReceiver<TMessage>
{
    void Receive(TMessage m);
}
public static class MessageReceiverExtensions
{
    public static void RegisterWithEventAggregator<TMessage>(this IMessageReceiver<TMessage> receiver)
    {
        EventAggregator.Instance.Register<TMessage>(receiver, receiver.Receive);
    }
}


class StringMessageReceiver : IMessageReceiver<StringMessage>
{
    public StringMessageReceiver()
    {
        this.RegisterWithEventAggregator<StringMessage>();
    }
    public void Receive(StringMessage m)
    {
        Console.WriteLine($"string message received: {m.Message}");
    }
}

class NumberMessageReceiver : IMessageReceiver<NumberMessage>
{
    public NumberMessageReceiver()
    {
        this.RegisterWithEventAggregator<NumberMessage>();
    }
    public void Receive(NumberMessage m)
    {
        Console.WriteLine($"string message received: {m.Number}");
    }
}






#if false
等价于以下代码
public sealed record StringMessage
{
    public string Message { get; init; }  // 不可变属性，初始化后不可改变

    // 构造函数（位置参数）
    public StringMessage(string Message)
    {
        this.Message = Message;
    }

    // 编译器自动生成：
    // - 基于值的 Equals() 和 GetHashCode()
    // - 包含所有属性的 ToString()
    // - 解构方法 Deconstruct(out string Message)
    // - 支持 with 表达式的克隆方法
}
#endif
// 一种使用 位置记录（Positional Record） 的声明方式。这是一种简洁的语法糖，编译器会自动生成完整的类结构。
record StringMessage(string Message);
record NumberMessage(int Number);
class EventAggregator
{
    // 单例实例
    public static EventAggregator Instance { get; } = new();

    // 在 C# 中，类内部是可以定义其他类（class）或记录类型（record）的。这种结构称为嵌套类型（Nested Type）
    // 订阅者信息记录
    record MessageReceiver(object Receiver, Action<object> Method);

    // 消息路由表：消息类型 → 订阅者列表
    private Dictionary<Type, List<MessageReceiver>> events = new();

    // 注册订阅
    public void Register<TMessage>(object receiver, Action<TMessage> method)
    {
        var type = typeof(TMessage);
        //events[typeof(TMessage)]：从字典中获取该消息类型对应的订阅者列表，一个Type对应一个List<MessageReceiver>
        if (!events.ContainsKey(type))
            events[type] = new List<MessageReceiver>();
        events[type].Add(new(receiver, o => method((TMessage)o)));//o => method((TMessage)o))，Lambda 表达式
    }

    // 发送消息
    public void Send<TMessage>(TMessage message)
    {
        var type = typeof(TMessage);
        if (!events.ContainsKey(type))
            return;
        foreach (var rec in  events[type])
        {
            rec.Method.Invoke(message);
        }
    }
}
