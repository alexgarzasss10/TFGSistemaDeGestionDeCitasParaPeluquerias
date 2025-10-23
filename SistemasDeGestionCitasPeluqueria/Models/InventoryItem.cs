namespace SistemasDeGestionCitasPeluqueria.Models;

public class InventoryItem
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Quantity { get; set; }
    public decimal? PurchasePrice { get; set; }
    public decimal? SalePrice { get; set; }
    public string? Supplier { get; set; }
}