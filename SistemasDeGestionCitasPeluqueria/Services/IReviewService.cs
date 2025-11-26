using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public interface IReviewService
{
    Task<IReadOnlyList<ServiceReview>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ServiceReview review, CancellationToken ct = default);
}