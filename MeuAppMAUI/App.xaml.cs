using MeuAppMAUI.ViewModels;

namespace MeuAppMAUI;

public partial class App : Application
{
	private readonly IServiceProvider _services;

	public App(IServiceProvider services)
	{
		_services = services;

		// Restore saved theme before UI is created
		var savedTheme = Preferences.Default.Get("selected_theme", "System");
		SettingsViewModel.ApplyTheme(savedTheme);

		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(_services.GetRequiredService<AppShell>());
	}
}