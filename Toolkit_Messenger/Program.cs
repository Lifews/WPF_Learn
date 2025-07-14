
// 源代码：https://github.com/CommunityToolkit/dotnet

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