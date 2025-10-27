using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services
{
    public interface IBarberService
    {
        Task<IReadOnlyList<Barber>> GetAllAsync(CancellationToken ct = default);

    }
}
