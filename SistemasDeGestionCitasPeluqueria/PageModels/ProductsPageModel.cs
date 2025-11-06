using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels
{
    public partial class ProductsPageModel(IInventoryService inventoryService, IProductCategoryService categoryService) : ObservableObject
    {
        private readonly IInventoryService _inventoryService = inventoryService;
        private readonly IProductCategoryService _categoryService = categoryService;
        private List<InventoryItem> _all = [];

        [ObservableProperty] private ObservableCollection<InventoryItem> products = [];
        [ObservableProperty] private ObservableCollection<string> categories = ["Todos"];
        [ObservableProperty] private string? selectedCategory = "Todos";
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string? error;

        [RelayCommand]
        public async Task LoadAsync(CancellationToken ct = default)
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                Error = null;

                // 1) Cargar categorías si aún no están cargadas 
                if (Categories.Count <= 1)
                {
                    var cats = await _categoryService.GetAllAsync(ct);
                    var names = new[] { "Todos" }.Concat(cats.OrderBy(c => c.Order).Select(c => c.Name));
                    Categories = new ObservableCollection<string>(names);
                    // Mantener la selección en "Todos" si no hay otra
                    SelectedCategory ??= "Todos";
                }

                // 2) Cargar productos
                var items = await _inventoryService.GetAllAsync(ct);
                _all = items.ToList();
                ApplyFilter();
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) { Error = ex.Message; }
            finally { IsBusy = false; }
        }

        // Se invoca al tocar un chip
        [RelayCommand]
        private void SelectCategory(string? category)
        {
            if (string.IsNullOrWhiteSpace(category)) return;
            if (!string.Equals(SelectedCategory, category, StringComparison.Ordinal))
                SelectedCategory = category;
        }

        partial void OnSelectedCategoryChanged(string? value) => ApplyFilter();

        private void ApplyFilter()
        {
            var cat = SelectedCategory ?? "Todos";
            IEnumerable<InventoryItem> query = _all;

            if (!string.Equals(cat, "Todos", StringComparison.OrdinalIgnoreCase))
            {
                query = _all.Where(p => MatchesCategory(p, cat));
            }

            Products = new ObservableCollection<InventoryItem>(query);
        }

        private static bool MatchesCategory(InventoryItem p, string cat)
        {
            var text = $"{p.Name} {p.Brand} {p.Description}".ToLowerInvariant();

            return cat switch
            {
                "Champús" => text.Contains("champú") || text.Contains("shampoo"),
                "Ceras y Pomadas" => text.Contains("cera") || text.Contains("pomada") || text.Contains("mate"),
                "Cuidado de Barba" => text.Contains("barba") || text.Contains("beard"),
                "Tratamientos" => text.Contains("mascarilla") || text.Contains("tratam"),
                "Afeitado" => text.Contains("afeit") || text.Contains("aftershave") || text.Contains("shav"),
                "Polvos y Sprays" => text.Contains("spray") || text.Contains("polvo") || text.Contains("polvos") || text.Contains("gel"),
                "Accesorios y Herramientas" => text.Contains("tijeras") || text.Contains("peine") || text.Contains("cepillo") || text.Contains("toalla"),
                _ => true
            };
        }
    }
}
