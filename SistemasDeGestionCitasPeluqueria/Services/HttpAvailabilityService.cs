using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services
{
    public sealed class HttpAvailabilityService(HttpClient http) : IAvailabilityService
    {
        private readonly HttpClient _http = http;

        public async Task<AvailabilityDto> GetAsync(int barberId, string dateStr, int slotMinutes = 10, int? serviceId = null, CancellationToken ct = default)
        {
            var url = $"availability?barberId={barberId}&dateStr={Uri.EscapeDataString(dateStr)}&slotMinutes={slotMinutes}";
            if (serviceId is not null)
                url += $"&serviceId={serviceId.Value}";

            var dto = await _http.GetFromJsonAsync<AvailabilityDto>(url, JsonDefaults.Web, ct);
            return dto ?? new AvailabilityDto
            {
                BarberId = barberId,
                Date = dateStr,
                SlotMinutes = slotMinutes,
                Timezone = "Europe/Madrid",
                Available = []
            };
        }
    }
}