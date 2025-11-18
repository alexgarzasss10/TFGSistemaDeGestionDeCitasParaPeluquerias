using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;

public sealed class AvailabilityDto
{
    public int BarberId { get; set; }
    public string Date { get; set; } = "";
    public string Timezone { get; set; } = "";
    public int SlotMinutes { get; set; }
    public List<string> Available { get; set; } = new();
}
