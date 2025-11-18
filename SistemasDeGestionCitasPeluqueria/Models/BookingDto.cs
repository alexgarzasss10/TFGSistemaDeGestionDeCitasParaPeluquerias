using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;
public sealed class BookingDto
{
    public int Id { get; set; }
    public int BarberId { get; set; }
    public int ServiceId { get; set; }
    public string CustomerName { get; set; } = "";
    public string? CustomerPhone { get; set; }
    public string Start { get; set; } = ""; // "YYYY-MM-DDTHH:MM"
    public string End { get; set; } = "";
    public string Status { get; set; } = ""; // "confirmed"
}
