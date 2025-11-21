using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SistemasDeGestionCitasPeluqueria.Models;

public class ServiceReview
{
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    public int? UserId { get; set; }

    [JsonPropertyName("appointmentId")]
    public int? AppointmentId { get; set; }

    [JsonPropertyName("barberId")]
    public int? BarberId { get; set; }

    [JsonPropertyName("serviceId")]
    public int? ServiceId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public string? Comment { get; set; }

    [JsonPropertyName("userName")]
    public string? UserName { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTimeOffset Date { get; set; }

    // NUEVO: foto de perfil del usuario que hizo la reseña
    [JsonPropertyName("userPhotoUrl")]
    public string? UserPhotoUrl { get; set; }

    // Enriquecidas en cliente
    public string? BarberName { get; set; }
    public string? ServiceName { get; set; }

    // NUEVO: propiedades auxiliares para la UI (ignoradas en JSON)
    [JsonIgnore]
    public bool HasBarberOrService =>
        !string.IsNullOrWhiteSpace(BarberName) || !string.IsNullOrWhiteSpace(ServiceName);

    [JsonIgnore]
    public bool HasBothSelection =>
        !string.IsNullOrWhiteSpace(BarberName) && !string.IsNullOrWhiteSpace(ServiceName);

    [JsonIgnore]
    public string? BarberDisplay =>
        string.IsNullOrWhiteSpace(BarberName) ? null : $"Barbero: {BarberName}";

    [JsonIgnore]
    public string? ServiceDisplay =>
        string.IsNullOrWhiteSpace(ServiceName) ? null : $"Servicio: {ServiceName}";
}
