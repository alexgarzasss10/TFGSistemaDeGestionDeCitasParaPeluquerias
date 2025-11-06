using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;

public class InventoryRecord
{
    public int BarbershopId { get; set; }                // INVENTARIO.barberia_id 
    public int ProductId { get; set; }                   // INVENTARIO.producto_id 
    public int Stock { get; set; }                       // INVENTARIO.stock
    public bool IsVisible { get; set; } = true;          // INVENTARIO.visible
    public int Order { get; set; }                       // INVENTARIO.orden
}
