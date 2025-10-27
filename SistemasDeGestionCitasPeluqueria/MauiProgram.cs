using Microsoft.Extensions.Logging;
using SistemasDeGestionCitasPeluqueria.PageModels;
using SistemasDeGestionCitasPeluqueria.Services;
using SistemasDeGestionCitasPeluqueria.Services.Fake;

namespace SistemasDeGestionCitasPeluqueria;

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

        // DI: Servicios fake (sustituibles por implementación HTTP más adelante)
        builder.Services.AddSingleton<IServiceOfferingService, FakeServiceOfferingService>();
        builder.Services.AddSingleton<IBarberService, FakeBarberService>();
        builder.Services.AddSingleton<IInventoryService, FakeInventoryService>();

        // ViewModels
        builder.Services.AddSingleton<MainPageModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
