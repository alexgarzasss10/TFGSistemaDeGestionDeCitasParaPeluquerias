using System.Net.Http.Json;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpAuthService(HttpClient http, ITokenStore tokenStore) : IAuthService
{
    private readonly HttpClient _http = http;
    private readonly ITokenStore _tokenStore = tokenStore;

    public async Task<bool> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        var req = new LoginRequestDto { Username = username, Password = password };
        var resp = await _http.PostAsJsonAsync("auth/login", req, JsonDefaults.Web, ct);
        if (!resp.IsSuccessStatusCode) return false;

        var tokens = await resp.Content.ReadFromJsonAsync<TokenResponse>(JsonDefaults.Web, ct);
        if (tokens is null || string.IsNullOrWhiteSpace(tokens.AccessToken) || string.IsNullOrWhiteSpace(tokens.RefreshToken))
            return false;

        var expiry = DateTimeOffset.UtcNow.AddSeconds(Math.Max(0, tokens.ExpiresIn - 30)); // margen
        await _tokenStore.SaveAsync(tokens.AccessToken, tokens.RefreshToken, expiry);
        return true;
    }

    public async Task<string?> GetAccessTokenAsync(CancellationToken ct = default)
    {
        var (access, _, expiry) = await _tokenStore.ReadAsync();
        if (string.IsNullOrWhiteSpace(access)) return null;
        if (expiry is DateTimeOffset e && e <= DateTimeOffset.UtcNow) return null;
        return access;
    }

    public async Task<bool> RefreshAsync(CancellationToken ct = default)
    {
        var (_, refresh, _) = await _tokenStore.ReadAsync();
        if (string.IsNullOrWhiteSpace(refresh)) return false;

        var req = new RefreshRequestDto { RefreshToken = refresh! };
        var resp = await _http.PostAsJsonAsync("auth/refresh", req, JsonDefaults.Web, ct);
        if (!resp.IsSuccessStatusCode) return false;

        var dto = await resp.Content.ReadFromJsonAsync<AccessTokenResponse>(JsonDefaults.Web, ct);
        if (dto is null || string.IsNullOrWhiteSpace(dto.AccessToken)) return false;

        var expiry = DateTimeOffset.UtcNow.AddSeconds(Math.Max(0, dto.ExpiresIn - 30));
        await _tokenStore.SaveAccessTokenAsync(dto.AccessToken, expiry);
        return true;
    }

    public Task<bool> IsLoggedInAsync(CancellationToken ct = default) => GetAccessTokenAsync(ct).ContinueWith(t => !string.IsNullOrWhiteSpace(t.Result), ct);

    public Task LogoutAsync() => _tokenStore.ClearAsync();
}