using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Reflection.Metadata;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace Serial_Learn;

#if false
    基础知识
    1个字节byte    1byte=8bit
    2个字节        short/Int16：-32768~32767;
                   ushort：0~65535
    4个字节        int/uint/Int32/uint32/float
    8个字节        long/ulong/int64/uint64/double
    16个字节       decimal
    
    浮点数 （单精度）float、（双精度）double精度丢失问题：
    原因：二进制无法精确表示所有十进制小数。
    计算机用二进制（0 和 1）存储数据。许多常见的十进制小数（如 0.1、0.2、0.3）在二进制中是无限循环小数。
        例如  十进制 0.1 → 二进制 0.0001100110011...（无限循环）
              十进制 0.2 → 二进制 0.001100110011...（无限循环）
    而浮点数（如 float）只有 32 位（单精度）存储空间，无法完整存储无限循环的二进制小数，必须进行舍入（Rounding），导致精度丢失。

    /// <summary>
    /// 数据类型和字节互相转换示例
    /// </summary>
    private void Initializa()
    {
        int n = 300;
        byte[] b = BitConverter.GetBytes(n);        //数字转换为字节byte[]

        byte[] c = { 0x2c, 0x01 };
        short m = BitConverter.ToInt16(c);          //字节byte[]转换为数字 0x012C = 300

        string str = "ab字符串";                    //1个汉字占 "2个字节+5bit用于编码"
        byte[] d = Encoding.UTF8.GetBytes(str);     //字符串 转换为 字节byte[]，此处3个汉字转成9个字节的数据

        string str1 = Encoding.UTF8.GetString(d);   //字节byte[] 转换为 字符串
    }
#endif

public partial class MainViewModel : ObservableObject, IDisposable
{
    #region 常量

    /// <summary>
    /// 包头
    /// </summary>
    private const byte head = 0xfa;

    /// <summary>
    /// 包尾
    /// </summary>
    private const byte end = 0xfe;

    /// <summary>
    /// 包长
    /// </summary>
    private const int packgeLen = 4;

    /// <summary>
    /// 设备标识与状态地址
    /// </summary>
    private const byte deviceInfoAdress = 0x10;

    /// <summary>
    /// 单轴通讯地址
    /// </summary>
    private byte[] channelAdress = { 0x20, 0x30, 0x40, 0x50, 0x60, 0x70 };

    //功能码
    private const byte lightNum = 0x1;
    private const byte redRead = 0x1;
    private const byte redWrite = 0x2;
    private const byte greenRead = 0x3;
    private const byte greenWrite = 0x4;
    private const byte blueRead = 0x5;
    private const byte blueWrite = 0x6;
    private const byte switchRead = 0x7;
    private const byte switchWrite = 0x8;
    private const byte switchOn = 0xFF;
    private const byte switchOff = 0x00;

    #endregion

    #region 字段

    private SerialPort serialPort = new();

    private Dispatcher _uiDispatcher;

    #endregion

    #region 属性

    [ObservableProperty] private bool _isConnected;//COM口是否链接标志

    [ObservableProperty] private FlowDocument _receivedDocument = new FlowDocument();

    [ObservableProperty] private string _sendContent;

    [ObservableProperty] private ObservableCollection<string> _COMList = new();

    [ObservableProperty] private string? _COM;

    [ObservableProperty] private BaudRate _baudRate;

    [ObservableProperty] private DataBits _dataBits;

    [ObservableProperty] private StopBit _stopBit;

    [ObservableProperty] private Parity _parity;

    [ObservableProperty] private LightNo _lightNo;

    [ObservableProperty] private int _colorR;

    [ObservableProperty] private int _colorG;

    [ObservableProperty] private int _colorB;

    #endregion

