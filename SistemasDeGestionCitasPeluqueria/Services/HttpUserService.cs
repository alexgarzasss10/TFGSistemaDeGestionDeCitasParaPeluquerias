using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpUserService(HttpClient http) : IUserService
{
    private readonly HttpClient _http = http;

    public async Task<UserProfile?> GetMeAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<UserProfile>("users/me", JsonDefaults.Web, ct);

    public async Task UpdateMeAsync(UpdateUserProfileRequest request, CancellationToken ct = default)
    {
        var resp = await _http.PutAsJsonAsync("users/me", request, JsonDefaults.Web, ct);
        resp.EnsureSuccessStatusCode();
    }
}