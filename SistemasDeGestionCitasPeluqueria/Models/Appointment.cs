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
    public int Id { get; set; }             // CITA.id
    public int UserId { get; set; }         // CITA.usuario_id (FK USUARIO)
    public int BarberId { get; set; }       // CITA.barbero_id (FK BARBERO)
    public int ServiceId { get; set; }      // CITA.servicio_id (FK SERVICIO)

    public DateTimeOffset ScheduledAt { get; set; }     // CITA.fecha_hora
    public AppointmentState State { get; set; } = AppointmentState.Pendiente; // CITA.estado
}