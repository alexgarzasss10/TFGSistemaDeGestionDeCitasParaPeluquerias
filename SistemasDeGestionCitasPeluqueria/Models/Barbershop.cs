using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;

public class Barbershop
{
    public int Id { get; set; }                          // BARBERIA.id
    public string Name { get; set; } = string.Empty;     // BARBERIA.nombre
    public string Phone { get; set; } = string.Empty;    // BARBERIA.telefono
    public string Email { get; set; } = string.Empty;    // BARBERIA.email
    public string Address { get; set; } = string.Empty;  // BARBERIA.direccion
    public string City { get; set; } = string.Empty;     // BARBERIA.ciudad
    public string Country { get; set; } = string.Empty;  // BARBERIA.pais
    public decimal Latitude { get; set; }                // BARBERIA.latitud (decimal(9,6))
    public decimal Longitude { get; set; }               // BARBERIA.longitud (decimal(9,6))
    public bool IsActive { get; set; } = true;           // BARBERIA.activo

    // Campos adicionales del endpoint /barbershop
    public string Timezone { get; set; } = "Europe/Madrid";
    public List<string> Images { get; set; } = [];       // barbershop.images
    public string? About { get; set; }                   // barbershop.about
    public SocialLinks? Social { get; set; }             // barbershop.social
    public OpeningHours? OpeningHours { get; set; }      // barbershop.openingHours
}

public sealed class SocialLinks
{
    public string? Instagram { get; set; }
    public string? Facebook { get; set; }
    public string? Web { get; set; }
    public string? TikTok { get; set; }
}

public sealed class OpeningHours
{
    public DayHours? Monday { get; set; }
    public DayHours? Tuesday { get; set; }
    public DayHours? Wednesday { get; set; }
    public DayHours? Thursday { get; set; }
    public DayHours? Friday { get; set; }
    public DayHours? Saturday { get; set; }
    public DayHours? Sunday { get; set; }
}

public sealed class DayHours
{
    public string Open { get; set; } = string.Empty;   // "09:00"
    public string Close { get; set; } = string.Empty;  // "20:00"
}
