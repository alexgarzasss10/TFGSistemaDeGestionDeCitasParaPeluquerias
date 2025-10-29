using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SistemasDeGestionCitasPeluqueria.PageModels;
using SistemasDeGestionCitasPeluqueria.Pages;
using SistemasDeGestionCitasPeluqueria.Services;
using SistemasDeGestionCitasPeluqueria.Services.Fake;
using Syncfusion.Maui.Core.Hosting;

namespace SistemasDeGestionCitasPeluqueria;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.ConfigureSyncfusionCore();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Servicios fake
        builder.Services.AddSingleton<IServiceOfferingService, FakeServiceOfferingService>();
        builder.Services.AddSingleton<IBarberService, FakeBarberService>();
        builder.Services.AddSingleton<IInventoryService, FakeInventoryService>();
        builder.Services.AddSingleton<IReviewService, FakeReviewService>();

        // VM y páginas
        builder.Services.AddSingleton<MainPageModel>();
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<BookingPage>();


        builder.Services.AddSingleton<ServicesPageModel>();
        builder.Services.AddSingleton<ServicesPage>();

        builder.Services.AddSingleton<ProductsPageModel>();
        builder.Services.AddSingleton<ProductsPage>();

        builder.Services.AddTransient<BookingPageModel>();
        builder.Services.AddTransient<BookingPage>();


        builder.Services.AddSingleton<ReviewsPageModel>();
        builder.Services.AddSingleton<ReviewsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
