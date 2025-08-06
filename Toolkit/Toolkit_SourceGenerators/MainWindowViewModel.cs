using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Toolkit_SourceGenerators;

public partial class MainWindowViewModel : ObservableObject
{

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ButtonClickCommand))]
    private bool _isEnable = false;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Caption))]
    private string _title = "hello, world!";

    private bool CanButtonClick => IsEnable;
    public string Caption => $"Title:{Title}";


    [RelayCommand(CanExecute = nameof(CanButtonClick))]
    private async Task ButtonClick()
    {
        await Task.Delay(1500);
        Title = ",,,,,";
    }
    public MainWindowViewModel()
    {

    }

}