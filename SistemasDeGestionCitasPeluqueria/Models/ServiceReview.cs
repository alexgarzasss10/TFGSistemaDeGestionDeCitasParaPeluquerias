using System;
using System.ComponentModel.DataAnnotations;

namespace SistemasDeGestionCitasPeluqueria.Models;

public class ServiceReview
{
    public int Id { get; set; }              // RESEÑA.id
    public int UserId { get; set; }          // RESEÑA.usuario_id 
    public int AppointmentId { get; set; }   // RESEÑA.cita_id 

    [Range(1, 5)]
    public int Rating { get; set; }          // RESEÑA.calificacion (1-5)

    public string? Comment { get; set; }     // RESEÑA.comentario
    public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow; // RESEÑA.fecha
}
