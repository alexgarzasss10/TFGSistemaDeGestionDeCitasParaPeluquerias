using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public interface IInventoryService
{
    Task<IReadOnlyList<InventoryItem>> GetFeaturedAsync(int take = 6, CancellationToken ct = default);
    Task<IReadOnlyList<InventoryItem>> GetAllAsync(CancellationToken ct = default);
}
