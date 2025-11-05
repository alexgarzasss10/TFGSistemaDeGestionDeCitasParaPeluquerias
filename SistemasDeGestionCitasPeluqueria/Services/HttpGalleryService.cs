using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpGalleryService(HttpClient http) : IGalleryService
{
    private readonly HttpClient _http = http;

    public async Task<IReadOnlyList<GalleryItem>> GetAllAsync(CancellationToken ct = default)
    {
        var list = await _http.GetFromJsonAsync<List<GalleryItem>>("gallery", JsonDefaults.Web, ct)
                   ?? new List<GalleryItem>();
        return list.Where(g => g.IsVisible).OrderBy(g => g.Order).ToList();
    }
}
