using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpBarberService(HttpClient http) : IBarberService
{
    private readonly HttpClient _http = http;

    public async Task<IReadOnlyList<Barber>> GetAllAsync(CancellationToken ct = default)
    {
        // Campos extra del JSON (teléfono, bio, etc.) se ignoran automáticamente
        var list = await _http.GetFromJsonAsync<List<Barber>>("barbers", cancellationToken: ct)
                   ?? new List<Barber>();
        return list;
    }
}
