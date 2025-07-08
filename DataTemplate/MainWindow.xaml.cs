using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataTemplate
{
    /// <summary>  
    /// MainWindow.xaml 的交互逻辑  
    /// </summary>  
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();


            List<Color> list = new List<Color>();

            list.Add(new Color() { Code = "#FFFAFA", Name = "Snow" });
            list.Add(new Color() { Code = "#F8F8FF", Name = "GhostWhite" });
            list.Add(new Color() { Code = "#FFFAF0", Name = "FloralWhite" });
            list.Add(new Color() { Code = "#FAF0E6", Name = "Linen" });
            list.Add(new Color() { Code = "#FFFFF0", Name = "Ivory" });

            listBox1.ItemsSource = list;
            dataGrid1.ItemsSource = list;
        }
        public class Color
        {
            public string Code { get; set; }
            public string Name { get; set; }
        }
    }
}
