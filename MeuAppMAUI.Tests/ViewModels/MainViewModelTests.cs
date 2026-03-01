using MeuAppMAUI.ViewModels;

namespace MeuAppMAUI.Tests.ViewModels;

public class MainViewModelTests
{
    [Fact]
    public void IncrementCounter_FirstClick_ShowsSingular()
    {
        // Arrange
        var vm = new MainViewModel();

        // Act
        vm.IncrementCounterCommand.Execute(null);

        // Assert
        Assert.Equal("Clicked 1 time", vm.CounterText);
    }

    [Theory]
    [InlineData(2, "Clicked 2 times")]
    [InlineData(5, "Clicked 5 times")]
    [InlineData(10, "Clicked 10 times")]
    public void IncrementCounter_MultipleTimes_ShowsPlural(int clicks, string expected)
    {
        // Arrange
        var vm = new MainViewModel();

        // Act
        for (var i = 0; i < clicks; i++)
            vm.IncrementCounterCommand.Execute(null);

        // Assert
        Assert.Equal(expected, vm.CounterText);
    }

    [Fact]
    public void IncrementCounter_RaisesPropertyChanged()
    {
        // Arrange
        var vm = new MainViewModel();
        var changedProperties = new List<string?>();
        vm.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        // Act
        vm.IncrementCounterCommand.Execute(null);

        // Assert
        Assert.Contains(nameof(vm.CounterText), changedProperties);
    }

    [Fact]
    public void Title_IsSetOnConstruction()
    {
        var vm = new MainViewModel();
        Assert.False(string.IsNullOrWhiteSpace(vm.Title));
    }

    [Fact]
    public void IsBusy_DefaultFalse()
    {
        var vm = new MainViewModel();
        Assert.False(vm.IsBusy);
        Assert.True(vm.IsNotBusy);
    }
}
