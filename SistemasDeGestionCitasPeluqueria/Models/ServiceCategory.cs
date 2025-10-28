using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;

public class ServiceCategory
{
    public int Id { get; set; }                          // CATEGORIA_SERVICIO.id
    public string Name { get; set; } = string.Empty;     // CATEGORIA_SERVICIO.nombre
    public int Order { get; set; }                       // CATEGORIA_SERVICIO.orden
}
