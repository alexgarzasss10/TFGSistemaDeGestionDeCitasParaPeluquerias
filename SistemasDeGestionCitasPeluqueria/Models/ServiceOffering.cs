namespace SistemasDeGestionCitasPeluqueria.Models;

public class ServiceOffering
{
    public int Id { get; set; }                          // SERVICIO.id
    public int BarbershopId { get; set; }                // SERVICIO.barberia_id 
    public int CategoryId { get; set; }                  // SERVICIO.categoria_id 
    public string Name { get; set; } = string.Empty;     // SERVICIO.nombre
    public string? Description { get; set; }             // SERVICIO.descripcion
    public decimal Price { get; set; }                   // SERVICIO.precio
    public int DurationMinutes { get; set; }             // SERVICIO.duracion_minutos
    public bool IsActive { get; set; } = true;           // SERVICIO.activo
}