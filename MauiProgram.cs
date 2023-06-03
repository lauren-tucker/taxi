using Microsoft.Extensions.Logging;

namespace TAXILauncher;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("Erpal.ttf", "Erpal");
			});
		

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
