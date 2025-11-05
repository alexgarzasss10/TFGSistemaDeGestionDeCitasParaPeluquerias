using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpInventoryService(HttpClient http) : IInventoryService
{
    private readonly HttpClient _http = http;

    public async Task<IReadOnlyList<InventoryItem>> GetFeaturedAsync(int take = 6, CancellationToken ct = default)
    {
        var all = await GetAllAsync(ct);
        return all.Take(take).ToList();
    }

    public async Task<IReadOnlyList<InventoryItem>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _http.GetFromJsonAsync<List<InventoryItem>>("products", JsonDefaults.Web, ct)
                   ?? new List<InventoryItem>();
        return list;
    }
}
