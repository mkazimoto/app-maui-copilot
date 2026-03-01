using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MeuAppMAUI.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private bool _notificationsEnabled;

    [ObservableProperty]
    private string _selectedLanguage = "English";

    public SettingsViewModel()
    {
        Title = "Settings";
    }

    [RelayCommand]
    private async Task SaveSettings()
    {
        IsBusy = true;

        try
        {
            // Persist settings (e.g., via Preferences or a service)
            Preferences.Default.Set("notifications_enabled", NotificationsEnabled);
            Preferences.Default.Set("selected_language", SelectedLanguage);

            await Shell.Current.DisplayAlertAsync("Settings", "Settings saved successfully.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
