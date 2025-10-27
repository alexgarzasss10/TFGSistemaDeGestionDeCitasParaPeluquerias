using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services.Fake;

public sealed class FakeInventoryService : IInventoryService
{
    private static readonly List<InventoryItem> _items =
    [
        new() { Id = 1, Name = "Champú Anticaspa", Brand = "Head&Shoulders", Description = "Limpieza profunda", Price = 6.99m, Stock = 20, ImageUrl = "https://images.unsplash.com/photo-1598440947619-2c35fc9aa908?q=80&w=800" },
        new() { Id = 2, Name = "Mascarilla Hidratante", Brand = "L'Oréal", Description = "Hidratación intensa", Price = 12.50m, Stock = 12, ImageUrl = "https://images.unsplash.com/photo-1601004890684-d8cbf643f5f2?q=80&w=800" },
        new() { Id = 3, Name = "Cera Mate", Brand = "American Crew", Description = "Acabado natural", Price = 9.90m, Stock = 30, ImageUrl = "https://images.unsplash.com/photo-1512203492609-8f7f06f1f1c3?q=80&w=800" },
        new() { Id = 4, Name = "Aceite Barba", Brand = "Proraso", Description = "Suaviza y nutre", Price = 14.00m, Stock = 15, ImageUrl = "https://images.unsplash.com/photo-1600180758890-6b94519a8ba6?q=80&w=800" },
        new() { Id = 5, Name = "Champú Volumen", Brand = "Schwarzkopf", Description = "Mayor cuerpo", Price = 8.50m, Stock = 10, ImageUrl = "https://images.unsplash.com/photo-1608245449230-4ac19066d5a5?q=80&w=800" },
        new() { Id = 6, Name = "Gel Fijador", Brand = "Taft", Description = "Fijación fuerte", Price = 5.40m, Stock = 25, ImageUrl = "https://images.unsplash.com/photo-1556228209-05c5258e1e8f?q=80&w=800" },
    ];

    public async Task<IReadOnlyList<InventoryItem>> GetFeaturedAsync(int take = 6, CancellationToken ct = default)
    {
        await Task.Delay(300, ct);
        return _items.Take(take).ToList();
    }

    public async Task<IReadOnlyList<InventoryItem>> GetAllAsync(CancellationToken ct = default)
    {
        await Task.Delay(300, ct);
        return _items;
    }
}