using Platformer.Views;
using Plugin.Maui.Audio;
using Platformer.Services;
using Platformer.ViewModel;

namespace Platformer;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton(AudioManager.Current);
		builder.Services.AddTransient<Game>();
		builder.Services.AddTransient<Game>();
		builder.Services.AddSingleton<UpgradeService>();
		builder.Services.AddTransient<UpgradesViewModel>();
		return builder.Build();
	}
}
