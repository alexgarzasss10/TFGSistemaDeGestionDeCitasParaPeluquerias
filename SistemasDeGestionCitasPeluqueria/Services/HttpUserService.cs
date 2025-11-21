using System.Net.Http.Headers;
using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpUserService(HttpClient http) : IUserService
{
    private readonly HttpClient _http = http;

    public async Task<UserProfile?> GetMeAsync(CancellationToken ct = default)
    {
        var me = await _http.GetFromJsonAsync<UserProfile>("users/me", JsonDefaults.Web, ct);
        if (me is not null)
            me.PhotoUrl = UrlHelper.EnsureAbsolute(me.PhotoUrl, _http.BaseAddress);
        return me;
    }

    public async Task UpdateMeAsync(UpdateUserProfileRequest request, CancellationToken ct = default)
    {
        var resp = await _http.PutAsJsonAsync("users/me", request, JsonDefaults.Web, ct);
        resp.EnsureSuccessStatusCode();
    }

    public async Task<string?> UploadPhotoAsync(Stream photoStream, string fileName, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(photoStream);

        var lower = fileName.ToLowerInvariant();
        var mime = lower.EndsWith(".png") ? "image/png"
                 : lower.EndsWith(".webp") ? "image/webp"
                 : "image/jpeg"; // por defecto

        streamContent.Headers.ContentType = new MediaTypeHeaderValue(mime);

        content.Add(streamContent, "file", fileName);

        var resp = await _http.PostAsync("users/me/photo", content, ct);

        if (!resp.IsSuccessStatusCode)
        {
            var body = await resp.Content.ReadAsStringAsync(ct);
            throw new HttpRequestException($"Error uploading photo: {(int)resp.StatusCode} {resp.StatusCode}. Body: {body}");
        }

        var dto = await resp.Content.ReadFromJsonAsync<PhotoUploadResponse>(JsonDefaults.Web, ct);
        return UrlHelper.EnsureAbsolute(dto?.PhotoUrl, _http.BaseAddress);
    }
}