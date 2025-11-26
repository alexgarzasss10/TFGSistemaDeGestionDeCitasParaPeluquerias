using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels
{
    public partial class ServicesPageModel(IServiceOfferingService serviceOfferingService) : ObservableObject
    {
        private readonly IServiceOfferingService _serviceOfferingService = serviceOfferingService;

        [ObservableProperty] private ObservableCollection<ServiceOffering> services = [];
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
                var all = await _serviceOfferingService.GetAllAsync(ct);
                Services = new ObservableCollection<ServiceOffering>(all);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) { Error = ex.Message; }
            finally { IsBusy = false; }
        }

        [RelayCommand]
        private async Task ReserveAsync(ServiceOffering? service)
        {
            if (service is null) return;
            // Navega a la página de reserva pasando parámetros
            await Shell.Current.GoToAsync("booking", new Dictionary<string, object>
            {
                ["ServiceId"] = service.Id,
                ["ServiceName"] = service.Name,
                ["Price"] = service.Price,
                ["DurationMinutes"] = service.DurationMinutes
            });
        }
    }
}
