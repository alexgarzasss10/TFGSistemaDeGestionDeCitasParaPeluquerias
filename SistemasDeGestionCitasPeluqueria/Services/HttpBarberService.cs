using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpBarberService(HttpClient http) : IBarberService
{
    private readonly HttpClient _http = http;

    public async Task<IReadOnlyList<Barber>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _http.GetFromJsonAsync<List<Barber>>("barbers", JsonDefaults.Web, ct)
                   ?? new List<Barber>();
        return list;
    }
}
