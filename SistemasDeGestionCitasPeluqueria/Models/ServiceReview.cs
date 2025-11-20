using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemasDeGestionCitasPeluqueria.Models;

public class ServiceReview
{
    public int Id { get; set; }              // RESEÑA.id

    [JsonPropertyName("userId")]
    public int? UserId { get; set; }          // RESEÑA.usuario_id 

    [JsonPropertyName("appointmentId")]
    public int? AppointmentId { get; set; }   // RESEÑA.cita_id 

    [JsonPropertyName("barberId")]
    public int? BarberId { get; set; }       // RESEÑA.barbero_id 

    [JsonPropertyName("serviceId")]
    public int? ServiceId { get; set; }      // RESEÑA.servicio_id 

    [Range(1, 5)]
    public int Rating { get; set; }          // RESEÑA.calificacion (1-5)

    public string? Comment { get; set; }     // RESEÑA.comentario

    // Nombre del autor (lo que devuelve el backend como "userName")
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }    // RESEÑA.usuario_nombre

    // El backend legacy devuelve "createdAt" (ISO). Lo mapeamos a Date.
    [JsonPropertyName("createdAt")]
    public DateTimeOffset Date { get; set; } = DateTimeOffset.UtcNow; // RESEÑA.fecha
}
