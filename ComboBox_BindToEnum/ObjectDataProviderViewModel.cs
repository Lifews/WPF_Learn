using CommunityToolkit.Mvvm.ComponentModel;

namespace ComboBox_BindToEnum;

public partial class ObjectDataProviderViewModel : ObservableObject
{
    [ObservableProperty]
    DateOfWeek dateOfWeek;
}