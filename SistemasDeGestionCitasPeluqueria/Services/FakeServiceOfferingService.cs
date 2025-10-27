using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services.Fake;

public sealed class FakeServiceOfferingService : IServiceOfferingService
{
    private static readonly IReadOnlyList<ServiceOffering> _services =
    [
        new() { Id = 1, Name = "Corte Premium", Description = "Corte + Peinado", Price = 18.00m, DurationMinutes = 30 },
        new() { Id = 2, Name = "Barba", Description = "Perfilado y arreglo de barba", Price = 12.00m, DurationMinutes = 20 },
        new() { Id = 3, Name = "Corte + Barba", Description = "Pack completo", Price = 27.00m, DurationMinutes = 50 },
        new() { Id = 4, Name = "Color", Description = "Tinte y asesoría", Price = 35.00m, DurationMinutes = 60 },
        new() { Id = 5, Name = "Niños", Description = "Corte infantil", Price = 12.00m, DurationMinutes = 25 },
    ];

    public async Task<IReadOnlyList<ServiceOffering>> GetAllAsync(CancellationToken ct = default)
    {
        await Task.Delay(300, ct);
        return _services;
    }
}