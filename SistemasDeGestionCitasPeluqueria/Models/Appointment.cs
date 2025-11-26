namespace SistemasDeGestionCitasPeluqueria.Models;

public enum AppointmentState
{
    Pendiente,
    Confirmada,
    Cancelada,
    Completada
}

public class Appointment
{
    public int Id { get; set; }                          // CITA.id
    public int UserId { get; set; }                      // CITA.usuario_id 
    public int BarbershopId { get; set; }                // CITA.barberia_id 
    public int BarberId { get; set; }                    // CITA.barbero_id 
    public int ServiceId { get; set; }                   // CITA.servicio_id 

    public DateTimeOffset ScheduledAt { get; set; }      
    public DateTimeOffset StartAt { get; set; }          // CITA.fecha_inicio
    public DateTimeOffset EndAt { get; set; }            // CITA.fecha_fin

    public AppointmentState State { get; set; } = AppointmentState.Pendiente; // CITA.estado
    public string? Notes { get; set; }                   // CITA.notas
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;    // CITA.creado_en
}