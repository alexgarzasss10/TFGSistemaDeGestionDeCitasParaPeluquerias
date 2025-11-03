using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services
{

    public sealed class HttpServiceOfferingService(HttpClient http) : IServiceOfferingService
    {
        private readonly HttpClient _http = http;

        public async Task<IReadOnlyList<ServiceOffering>> GetAllAsync(CancellationToken ct = default)
        {
            var list = await _http.GetFromJsonAsync<List<ServiceOffering>>("services", cancellationToken: ct)
                       ?? new List<ServiceOffering>();
            return list;
        }
    }
}
