using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;


public class Review
{
    public int Id { get; set; }                          // RESEÑA.id
    public int UserId { get; set; }                      // RESEÑA.usuario_id 
    public int AppointmentId { get; set; }               // RESEÑA.cita_id 
    public int Rating { get; set; }                      // RESEÑA.calificacion 
    public string? Comment { get; set; }                 // RESEÑA.comentario
    public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow; // RESEÑA.fecha
}
