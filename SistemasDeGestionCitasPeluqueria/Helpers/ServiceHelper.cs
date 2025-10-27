using Microsoft.Extensions.DependencyInjection;

namespace SistemasDeGestionCitasPeluqueria.Helpers;

public static class ServiceHelper
{
    public static IServiceProvider Services =>
        Application.Current?.Handler?.MauiContext?.Services
        ?? throw new InvalidOperationException("Services no disponible aún.");

    public static T GetRequiredService<T>() where T : notnull =>
        Services.GetRequiredService<T>();
}