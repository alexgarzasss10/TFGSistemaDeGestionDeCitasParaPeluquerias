using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public interface IBookingService
{
    Task<BookingDto> CreateAsync(CreateBookingRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<BookingDto>> GetAllAsync(int? barberId = null, string? date = null, CancellationToken ct = default);
    Task<IReadOnlyList<BookingDto>> GetMineAsync(CancellationToken ct = default);
    Task<IReadOnlyList<BookingDto>> GetMyUpcomingAsync(int limit = 10, IEnumerable<string>? states = null, CancellationToken ct = default);
    Task<BookingDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
    Task<BookingDto> CancelAsync(int id, CancellationToken ct = default);
}