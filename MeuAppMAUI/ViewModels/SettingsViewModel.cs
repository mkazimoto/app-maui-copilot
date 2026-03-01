using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MeuAppMAUI.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    [ObservableProperty]
    private bool _notificationsEnabled;

    [ObservableProperty]
    private string _selectedLanguage = "English";

    [ObservableProperty]
    private string _selectedTheme = "System";

    public SettingsViewModel()
    {
        Title = "Settings";
        _selectedTheme = Application.Current?.UserAppTheme switch
        {
            AppTheme.Light => "Light",
            AppTheme.Dark => "Dark",
            _ => "System"
        } ?? "System";
    }

    partial void OnSelectedThemeChanged(string value) => ApplyTheme(value);

    public static void ApplyTheme(string theme)
    {
        if (Application.Current is null) return;
        Application.Current.UserAppTheme = theme switch
        {
            "Light" => AppTheme.Light,
            "Dark" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
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
            Preferences.Default.Set("selected_theme", SelectedTheme);

            await Shell.Current.DisplayAlertAsync("Settings", "Settings saved successfully.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