    #region 构造、释放
    public MainViewModel()
    {
        SerialInitialize();
        _uiDispatcher = Dispatcher.CurrentDispatcher;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            serialPort.Dispose();
        }
    }

    /// <summary>
    /// 初始化串口列表
    /// </summary>
    private void SerialInitialize()
    {
        var ports = SerialPort.GetPortNames();
        foreach (var port in ports)
        {
            COMList.Add(port);
        }
        if (COMList.Count > 0)
        {
            COM = COMList[0];
        }
    }
    #endregion

    #region 私有方法
    private void AppendTxt(string txt, System.Windows.Media.Color color)
    {
        var p = new Paragraph();
        var r = new Run(txt);
        p.Inlines.Add(r);
        p.Foreground = new SolidColorBrush(color);
        ReceivedDocument.Blocks.Add(p);
    }
    #endregion

    #region 颜色更新
    partial void OnColorRChanged(int value)
    {
        if (!serialPort.IsOpen)
            return;

        int codeFunctionRed = channelAdress[(int)LightNo] + redWrite;

        byte[] sender = new byte[4];
        sender[0] = head;
        sender[1] = (byte)codeFunctionRed;
        sender[2] = (byte)ColorR;
        sender[3] = end;

        //发送数据
        serialPort.Write(sender, 0, sender.Length);
    }
    partial void OnColorGChanged(int value)
    {
        if (!serialPort.IsOpen)
            return;

        int codeFunctionGreen = channelAdress[(int)LightNo] + greenWrite;

        byte[] sender = new byte[4];
        sender[0] = head;
        sender[1] = (byte)codeFunctionGreen;
        sender[2] = (byte)ColorG;
        sender[3] = end;

        //发送数据
        serialPort.Write(sender, 0, sender.Length);
    }
    partial void OnColorBChanged(int value)
    {
        if (!serialPort.IsOpen)
            return;

        int codeFunctionBlue = channelAdress[(int)LightNo] + blueWrite;

        byte[] sender = new byte[4];
        sender[0] = head;
        sender[1] = (byte)codeFunctionBlue;
        sender[2] = (byte)ColorB;
        sender[3] = end;

        //发送数据
        serialPort.Write(sender, 0, sender.Length);
    }
    #endregion

    #region 开关光源
    [RelayCommand]
    public void OpenLight()
    {
        if (!serialPort.IsOpen)
            return;

        int codeFunction = channelAdress[(int)LightNo] + switchWrite;

        byte[] sender = new byte[4];
        sender[0] = head;
        sender[1] = (byte)codeFunction;
        sender[2] = switchOn;
        sender[3] = end;

        //发送数据
        serialPort.Write(sender, 0, sender.Length);
    }
    [RelayCommand]
    public void CloseLight()
    {
        if (!serialPort.IsOpen)
            return;

        int codeFunction = channelAdress[(int)LightNo] + switchWrite;

        byte[] sender = new byte[4];
        sender[0] = head;
        sender[1] = (byte)codeFunction;
        sender[2] = switchOff;
        sender[3] = end;

        //发送数据
        serialPort.Write(sender, 0, sender.Length);
    }
    #endregion









    /// <summary>
    /// 打开/关闭串口
    /// </summary>
    [RelayCommand]
    private void ToggleConnection()
    {
        if (serialPort.IsOpen)
        {
            try
            {
                serialPort.Close();
                AppendTxt($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  关闭串口 {COM}", Colors.Gray);
                IsConnected = false;
            }
            catch
            {
                AppendTxt($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  关闭串口失败", Colors.Red);
            }
        }
        else
        {
            serialPort.PortName = COM;
            serialPort.BaudRate = (int)BaudRate;
            serialPort.DataBits = (int)DataBits;
            switch (StopBit)
            {
                case StopBit.One:
                    serialPort.StopBits = StopBits.One;
                    break;
                case StopBit.OnePointFive:
                    serialPort.StopBits = StopBits.OnePointFive;
                    break;
                case StopBit.Two:
                    serialPort.StopBits = StopBits.Two;
                    break;
            }
            switch (Parity)
            {
                case Parity.None:
                    serialPort.Parity = System.IO.Ports.Parity.None;
                    break;
                case Parity.Odd:
                    serialPort.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case Parity.Even:
                    serialPort.Parity = System.IO.Ports.Parity.Even;
                    break;
                case Parity.Mark:
                    serialPort.Parity = System.IO.Ports.Parity.Mark;
                    break;
                case Parity.Space:
                    serialPort.Parity = System.IO.Ports.Parity.Space;
                    break;
            }
            try
            {
                serialPort.Open();

                Task.Run(async () =>
                {
                    while (true)
                    {
                        _ = _uiDispatcher.Invoke(async () =>

                        {
                            await ReceiveAsync();
                        });
                        await Task.Delay(100);
                    }
                    ;
                });

                AppendTxt($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  打开串口{COM}成功", Colors.Green);
                IsConnected = true;
            }
            catch (Exception ex)
            {
                AppendTxt($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  打开串口{COM}失败", Colors.Red);
            }
        }
    }
    /// <summary>
    /// 发送数据
    /// </summary>
    [RelayCommand]
    private void Send()
    {
        if (serialPort.IsOpen)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(SendContent);
            serialPort.Write(bytes, 0, bytes.Length);

            AppendTxt($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  成功发送\n    {SendContent} ", Colors.MediumPurple);
        }
        else
        {
            MessageBox.Show("请先连接串口");
        }
    }
    /// <summary>
    /// 接收函数
    /// </summary>
    /// <returns></returns>
    private async Task ReceiveAsync()
    {
        byte[] buffer = new byte[4096];

        if (serialPort.ReadBufferSize > 0)
        {
            int readCount = await serialPort.BaseStream.ReadAsync(buffer, 0, buffer.Length);

            if (readCount > 0)
            {
                string data = Encoding.UTF8.GetString(buffer);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AppendTxt($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} 接收\n{data}", Colors.Blue);
                });
            }
        }
    }
}