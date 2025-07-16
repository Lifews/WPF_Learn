using System.Windows;
using System.Globalization;

namespace Toolkit_ObservableValidator;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
public partial class App : Application
{
    public App()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-cn");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-cn");
    }
}

