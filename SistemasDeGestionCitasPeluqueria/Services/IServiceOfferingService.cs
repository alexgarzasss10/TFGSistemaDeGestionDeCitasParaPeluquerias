using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services
{
    public interface IServiceOfferingService
    {
        Task<IReadOnlyList<ServiceOffering>> GetAllAsync(CancellationToken ct = default);

    }
}
