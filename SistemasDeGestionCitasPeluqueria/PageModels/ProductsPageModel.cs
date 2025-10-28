using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels
{
    public partial class ProductsPageModel(IInventoryService inventoryService) : ObservableObject
    {
        private readonly IInventoryService _inventoryService = inventoryService;
        private List<InventoryItem> _all = [];

        [ObservableProperty] private ObservableCollection<InventoryItem> products = [];
        [ObservableProperty] private ObservableCollection<string> categories =
        [
            "Todos",
            "Champús",
            "Ceras y Pomadas",
            "Cuidado de Barba",
            "Tratamientos",
            "Afeitado",
            "Polvos y Sprays"
        ];
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

                var items = await _inventoryService.GetAllAsync(ct);
                _all = items.ToList();
                ApplyFilter();
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) { Error = ex.Message; }
            finally { IsBusy = false; }
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
                _ => true
            };
        }
    }
}
