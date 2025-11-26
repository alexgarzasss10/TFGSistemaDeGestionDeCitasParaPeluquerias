using Microsoft.Maui.Storage;

namespace SistemasDeGestionCitasPeluqueria.Services;

public interface ITokenStore
{
    Task SaveAsync(string accessToken, string refreshToken, DateTimeOffset accessExpiry);
    Task<(string? accessToken, string? refreshToken, DateTimeOffset? accessExpiry)> ReadAsync();
    Task SaveAccessTokenAsync(string accessToken, DateTimeOffset accessExpiry);
    Task ClearAsync();
}

public sealed class SecureTokenStore : ITokenStore
{
    private const string KeyAccess = "auth.access_token";
    private const string KeyRefresh = "auth.refresh_token";
    private const string KeyExpiry = "auth.access_expiry_utc";

    public async Task SaveAsync(string accessToken, string refreshToken, DateTimeOffset accessExpiry)
    {
        await SecureStorage.SetAsync(KeyAccess, accessToken);
        await SecureStorage.SetAsync(KeyRefresh, refreshToken);
        await SecureStorage.SetAsync(KeyExpiry, accessExpiry.UtcDateTime.ToString("O"));
    }

    public async Task<(string? accessToken, string? refreshToken, DateTimeOffset? accessExpiry)> ReadAsync()
    {
        var access = await SecureStorage.GetAsync(KeyAccess);
        var refresh = await SecureStorage.GetAsync(KeyRefresh);
        var expiryRaw = await SecureStorage.GetAsync(KeyExpiry);

        DateTimeOffset? expiry = null;
        if (!string.IsNullOrWhiteSpace(expiryRaw) && DateTimeOffset.TryParse(expiryRaw, out var dto))
            expiry = dto;

        return (access, refresh, expiry);
    }

    public async Task SaveAccessTokenAsync(string accessToken, DateTimeOffset accessExpiry)
    {
        await SecureStorage.SetAsync(KeyAccess, accessToken);
        await SecureStorage.SetAsync(KeyExpiry, accessExpiry.UtcDateTime.ToString("O"));
    }

    public Task ClearAsync()
    {
        SecureStorage.Remove(KeyAccess);
        SecureStorage.Remove(KeyRefresh);
        SecureStorage.Remove(KeyExpiry);
        return Task.CompletedTask;
    }
}