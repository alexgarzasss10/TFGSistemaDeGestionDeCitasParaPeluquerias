using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using SistemasDeGestionCitasPeluqueria.PageModels;
using SistemasDeGestionCitasPeluqueria.Pages;
using SistemasDeGestionCitasPeluqueria.Services;
using SistemasDeGestionCitasPeluqueria.Services.Fake;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Licensing;

namespace SistemasDeGestionCitasPeluqueria;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH9dcXRSQmhdUUB1WEJWYEg=");

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

        // Cambia este flag según necesites backend real o datos fake
        var useFakes = false;

        if (!useFakes)
        {
            // Clientes HTTP hacia FastAPI (puerto/host según plataforma)
            builder.Services.AddBackendClients(ServiceRegistration.GetDevBaseAddress(), replaceFakes: true);
        }
        else
        {
            // Servicios fake
            builder.Services.AddSingleton<IServiceOfferingService, FakeServiceOfferingService>();
            builder.Services.AddSingleton<IBarberService, FakeBarberService>();
            builder.Services.AddSingleton<IInventoryService, FakeInventoryService>();
            builder.Services.AddSingleton<IReviewService, FakeReviewService>();
        }

        // VM y páginas
        builder.Services.AddSingleton<MainPageModel>();
        builder.Services.AddSingleton<MainPage>();

        builder.Services.AddSingleton<ServicesPageModel>();
        builder.Services.AddSingleton<ServicesPage>();

        // Products page + VM: transient to avoid shared state/cancellation between navigations
        builder.Services.AddTransient<ProductsPageModel>();
        builder.Services.AddTransient<ProductsPage>();

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
