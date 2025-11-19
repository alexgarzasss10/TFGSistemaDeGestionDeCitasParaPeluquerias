using System.Threading;
using System.Threading.Tasks;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public interface IUserService
{
    Task<UserProfile?> GetMeAsync(CancellationToken ct = default);
    Task UpdateMeAsync(UpdateUserProfileRequest request, CancellationToken ct = default);
}