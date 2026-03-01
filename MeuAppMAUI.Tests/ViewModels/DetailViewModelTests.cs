using MeuAppMAUI.ViewModels;

namespace MeuAppMAUI.Tests.ViewModels;

public class DetailViewModelTests
{
    [Fact]
    public void ApplyQueryAttributes_SetsItemIdAndDescription()
    {
        // Arrange
        var vm = new DetailViewModel();
        var query = new Dictionary<string, object>
        {
            { "id", "42" },
            { "description", "You clicked the button 42 time(s)." }
        };

        // Act
        vm.ApplyQueryAttributes(query);

        // Assert
        Assert.Equal("42", vm.ItemId);
        Assert.Equal("You clicked the button 42 time(s).", vm.ItemDescription);
    }

    [Fact]
    public void ApplyQueryAttributes_UpdatesTitle()
    {
        // Arrange
        var vm = new DetailViewModel();
        var query = new Dictionary<string, object>
        {
            { "id", "7" },
            { "description", "Seven clicks." }
        };

        // Act
        vm.ApplyQueryAttributes(query);

        // Assert
        Assert.Equal("Detail – 7", vm.Title);
    }

    [Fact]
    public void ApplyQueryAttributes_EmptyQuery_DoesNotThrow()
    {
        // Arrange
        var vm = new DetailViewModel();

        // Act
        var ex = Record.Exception(() =>
            vm.ApplyQueryAttributes(new Dictionary<string, object>()));

        // Assert
        Assert.Null(ex);
    }

    [Fact]
    public void ApplyQueryAttributes_DecodesUriEncodedValues()
    {
        // Arrange
        var vm = new DetailViewModel();
        var query = new Dictionary<string, object>
        {
            { "id", "10" },
            { "description", "Hello%20World" }
        };

        // Act
        vm.ApplyQueryAttributes(query);

        // Assert
        Assert.Equal("Hello World", vm.ItemDescription);
    }

    [Fact]
    public void Title_IsDetailOnConstruction()
    {
        var vm = new DetailViewModel();
        Assert.Equal("Detail", vm.Title);
    }

    [Fact]
    public void IsBusy_DefaultFalse()
    {
        var vm = new DetailViewModel();
        Assert.False(vm.IsBusy);
        Assert.True(vm.IsNotBusy);
    }
}
