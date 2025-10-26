namespace SistemasDeGestionCitasPeluqueria.Models;

public class InventoryItem
{
    public int Id { get; set; }                         // PRODUCTO.id
    public string Name { get; set; } = string.Empty;    // PRODUCTO.nombre
    public string? Brand { get; set; }                  // PRODUCTO.marca
    public string? Description { get; set; }            // PRODUCTO.descripcion
    public decimal Price { get; set; }                  // PRODUCTO.precio
    public int Stock { get; set; }                      // PRODUCTO.stock
    public string? ImageUrl { get; set; }               // PRODUCTO.imagen_url
}