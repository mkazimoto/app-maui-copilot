using System.ComponentModel;
using System.Globalization;
using MeuAppMAUI.Resources.Strings;

namespace MeuAppMAUI.Services;

/// <summary>
/// Singleton that exposes localized strings as an indexer so XAML bindings
/// update automatically when the culture is changed at runtime.
/// </summary>
public class LocalizationResourceManager : INotifyPropertyChanged
{
    public static LocalizationResourceManager Instance { get; } = new();

    private LocalizationResourceManager() { }

    /// <summary>Indexer — use as <c>{Binding [KeyName], Source={x:Static ...}}</c>.</summary>
    public string this[string key] =>
        AppResources.ResourceManager.GetString(key, AppResources.Culture) ?? key;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Changes the active UI culture and notifies all bound elements to refresh.
    /// </summary>
    public void SetCulture(CultureInfo culture)
    {
        AppResources.Culture = culture;
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}
