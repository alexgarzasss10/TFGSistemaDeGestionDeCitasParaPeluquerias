using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public interface IBarbershopService
{
    Task<Barbershop?> GetAsync(CancellationToken ct = default);
}
