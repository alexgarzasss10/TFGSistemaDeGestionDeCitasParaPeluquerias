namespace SistemasDeGestionCitasPeluqueria.Models;

public class User
{
    public int Id { get; set; }                           // USUARIO.id
    public string Name { get; set; } = string.Empty;      // USUARIO.nombre
    public string Email { get; set; } = string.Empty;     // USUARIO.email 
    public string Phone { get; set; } = string.Empty;     // USUARIO.telefono
    public string PasswordHash { get; set; } = string.Empty; // USUARIO.password_hash
    public bool IsGuest { get; set; } = false;            // USUARIO.es_invitado
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow; // USUARIO.creado_en
}
