using System;

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

    // Nuevas propiedades enriquecidas por el backend
    public string? BarberName { get; set; }
    public string? ServiceName { get; set; }

    // Helpers para UI y fallback si backend no devuelve el nombre
    public string DisplayService => !string.IsNullOrWhiteSpace(ServiceName) ? ServiceName : $"Servicio #{ServiceId}";
    public string DisplayBarber => !string.IsNullOrWhiteSpace(BarberName) ? BarberName : $"Barbero #{BarberId}";
}
