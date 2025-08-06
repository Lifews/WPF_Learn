using System;
using System.Windows;
using System.Windows.Input;

namespace INotifyPropertyChanged_
{
    #region 命令绑定,正常框架不会把自定义类写在vm中，这里只是learning，为了方便
    public class MyCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        Action executeAction;
        public MyCommand(Action action)
        {
            executeAction = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            executeAction();
        }
    }
    #endregion


// 老东西好久没手写属性了，重写一遍还是感觉学到不少
// 通知属性基础写法，继承INotifyPropertyChanged
    #if false
    public class MainViewModel : INotifyPropertyChanged
    {
        #region 事件
        //PropertyChangedEventHandler类型，在 .NET 源代码中，它的定义如下：
        //public delegate void PropertyChangedEventHandler(object? sender, PropertyChangedEventArgs e);
        //### 参数
        //1. **sender** (object?)：触发事件的对象，通常是实现了 INotifyPropertyChanged 接口的类的实例。
        //2. **e** (PropertyChangedEventArgs)：事件参数，包含属性变更的详细信息，其中最重要的属性是 `PropertyName`（表示发生变更的属性名称）。
        //### 使用场景
        //当某个类的属性值发生变化时，该类通过触发 PropertyChanged 事件来通知所有订阅者（如UI控件）该属性已更新。订阅者可以根据 PropertyName 来决定是否需要更新自己的状态。
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region 属性
        public MyCommand ShowCommand { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;//`value`是一个上下文关键字，用于表示属性setter中传入的新值。

                // ?.Invoke() 是两个独立但经常一起使用的语法特性的组合：空条件操作符 ?. 和 委托调用方法 Invoke()。
                // Invoke()（委托调用方法）
                // 作用：显式调用委托或事件
                // 行为：执行委托封装的所有方法
                // 语法：委托实例.Invoke(参数)

                // PropertyChanged?
                // 检查 PropertyChanged 事件是否为 null（是否有订阅者）
                // 如果为 null，整个表达式停止执行
                // 如果不为 null，继续执行.Invoke()
                // .Invoke(this, ...)
                // 调用 PropertyChanged 事件（委托）的 Invoke 方法
                // 传递参数：
                // this：触发事件的对象（当前实例）
                // new PropertyChangedEventArgs(...)：事件参数
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        #endregion

        #region 构造函数
        public MainViewModel()
        {
            Name = "hello, life";
            ShowCommand = new MyCommand(Show);
        }
        #endregion

        #region 执行函数
        public void Show()
        {
            Name = "bye";
            MessageBox.Show(Name);
        }
        #endregion
    }
    #endif


    // 语法糖：定义多一个ViewModelBase
    public class MainViewModel : ViewModelBase
    {
        #region 属性
        public MyCommand ShowCommand { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region 构造函数
        public MainViewModel()
        {
            Name = "hello, life";
            Title = "string.Empty";
            ShowCommand = new MyCommand(Show);
        }
        #endregion

        #region 执行函数
        public void Show()
        {
            Name = "bye";
            Title = "labubu";
            MessageBox.Show(Name);
        }
        #endregion
    }





}
