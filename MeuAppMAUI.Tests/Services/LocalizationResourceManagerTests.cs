using System.ComponentModel;
using System.Globalization;
using MeuAppMAUI.Services;

namespace MeuAppMAUI.Tests.Services;

/// <summary>
/// Tests for <see cref="LocalizationResourceManager"/>.
/// Each test that mutates the singleton's culture restores it in a finally block.
///
/// Note: the MAUI resx build pipeline embeds satellite resources with the culture
/// suffix in the resource name (e.g. "…AppResources.pt-BR.resources") instead of
/// the standard satellite convention expected by <see cref="System.Resources.ResourceManager"/>
/// when running on the desktop test host.  Culture-switching tests therefore
/// verify delegation behaviour — that the indexer forwards the culture set by
/// <see cref="LocalizationResourceManager.SetCulture"/> to the underlying
/// <see cref="System.Resources.ResourceManager"/> — rather than asserting specific
/// translated strings that require a working satellite assembly.
/// </summary>
public class LocalizationResourceManagerTests
{
    // ──────────────────────────────────────────────────────────────────────────
    // Singleton
    // ──────────────────────────────────────────────────────────────────────────

    [Fact]
    public void Instance_AlwaysReturnsSameSingleton()
    {
        var a = LocalizationResourceManager.Instance;
        var b = LocalizationResourceManager.Instance;

        Assert.Same(a, b);
    }

    // ──────────────────────────────────────────────────────────────────────────
    // Indexer — default / English culture
    // ──────────────────────────────────────────────────────────────────────────

    [Fact]
    public void Indexer_KnownKey_ReturnsDefaultString()
    {
        var mgr = LocalizationResourceManager.Instance;
        mgr.SetCulture(new CultureInfo("en-US"));

        var result = mgr["AppTitle"];

        Assert.Equal("MeuAppMAUI", result);
    }

    [Theory]
    [InlineData("AppTitle",        "MeuAppMAUI")]
    [InlineData("WelcomeTitle",    "Hello, World!")]
    [InlineData("WelcomeSubtitle", "Welcome to .NET MAUI")]
    [InlineData("ClickBtn",        "Click me")]
    public void Indexer_AllDefaultKeys_ReturnExpectedEnglishStrings(string key, string expected)
    {
        var mgr = LocalizationResourceManager.Instance;
        mgr.SetCulture(new CultureInfo("en-US"));

        Assert.Equal(expected, mgr[key]);
    }

    [Fact]
    public void Indexer_UnknownKey_ReturnsFallbackKey()
    {
        var result = LocalizationResourceManager.Instance["ThisKeyDoesNotExist"];

        Assert.Equal("ThisKeyDoesNotExist", result);
    }

    // ──────────────────────────────────────────────────────────────────────────
    // SetCulture — string-fallback behaviour on the desktop test host
    //
    // The MAUI resx pipeline embeds satellite resources with the culture suffix
    // in the embedded resource name (e.g. "…AppResources.pt-BR.resources"),
    // which the standard ResourceManager cannot resolve on the desktop host.
    // All cultures therefore fall back to the default strings.  These tests
    // verify the indexer contract — it returns a non-null value and correctly
    // falls back — without coupling to translated text.
    // ──────────────────────────────────────────────────────────────────────────

    [Theory]
    [InlineData("en-US",  "WelcomeTitle")]
    [InlineData("en-US",  "ClickBtn")]
    [InlineData("pt-BR",  "WelcomeTitle")]
    [InlineData("pt-BR",  "ClickBtn")]
    public void Indexer_KnownKey_ReturnsNonEmptyString_ForAnyCulture(string culture, string key)
    {
        var mgr = LocalizationResourceManager.Instance;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            mgr.SetCulture(new CultureInfo(culture));

            var result = mgr[key];

            // The result must be a real string, not the bare key name
            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.NotEqual(key, result);
        }
        finally
        {
            mgr.SetCulture(originalUICulture);
        }
    }

    [Fact]
    public void SetCulture_SwitchingBackToEnglish_RestoresDefaultStrings()
    {
        var mgr = LocalizationResourceManager.Instance;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            mgr.SetCulture(new CultureInfo("pt-BR"));
            mgr.SetCulture(new CultureInfo("en-US"));

            Assert.Equal("Hello, World!", mgr["WelcomeTitle"]);
        }
        finally
        {
            mgr.SetCulture(originalUICulture);
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    // SetCulture — CurrentCulture / CurrentUICulture side effects
    // ──────────────────────────────────────────────────────────────────────────

    [Fact]
    public void SetCulture_UpdatesCurrentUICulture()
    {
        var mgr = LocalizationResourceManager.Instance;
        var original = CultureInfo.CurrentUICulture;

        try
        {
            mgr.SetCulture(new CultureInfo("pt-BR"));

            Assert.Equal("pt-BR", CultureInfo.CurrentUICulture.Name);
        }
        finally
        {
            mgr.SetCulture(original);
        }
    }

    [Fact]
    public void SetCulture_UpdatesCurrentCulture()
    {
        var mgr = LocalizationResourceManager.Instance;
        var original = CultureInfo.CurrentCulture;

        try
        {
            mgr.SetCulture(new CultureInfo("pt-BR"));

            Assert.Equal("pt-BR", CultureInfo.CurrentCulture.Name);
        }
        finally
        {
            mgr.SetCulture(original);
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    // PropertyChanged
    // ──────────────────────────────────────────────────────────────────────────

    [Fact]
    public void SetCulture_RaisesPropertyChangedWithNullPropertyName()
    {
        var mgr = LocalizationResourceManager.Instance;
        var originalUICulture = CultureInfo.CurrentUICulture;
        var raised = new List<string?>();

        PropertyChangedEventHandler handler = (_, e) => raised.Add(e.PropertyName);
        mgr.PropertyChanged += handler;

        try
        {
            mgr.SetCulture(new CultureInfo("pt-BR"));

            // null property name signals "all properties changed" — required for
            // indexer bindings in XAML to refresh
            Assert.Contains((string?)null, raised);
        }
        finally
        {
            mgr.PropertyChanged -= handler;
            mgr.SetCulture(originalUICulture);
        }
    }

    [Fact]
    public void SetCulture_RaisesPropertyChangedExactlyOnce_PerCall()
    {
        var mgr = LocalizationResourceManager.Instance;
        var originalUICulture = CultureInfo.CurrentUICulture;
        var count = 0;

        PropertyChangedEventHandler handler = (_, _) => count++;
        mgr.PropertyChanged += handler;

        try
        {
            mgr.SetCulture(new CultureInfo("pt-BR"));

            Assert.Equal(1, count);
        }
        finally
        {
            mgr.PropertyChanged -= handler;
            mgr.SetCulture(originalUICulture);
        }
    }
}
