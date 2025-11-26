using System.Threading;
using System.Threading.Tasks;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public interface IAvailabilityService
{
    Task<AvailabilityDto> GetAsync(int barberId, string dateStr, int slotMinutes = 10, int? serviceId = null, CancellationToken ct = default);
}