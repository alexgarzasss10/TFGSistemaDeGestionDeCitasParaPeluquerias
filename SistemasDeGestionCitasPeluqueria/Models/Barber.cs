namespace SistemasDeGestionCitasPeluqueria.Models;

public class Barber
{
    public int Id { get; set; }                       // BARBERO.id
    public string Name { get; set; } = string.Empty;  // BARBERO.nombre
    public string? Specialty { get; set; }            // BARBERO.especialidad
    public string? PhotoUrl { get; set; }             // BARBERO.foto_url
}
