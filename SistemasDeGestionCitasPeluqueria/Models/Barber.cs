namespace SistemasDeGestionCitasPeluqueria.Models;

public class Barber
{
    public int Id { get; set; }                           // BARBERO.id
    public int BarbershopId { get; set; }                 // BARBERO.barberia_id 
    public string Name { get; set; } = string.Empty;      // BARBERO.nombre
    public string? Specialty { get; set; }                // BARBERO.especialidad
    public string? PhotoUrl { get; set; }                 // BARBERO.foto_url
    public bool IsActive { get; set; } = true;            // BARBERO.activo
}
