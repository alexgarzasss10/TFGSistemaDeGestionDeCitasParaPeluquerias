using System.Text.Json;
using System.Text.Json.Serialization;

namespace SistemasDeGestionCitasPeluqueria.Services;

internal static class JsonDefaults
{
    // Perfil Web: camelCase, case-insensitive y tolera números como string
    public static readonly JsonSerializerOptions Web = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    };
}