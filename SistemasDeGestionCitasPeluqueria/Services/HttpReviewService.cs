using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services
{
    public sealed class HttpReviewService(HttpClient http) : IReviewService
    {
        private readonly HttpClient _http = http;

        public async Task<IReadOnlyList<ServiceReview>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _http.GetFromJsonAsync<List<ServiceReview>>("reviews", cancellationToken: ct)
                       ?? new List<ServiceReview>();
            // Más recientes primero
            return list.OrderByDescending(r => r.Date).ToList();
        }

        public async Task AddAsync(ServiceReview review, CancellationToken ct = default)
        {
            var response = await _http.PostAsJsonAsync("reviews", review, ct);
            response.EnsureSuccessStatusCode();

            // Intenta leer el creado (por si el backend asigna Id/Date)
            var created = await response.Content.ReadFromJsonAsync<ServiceReview>(cancellationToken: ct);
            if (created is not null)
            {
                review.Id = created.Id;
                review.Date = created.Date;
            }
            else
            {
                // Fallback si el backend no devuelve payload
                review.Date = DateTimeOffset.UtcNow;
            }
        }
    }
}
