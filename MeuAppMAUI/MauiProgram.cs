using CommunityToolkit.Maui;
using MeuAppMAUI.ViewModels;
using MeuAppMAUI.Views;
using Microsoft.Extensions.Logging;

namespace MeuAppMAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
				var builder = MauiApp.CreateBuilder();
				builder
					.UseMauiApp<App>()
		#if ANDROID || IOS || MACCATALYST || TIZEN || WINDOWS
					.UseMauiCommunityToolkit()
		#endif
					.ConfigureFonts(fonts =>
					{
						fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
						fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					});

		// Register ViewModels
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();
		builder.Services.AddTransient<DetailViewModel>();

		// Register Views
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<SettingsPage>();
		builder.Services.AddTransient<DetailPage>();

		// Register Services
		// builder.Services.AddSingleton<IMyService, MyService>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

