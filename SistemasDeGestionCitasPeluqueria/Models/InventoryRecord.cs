using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;

public class InventoryRecord
{
    public int BarbershopId { get; set; }                // INVENTARIO.barberia_id (PK parte 1)
    public int ProductId { get; set; }                   // INVENTARIO.producto_id (PK parte 2)
    public int Stock { get; set; }                       // INVENTARIO.stock
    public bool IsVisible { get; set; } = true;          // INVENTARIO.visible
    public int Order { get; set; }                       // INVENTARIO.orden
}
