namespace SistemasDeGestionCitasPeluqueria.Models;

public class User
{
    public int? Id { get; set; }
    public string? Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Role { get; set; } // e.g., "Admin", "Barber", "FrontDesk"
    public int? BarberId { get; set; } // Linked barber (optional)
    public DateTimeOffset? LastAccess { get; set; }
}
