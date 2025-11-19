using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public sealed class HttpBookingService(HttpClient http) : IBookingService
{
    private readonly HttpClient _http = http;

    public async Task<BookingDto> CreateAsync(CreateBookingRequest request, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("bookings", request, JsonDefaults.Web, ct);
        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<BookingDto>(cancellationToken: ct);
        if (created is null)
            throw new InvalidOperationException("El backend no devolvió la reserva creada.");
        return created;
    }

    public async Task<IReadOnlyList<BookingDto>> GetAllAsync(int? barberId = null, string? date = null, CancellationToken ct = default)
    {
        var url = "bookings";
        var sep = "?";
        if (barberId is not null)
        {
            url += $"{sep}barberId={barberId.Value}";
            sep = "&";
        }
        if (!string.IsNullOrWhiteSpace(date))
        {
            url += $"{sep}date={Uri.EscapeDataString(date)}";
        }

        var list = await _http.GetFromJsonAsync<List<BookingDto>>(url, JsonDefaults.Web, ct);
        return list ?? new List<BookingDto>();
    }

    public async Task<IReadOnlyList<BookingDto>> GetMineAsync(CancellationToken ct = default)
    {
        var list = await _http.GetFromJsonAsync<List<BookingDto>>("bookings/me", JsonDefaults.Web, ct);
        return list ?? new List<BookingDto>();
    }

    public async Task<BookingDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _http.GetFromJsonAsync<BookingDto>($"bookings/{id}", JsonDefaults.Web, ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync($"bookings/{id}", ct);
        response.EnsureSuccessStatusCode();
    }
}