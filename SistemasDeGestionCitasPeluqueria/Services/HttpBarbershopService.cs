using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpBarbershopService(HttpClient http) : IBarbershopService
{
    private readonly HttpClient _http = http;

    public async Task<Barbershop?> GetAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<Barbershop>("barbershop", JsonDefaults.Web, ct);
}
