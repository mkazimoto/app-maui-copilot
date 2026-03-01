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

    /// <summary>
    /// Navigate to <c>DetailPage</c> passing query parameters.
    /// The route "details" is registered in <see cref="AppShell"/>.
    /// </summary>
    [RelayCommand]
    private async Task NavigateToDetail()
    {
        // Pass simple values as query-string parameters.
        // Received by DetailViewModel via IQueryAttributable.
        var parameters = new ShellNavigationQueryParameters
        {
            { "id", _count.ToString() },
            { "description", $"You clicked the button {_count} time(s)." }
        };

        await Shell.Current.GoToAsync("details", parameters);
    }
}

