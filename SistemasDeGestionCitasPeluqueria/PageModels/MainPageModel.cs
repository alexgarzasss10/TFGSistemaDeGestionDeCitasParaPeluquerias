using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels;

public partial class MainPageModel(
    IServiceOfferingService serviceOfferingService,
    IBarberService barberService,
    IInventoryService inventoryService,
    IBarbershopService barbershopService,
    IGalleryService galleryService
) : ObservableObject
{
    private readonly IServiceOfferingService _serviceOfferingService = serviceOfferingService;
    private readonly IBarberService _barberService = barberService;
    private readonly IInventoryService _inventoryService = inventoryService;
    private readonly IBarbershopService _barbershopService = barbershopService;
    private readonly IGalleryService _galleryService = galleryService;

    [ObservableProperty] private ObservableCollection<ServiceOffering> services = [];
    [ObservableProperty] private ObservableCollection<Barber> barbers = [];
    [ObservableProperty] private ObservableCollection<InventoryItem> featuredProducts = [];
    [ObservableProperty] private ObservableCollection<GalleryItem> gallery = [];

    [ObservableProperty] private string barbershopName = string.Empty;
    [ObservableProperty] private string heroImageUrl = string.Empty;

    
    [ObservableProperty] private string openingText = string.Empty;
    [ObservableProperty] private string openingLine1Label = string.Empty;
    [ObservableProperty] private string openingLine1Value = string.Empty;
    [ObservableProperty] private string openingLine2Label = string.Empty;
    [ObservableProperty] private string openingLine2Value = string.Empty;

    [ObservableProperty] private string addressText = string.Empty;
    [ObservableProperty] private string contactText = string.Empty;

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

            Barbershop? shop = null;
            try
            {
                shop = await _barbershopService.GetAsync(ct);
                if (shop is not null)
                {
                    BarbershopName = shop.Name;
                    HeroImageUrl = shop.Images?.FirstOrDefault() ?? "https://picsum.photos/1200/400?blur=2";

                    AddressText = string.IsNullOrWhiteSpace(shop.Address)
                        ? string.Empty
                        : $"{shop.Address}\n{shop.City}, {shop.Country}";

                    ContactText = $"{shop.Phone}\n{shop.Email}";

                    
                    OpeningText = BuildOpeningText(shop.OpeningHours);
                   
                    BuildOpeningLines(shop.OpeningHours);
                }
            }
            catch (Exception ex) { Error = ex.Message; }

            var servicesTask = _serviceOfferingService.GetAllAsync(ct);
            var barbersTask = _barberService.GetAllAsync(ct);
            var productsTask = _inventoryService.GetFeaturedAsync(6, ct);
            var galleryTask = _galleryService.GetAllAsync(ct);

            try { Services = new ObservableCollection<ServiceOffering>(await servicesTask); } catch (Exception ex) { Error = ex.Message; }
            try { Barbers = new ObservableCollection<Barber>(await barbersTask); } catch (Exception ex) { Error = ex.Message; }
            try { FeaturedProducts = new ObservableCollection<InventoryItem>(await productsTask); } catch (Exception ex) { Error = ex.Message; }
            try { Gallery = new ObservableCollection<GalleryItem>(await galleryTask); } catch (Exception ex) { Error = ex.Message; }
        }
        catch (OperationCanceledException) { }
        finally { IsBusy = false; }
    }

    private static string BuildOpeningText(OpeningHours? oh)
    {
        if (oh is null) return "Horario no disponible";
        string Today(DayHours? d) => d is null ? "Cerrado" : $"{d.Open} - {d.Close}";
        var now = DateTime.Now.DayOfWeek;
        var today = GetDay(oh, now);
        var sunday = GetDay(oh, DayOfWeek.Sunday);
        return $"Hoy: {Today(today)}\nDom: {Today(sunday)}";
    }

    private void BuildOpeningLines(OpeningHours? oh)
    {
        
        if (oh is null)
        {
            OpeningLine1Label = "Hoy:";
            OpeningLine1Value = "Horario no disponible";
            OpeningLine2Label = "Dom:";
            OpeningLine2Value = "—";
            return;
        }

        string Today(DayHours? d) => d is null ? "Cerrado" : $"{d.Open} - {d.Close}";
        var now = DateTime.Now.DayOfWeek;
        OpeningLine1Label = "Hoy:";
        OpeningLine1Value = Today(GetDay(oh, now));
        OpeningLine2Label = "Dom:";
        OpeningLine2Value = Today(GetDay(oh, DayOfWeek.Sunday));
    }

    private static DayHours? GetDay(OpeningHours oh, DayOfWeek d) => d switch
    {
        DayOfWeek.Monday => oh.Monday,
        DayOfWeek.Tuesday => oh.Tuesday,
        DayOfWeek.Wednesday => oh.Wednesday,
        DayOfWeek.Thursday => oh.Thursday,
        DayOfWeek.Friday => oh.Friday,
        DayOfWeek.Saturday => oh.Saturday,
        DayOfWeek.Sunday => oh.Sunday,
        _ => null
    };
}
