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
            var list = await _http.GetFromJsonAsync<List<ServiceReview>>("reviews", JsonDefaults.Web, ct)
                       ?? new List<ServiceReview>();
            // Más recientes primero
            return list.OrderByDescending(r => r.Date).ToList();
        }

        public async Task AddAsync(ServiceReview review, CancellationToken ct = default)
        {
            // Construye payload alineado con el esquema legacy del backend
            var payload = new
            {
                barberId = review.BarberId ?? 0,
                serviceId = review.ServiceId ?? 0,
                rating = review.Rating,
                comment = review.Comment ?? string.Empty,
                userName = review.UserName ?? "Usuario"
            };

            var response = await _http.PostAsJsonAsync("reviews", payload, JsonDefaults.Web, ct);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(ct);
                // Lanza excepción con el cuerpo para que la UI pueda mostrarlo
                throw new HttpRequestException($"POST /reviews -> {(int)response.StatusCode} {response.ReasonPhrase}. Body: {body}");
            }

            // Si devuelve el objeto creado, actualiza el review local
            var created = await response.Content.ReadFromJsonAsync<ServiceReview>(JsonDefaults.Web, ct);
            if (created is not null)
            {
                review.Id = created.Id;
                review.Date = created.Date;
                review.BarberId = created.BarberId;
                review.ServiceId = created.ServiceId;
                review.UserName = created.UserName;
            }
            else
            {
                review.Date = DateTimeOffset.UtcNow;
            }
        }
    }
}
