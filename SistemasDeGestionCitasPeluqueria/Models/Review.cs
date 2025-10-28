using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;


public class Review
{
    public int Id { get; set; }                          // RESEÑA.id
    public int UserId { get; set; }                      // RESEÑA.usuario_id (FK USUARIO)
    public int AppointmentId { get; set; }               // RESEÑA.cita_id (FK CITA, unique)
    public int Rating { get; set; }                      // RESEÑA.calificacion (1..5)
    public string? Comment { get; set; }                 // RESEÑA.comentario
    public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow; // RESEÑA.fecha
}
