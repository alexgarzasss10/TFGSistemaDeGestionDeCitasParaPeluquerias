namespace SistemasDeGestionCitasPeluqueria.Models;

public class Client
{
    public int? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public DateTimeOffset? RegisteredAt { get; set; }
    public string? AvatarUrl { get; set; }
}
