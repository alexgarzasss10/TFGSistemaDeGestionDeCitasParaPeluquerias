using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services.Fake;

public sealed class FakeBarberService : IBarberService
{
    private static readonly IReadOnlyList<Barber> _barbers =
    [
        new() { Id = 1, Name = "Juan Díaz", Specialty = "Fade, Barba", PhotoUrl = "https://images.unsplash.com/photo-1519741497674-611481863552?q=80&w=800" },
        new() { Id = 2, Name = "María Pérez", Specialty = "Color, Corte mujer", PhotoUrl = "https://images.unsplash.com/photo-1503951914875-452162b0f3f1?q=80&w=800" },
        new() { Id = 3, Name = "Carlos Ruiz", Specialty = "Clásicos, Niños", PhotoUrl = "https://images.unsplash.com/photo-1522335789203-aabd1fc54bc9?q=80&w=800" }
    ];

    public async Task<IReadOnlyList<Barber>> GetAllAsync(CancellationToken ct = default)
    {
        await Task.Delay(300, ct); // simula latencia
        return _barbers;
    }
}