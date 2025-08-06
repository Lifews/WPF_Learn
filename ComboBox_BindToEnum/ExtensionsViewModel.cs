using ComboBox_binding;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComboBox_BindToEnum;

public partial class ExtensionsViewModel : ObservableObject
{
    [ObservableProperty]
    DateOfWeek dateOfWeek;
}