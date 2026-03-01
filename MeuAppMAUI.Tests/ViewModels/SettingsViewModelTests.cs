using MeuAppMAUI.ViewModels;

namespace MeuAppMAUI.Tests.ViewModels;

public class SettingsViewModelTests
{
    [Fact]
    public void Title_IsSettingsOnConstruction()
    {
        var vm = new SettingsViewModel();
        Assert.Equal("Settings", vm.Title);
    }

    [Fact]
    public void NotificationsEnabled_DefaultFalse()
    {
        var vm = new SettingsViewModel();
        Assert.False(vm.NotificationsEnabled);
    }

    [Fact]
    public void SelectedLanguage_DefaultIsEnglish()
    {
        var vm = new SettingsViewModel();
        Assert.Equal("English", vm.SelectedLanguage);
    }

    [Fact]
    public void NotificationsEnabled_SetTrue_RaisesPropertyChanged()
    {
        // Arrange
        var vm = new SettingsViewModel();
        var changedProperties = new List<string?>();
        vm.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        // Act
        vm.NotificationsEnabled = true;

        // Assert
        Assert.True(vm.NotificationsEnabled);
        Assert.Contains(nameof(vm.NotificationsEnabled), changedProperties);
    }

    [Fact]
    public void SelectedLanguage_Set_RaisesPropertyChanged()
    {
        // Arrange
        var vm = new SettingsViewModel();
        var changedProperties = new List<string?>();
        vm.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        // Act
        vm.SelectedLanguage = "Portuguese";

        // Assert
        Assert.Equal("Portuguese", vm.SelectedLanguage);
        Assert.Contains(nameof(vm.SelectedLanguage), changedProperties);
    }

    [Fact]
    public void IsBusy_DefaultFalse()
    {
        var vm = new SettingsViewModel();
        Assert.False(vm.IsBusy);
        Assert.True(vm.IsNotBusy);
    }

    [Fact]
    public void IsNotBusy_UpdatesWhenIsBusyChanges()
    {
        // Arrange
        var vm = new SettingsViewModel();
        var changedProperties = new List<string?>();
        vm.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        // Act
        vm.IsBusy = true;

        // Assert
        Assert.True(vm.IsBusy);
        Assert.False(vm.IsNotBusy);
        Assert.Contains(nameof(vm.IsNotBusy), changedProperties);
    }
}
