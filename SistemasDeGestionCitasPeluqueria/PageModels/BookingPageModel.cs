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

namespace SistemasDeGestionCitasPeluqueria.PageModels;

public partial class BookingPageModel(IBarberService barberService) : ObservableObject
{
    private readonly IBarberService _barberService = barberService;

    [ObservableProperty] private int serviceId;
    [ObservableProperty] private string serviceName = string.Empty;
    [ObservableProperty] private decimal price;
    [ObservableProperty] private int durationMinutes;

    [ObservableProperty] private ObservableCollection<Barber> barbers = [];
    [ObservableProperty] private Barber? selectedBarber;

    [ObservableProperty] private DateTime selectedDate = DateTime.Today;

    [ObservableProperty] private ObservableCollection<TimeSpan> availableSlots = [];
    [ObservableProperty] private TimeSpan? selectedSlot;

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
            var list = await _barberService.GetAllAsync(ct);
            Barbers = new ObservableCollection<Barber>(list);
            UpdateSlots(); // en caso de que ya hubiese fecha/selección
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    partial void OnSelectedBarberChanged(Barber? value) => UpdateSlots();
    partial void OnSelectedDateChanged(DateTime value) => UpdateSlots();

    private void UpdateSlots()
    {
        AvailableSlots.Clear();
        SelectedSlot = null;

        if (SelectedBarber is null) return;

        // Demo: genera huecos cada 60 min de 09:00 a 20:00, respetando la duración
        var open = new TimeSpan(9, 0, 0);
        var close = new TimeSpan(20, 0, 0);
        var step = TimeSpan.FromMinutes(60);
        var serviceDuration = TimeSpan.FromMinutes(Math.Max(15, DurationMinutes > 0 ? DurationMinutes : 30));

        // Evita horas pasadas si la fecha es hoy
        var now = DateTime.Now;
        if (SelectedDate.Date == now.Date && now.TimeOfDay > open)
        {
            // redondea al siguiente bloque
            var minutes = (int)Math.Ceiling(now.TimeOfDay.TotalMinutes / step.TotalMinutes) * (int)step.TotalMinutes;
            open = TimeSpan.FromMinutes(minutes);
        }

        for (var t = open; t + serviceDuration <= close; t += step)
            AvailableSlots.Add(t);
    }

    [RelayCommand]
    private async Task ConfirmAsync()
    {
        if (SelectedBarber is null || SelectedSlot is null)
            return;

        // Aquí podrías navegar a una pantalla de confirmación o llamar a un servicio
        await Shell.Current.DisplayAlert(
            "Reserva",
            $"Servicio: {ServiceName}\nBarbero: {SelectedBarber.Name}\nFecha: {SelectedDate:dd/MM/yyyy}\nHora: {SelectedSlot:hh\\:mm}",
            "OK");
    }
}
