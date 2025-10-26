namespace SistemasDeGestionCitasPeluqueria.Models;

public class ServiceOffering
{
    public int Id { get; set; }                          // SERVICIO.id
    public string Name { get; set; } = string.Empty;     // SERVICIO.nombre
    public string? Description { get; set; }             // SERVICIO.descripcion
    public decimal Price { get; set; }                   // SERVICIO.precio
    public int DurationMinutes { get; set; }             // SERVICIO.duracion_minutos
}