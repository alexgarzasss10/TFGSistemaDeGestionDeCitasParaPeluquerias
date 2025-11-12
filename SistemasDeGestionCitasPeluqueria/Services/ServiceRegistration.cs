using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace SistemasDeGestionCitasPeluqueria.Services;

public static class ServiceRegistration
{
    public static IServiceCollection AddBackendClients(this IServiceCollection services, Uri baseAddress, bool replaceFakes = true, TimeSpan? httpTimeout = null)
    {
        if (replaceFakes)
        {
            var timeout = httpTimeout ?? TimeSpan.FromSeconds(15);

            services.AddHttpClient<IBarberService, HttpBarberService>(c =>
            {
                c.BaseAddress = baseAddress;
                c.Timeout = timeout;
            });
            services.AddHttpClient<IInventoryService, HttpInventoryService>(c =>
            {
                c.BaseAddress = baseAddress;
                c.Timeout = timeout;
            });
            services.AddHttpClient<IServiceOfferingService, HttpServiceOfferingService>(c =>
            {
                c.BaseAddress = baseAddress;
                c.Timeout = timeout;
            });
            services.AddHttpClient<IReviewService, HttpReviewService>(c =>
            {
                c.BaseAddress = baseAddress;
                c.Timeout = timeout;
            });
            services.AddHttpClient<IProductCategoryService, HttpProductCategoryService>(c =>
            {
                c.BaseAddress = baseAddress;
                c.Timeout = timeout;
            });
           
            services.AddHttpClient<IBarbershopService, HttpBarbershopService>(c =>
            {
                c.BaseAddress = baseAddress;
                c.Timeout = timeout;
            });
            services.AddHttpClient<IGalleryService, HttpGalleryService>(c =>
            {
                c.BaseAddress = baseAddress;
                c.Timeout = timeout;
            });
        }
        return services;
    }

    public static Uri GetDevBaseAddress()
    {
        var env = Environment.GetEnvironmentVariable("API_BASEURL");
        if (!string.IsNullOrWhiteSpace(env) && Uri.TryCreate(env, UriKind.Absolute, out var u))
            return u;

        //MI API ANTERIOR

        //#if ANDROID
        //        return new Uri("http://10.0.2.2:5180/");
        //#elif IOS || MACCATALYST
        //        return new Uri("http://localhost:5180/");
        //#else
        //        return new Uri("http://localhost:5180/");
        //#endif

        //API NUEVA FASTAPI CRUZ

#if ANDROID
    return new Uri("http://10.0.2.2:8000/");
#elif IOS || MACCATALYST
        return new Uri("http://localhost:8000/");
#else
        return new Uri("http://localhost:8000/");
#endif
    }
}
