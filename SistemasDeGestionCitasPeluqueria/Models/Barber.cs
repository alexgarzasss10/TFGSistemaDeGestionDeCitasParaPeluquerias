namespace SistemasDeGestionCitasPeluqueria.Models;


public class Barber
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Specialties { get; set; }
    public bool? IsActive { get; set; }
}
