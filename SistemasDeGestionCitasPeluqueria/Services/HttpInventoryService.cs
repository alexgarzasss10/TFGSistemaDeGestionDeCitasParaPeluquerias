using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpInventoryService(HttpClient http) : IInventoryService
{
    private readonly HttpClient _http = http;

    public async Task<IReadOnlyList<InventoryItem>> GetFeaturedAsync(int take = 6, CancellationToken ct = default)
    {
        // Si el backend no tiene endpoint específico de destacados, usa /products y toma los primeros
        var list = await _http.GetFromJsonAsync<List<InventoryItem>>("products", JsonDefaults.Web, ct) ?? new List<InventoryItem>();

        // Normaliza URLs de imagen (relativas -> absolutas)
        foreach (var p in list)
            p.ImageUrl = UrlHelper.EnsureAbsolute(p.ImageUrl, _http.BaseAddress);

        return list.Take(take).ToList();
    }

    public async Task<IReadOnlyList<InventoryItem>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _http.GetFromJsonAsync<List<InventoryItem>>("products", JsonDefaults.Web, ct) ?? new List<InventoryItem>();

        foreach (var p in list)
            p.ImageUrl = UrlHelper.EnsureAbsolute(p.ImageUrl, _http.BaseAddress);

        return list;
    }
}
