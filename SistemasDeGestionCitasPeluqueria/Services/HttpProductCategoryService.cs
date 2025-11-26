using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpProductCategoryService(HttpClient http) : IProductCategoryService
{
    private readonly HttpClient _http = http;

    public async Task<IReadOnlyList<ProductCategory>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _http.GetFromJsonAsync<List<ProductCategory>>("product-categories", JsonDefaults.Web, ct);
        return list ?? new List<ProductCategory>();
    }
}
