namespace SistemasDeGestionCitasPeluqueria.Models;

public sealed class CreateBookingRequest
{
    public int BarberId { get; set; }
    public int ServiceId { get; set; }
    public string Date { get; set; } = "";  // "YYYY-MM-DD"
    public string Time { get; set; } = "";  // "HH:MM"
    public string CustomerName { get; set; } = "";
    public string? CustomerPhone { get; set; }
}
