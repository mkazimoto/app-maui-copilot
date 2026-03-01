using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MeuAppMAUI.ViewModels;

/// <summary>
/// Receives navigation parameters via <see cref="IQueryAttributable"/>.
/// Both query-string and complex-object passing are handled here.
/// </summary>
public partial class DetailViewModel : BaseViewModel, IQueryAttributable
{
    [ObservableProperty]
    private string _itemId = string.Empty;

    [ObservableProperty]
    private string _itemDescription = string.Empty;

    public DetailViewModel()
    {
        Title = "Detail";
    }

    /// <inheritdoc/>
    /// Called by Shell with every parameter passed via GoToAsync.
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var id))
            ItemId = Uri.UnescapeDataString(id.ToString() ?? string.Empty);

        if (query.TryGetValue("description", out var desc))
            ItemDescription = Uri.UnescapeDataString(desc.ToString() ?? string.Empty);

        Title = $"Detail – {ItemId}";
    }

    [RelayCommand]
    private async Task GoBack()
    {
        // Navigate back one level in the stack
        await Shell.Current.GoToAsync("..");
    }
}
