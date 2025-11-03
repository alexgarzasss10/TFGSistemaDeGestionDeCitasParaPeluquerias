using Microsoft.Extensions.DependencyInjection;
using System;

namespace SistemasDeGestionCitasPeluqueria.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddBackendClients(this IServiceCollection services, Uri baseAddress, bool replaceFakes = true)
    {
        if (replaceFakes)
        {
            services.AddHttpClient<IBarberService, HttpBarberService>(c => c.BaseAddress = baseAddress);
            services.AddHttpClient<IInventoryService, HttpInventoryService>(c => c.BaseAddress = baseAddress);
            services.AddHttpClient<IServiceOfferingService, HttpServiceOfferingService>(c => c.BaseAddress = baseAddress);
        }
        return services;
    }

    // Dev helpers para baseAddress según plataforma/emulador
    public static Uri GetDevBaseAddress()
    {
#if ANDROID
        // Emulador Android → host de la máquina
        return new Uri("http://10.0.2.2:5180/");
#elif IOS || MACCATALYST
        return new Uri("http://localhost:5180/");
#else
        return new Uri("http://localhost:5180/");
#endif
    }
}
