using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MeuAppMAUI.Resources.Strings;

namespace MeuAppMAUI.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _counterText = AppResources.ClickBtn;

    private int _count;

    public MainViewModel()
    {
        Title = AppResources.AppTitle;
    }

    [RelayCommand]
    private void IncrementCounter()
    {
        _count++;
        CounterText = _count == 1
            ? $"Clicked {_count} time"
            : $"Clicked {_count} times";
    }
}

