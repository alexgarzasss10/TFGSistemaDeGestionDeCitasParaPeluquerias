namespace SistemasDeGestionCitasPeluqueria.Models;

public sealed class UpdateUserProfileRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? PhotoUrl { get; set; }
}