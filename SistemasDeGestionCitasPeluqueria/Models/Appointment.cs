namespace SistemasDeGestionCitasPeluqueria.Models;

public class Appointment
{
    public int? Id { get; set; }
    public int? ClientId { get; set; }
    public int? BarberId { get; set; }

    // Antes: ServiceId
    public int? ServiceOfferingId { get; set; }

    public DateTimeOffset? StartAt { get; set; }
    public DateTimeOffset? EndAt { get; set; }
    public string? Status { get; set; }   // "Scheduled", "Confirmed", "Completed", "Canceled"
    public string? Notes { get; set; }
}