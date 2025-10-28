using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemasDeGestionCitasPeluqueria.Models;


public class Barbershop
{
    public int Id { get; set; }                          // BARBERIA.id
    public string Name { get; set; } = string.Empty;     // BARBERIA.nombre
    public string Phone { get; set; } = string.Empty;    // BARBERIA.telefono
    public string Email { get; set; } = string.Empty;    // BARBERIA.email
    public string Address { get; set; } = string.Empty;  // BARBERIA.direccion
    public string City { get; set; } = string.Empty;     // BARBERIA.ciudad
    public string Country { get; set; } = string.Empty;  // BARBERIA.pais
    public decimal Latitude { get; set; }                // BARBERIA.latitud (decimal(9,6))
    public decimal Longitude { get; set; }               // BARBERIA.longitud (decimal(9,6))
    public bool IsActive { get; set; } = true;           // BARBERIA.activo
}
