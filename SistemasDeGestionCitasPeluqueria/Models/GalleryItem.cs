using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;

public class GalleryItem
{
    public int Id { get; set; }                          // GALERIA_ITEM.id
    public int BarbershopId { get; set; }                // GALERIA_ITEM.barberia_id 
    public string Title { get; set; } = string.Empty;    // GALERIA_ITEM.titulo
    public string? Description { get; set; }             // GALERIA_ITEM.descripcion
    public string ImageUrl { get; set; } = string.Empty; // GALERIA_ITEM.imagen_url

    
    public string Date { get; set; } = string.Empty;     // GALERIA_ITEM.fecha

    public bool IsVisible { get; set; } = true;          // GALERIA_ITEM.visible
    public int Order { get; set; }                       // GALERIA_ITEM.orden
    public int? ServiceId { get; set; }                  // GALERIA_ITEM.servicio_id 
    public int? BarberId { get; set; }                   // GALERIA_ITEM.barbero_id 
}
