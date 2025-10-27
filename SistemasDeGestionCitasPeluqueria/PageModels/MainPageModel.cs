using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels;

public partial class MainPageModel(
    IServiceOfferingService serviceOfferingService,
    IBarberService barberService,
    IInventoryService inventoryService
) : ObservableObject
{
    private readonly IServiceOfferingService _serviceOfferingService = serviceOfferingService;
    private readonly IBarberService _barberService = barberService;
    private readonly IInventoryService _inventoryService = inventoryService;

    [ObservableProperty] private ObservableCollection<ServiceOffering> services = [];
    [ObservableProperty] private ObservableCollection<Barber> barbers = [];
    [ObservableProperty] private ObservableCollection<InventoryItem> featuredProducts = [];
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? error;

    [RelayCommand]
    public async Task LoadHomeAsync(CancellationToken ct = default)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Error = null;

            // Lanza en paralelo
            var servicesTask = _serviceOfferingService.GetAllAsync(ct);
            var barbersTask = _barberService.GetAllAsync(ct);
            var productsTask = _inventoryService.GetFeaturedAsync(6, ct);

            // Espera a que todas terminen
            await Task.WhenAll(servicesTask, barbersTask, productsTask);

            // Recupera resultados (ya completados)
            var svc = await servicesTask;
            var brb = await barbersTask;
            var prods = await productsTask;

            Services = new ObservableCollection<ServiceOffering>(svc);
            Barbers = new ObservableCollection<Barber>(brb);
            FeaturedProducts = new ObservableCollection<InventoryItem>(prods);
        }
        catch (OperationCanceledException)
        {
            // Cancelación: opcional manejar silenciosamente
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
