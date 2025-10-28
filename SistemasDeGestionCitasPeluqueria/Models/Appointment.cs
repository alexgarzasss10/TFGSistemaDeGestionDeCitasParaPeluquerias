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
    public int UserId { get; set; }                      // CITA.usuario_id (FK USUARIO)
    public int BarbershopId { get; set; }                // CITA.barberia_id (FK BARBERIA)
    public int BarberId { get; set; }                    // CITA.barbero_id (FK BARBERO)
    public int ServiceId { get; set; }                   // CITA.servicio_id (FK SERVICIO)

    public DateTimeOffset ScheduledAt { get; set; }      // (compat) usado en UI; opcional respecto a BD
    public DateTimeOffset StartAt { get; set; }          // CITA.fecha_inicio
    public DateTimeOffset EndAt { get; set; }            // CITA.fecha_fin

    public AppointmentState State { get; set; } = AppointmentState.Pendiente; // CITA.estado
    public string? Notes { get; set; }                   // CITA.notas
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;    // CITA.creado_en
}