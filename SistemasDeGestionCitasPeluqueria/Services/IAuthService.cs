namespace SistemasDeGestionCitasPeluqueria.Services;

public interface IAuthService
{
    Task<bool> LoginAsync(string username, string password, CancellationToken ct = default);

    // Nuevo parámetro opcional 'signIn' antes del CancellationToken.
    Task<bool> RegisterAsync(string username, string password, string? email = null, string? name = null, bool signIn = true, CancellationToken ct = default);

    Task<string?> GetAccessTokenAsync(CancellationToken ct = default);
    Task<bool> RefreshAsync(CancellationToken ct = default);
    Task LogoutAsync();
    Task<bool> IsLoggedInAsync(CancellationToken ct = default);
}