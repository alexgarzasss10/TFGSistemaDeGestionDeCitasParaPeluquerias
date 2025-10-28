using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;


public class Product
{
    public int Id { get; set; }                          // PRODUCTO.id
    public int CategoryId { get; set; }                  // PRODUCTO.categoria_id (FK CATEGORIA_PRODUCTO)
    public string Name { get; set; } = string.Empty;     // PRODUCTO.nombre
    public string? Brand { get; set; }                   // PRODUCTO.marca
    public string? Description { get; set; }             // PRODUCTO.descripcion
    public decimal DisplayedPrice { get; set; }          // PRODUCTO.precio_mostrado
    public string? ImageUrl { get; set; }                // PRODUCTO.imagen_url
    public bool IsActive { get; set; } = true;           // PRODUCTO.activo
}
