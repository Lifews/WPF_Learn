using System.ComponentModel;

namespace Serial_Learn;

/// <summary>
/// 波特率
/// </summary>
public enum BaudRate
{
    [Description("300")]
    Baud300 = 300,

    [Description("1200")]
    Baud1200 = 1200,

    [Description("2400")]
    Baud2400 = 2400,

    [Description("4800")]
    Baud4800 = 4800,

    [Description("9600")]
    Baud9600 = 9600,

    [Description("19200")]
    Baud19200 = 19200,

    [Description("38400")]
    Baud38400 = 38400,

    [Description("57600")]
    Baud57600 = 57600,

    [Description("115200")]
    Baud115200 = 115200
}
/// <summary>
/// 数据位
/// </summary>
public enum DataBits
{
    [Description("5位")]
    Five = 5,

    [Description("6位")]
    Six = 6,

    [Description("7位")]
    Seven = 7,

    [Description("8位")]
    Eight = 8
}
/// <summary>
/// 停止位
/// </summary>
public enum StopBit
{
    [Description("1位")]
    One,

    [Description("1.5位")]
    OnePointFive,

    [Description("2位")]
    Two
}
/// <summary>
/// 校验位
/// </summary>
public enum Parity
{
    None,

    Odd,

    Even,

    Mark,

    Space
}