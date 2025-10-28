using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;


public class ProductCategory
{
    public int Id { get; set; }                          // CATEGORIA_PRODUCTO.id
    public string Name { get; set; } = string.Empty;     // CATEGORIA_PRODUCTO.nombre
    public int Order { get; set; }                       // CATEGORIA_PRODUCTO.orden
}
