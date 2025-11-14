using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels;

public partial class BookingPageModel(IBarberService barberService) : ObservableObject
{
    private readonly IBarberService _barberService = barberService;
    private CancellationTokenSource? _cts;

    [ObservableProperty] private int serviceId;
    [ObservableProperty] private string serviceName = string.Empty;
    [ObservableProperty] private decimal price;
    [ObservableProperty] private int durationMinutes;

    [ObservableProperty] private ObservableCollection<Barber> barbers = new ObservableCollection<Barber>();
    [ObservableProperty] private Barber? selectedBarber;

    [ObservableProperty] private DateTime selectedDate = DateTime.Today;

    [ObservableProperty] private ObservableCollection<TimeSpan> availableSlots = new ObservableCollection<TimeSpan>();
    [ObservableProperty] private TimeSpan? selectedSlot;

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? error;
    [ObservableProperty] private bool hasError;

    // Notificadores parciales generados por CommunityToolkit
    partial void OnSelectedBarberChanged(Barber? value)
    {
        UpdateSlots();
        ConfirmCommand?.NotifyCanExecuteChanged();
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        UpdateSlots();
    }

    partial void OnSelectedSlotChanged(TimeSpan? value)
    {
        ConfirmCommand?.NotifyCanExecuteChanged();
    }

    partial void OnIsBusyChanged(bool value)
    {
        ConfirmCommand?.NotifyCanExecuteChanged();
    }

    partial void OnErrorChanged(string? value) => HasError = !string.IsNullOrEmpty(value);

    // Carga inicial / refresco de barberos
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
            UpdateSlots();
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    // Permite que la Page reenvíe parámetros de Shell al VM
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ServiceId", out var id) && id is int i) ServiceId = i;
        if (query.TryGetValue("ServiceName", out var name) && name is string s) ServiceName = s;
        if (query.TryGetValue("Price", out var p) && p is decimal d) Price = d;
        if (query.TryGetValue("DurationMinutes", out var dur) && dur is int dm) DurationMinutes = dm;

        // Si cambian parámetros recomputar slots
        UpdateSlots();
    }

    // Helpers de ciclo de vida (antes en el code-behind)
    public async Task OnAppearingAsync()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        try
        {
            await LoadAsync(_cts.Token);
        }
        catch (OperationCanceledException) { }
    }

    public void OnDisappearing()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private void UpdateSlots()
    {
        AvailableSlots.Clear();
        SelectedSlot = null;

        if (SelectedBarber is null) return;

        var open = new TimeSpan(9, 0, 0);
        var close = new TimeSpan(20, 0, 0);
        var step = TimeSpan.FromMinutes(60);
        var serviceDuration = TimeSpan.FromMinutes(Math.Max(15, DurationMinutes > 0 ? DurationMinutes : 30));

        var now = DateTime.Now;
        if (SelectedDate.Date == now.Date && now.TimeOfDay > open)
        {
            var minutes = (int)Math.Ceiling(now.TimeOfDay.TotalMinutes / step.TotalMinutes) * (int)step.TotalMinutes;
            open = TimeSpan.FromMinutes(minutes);
        }

        for (var t = open; t + serviceDuration <= close; t += step)
            AvailableSlots.Add(t);
    }

    // Habilita/deshabilita Confirm según selección y estado
    public bool CanConfirm() => SelectedBarber != null && SelectedSlot != null && !IsBusy;

    [RelayCommand(CanExecute = nameof(CanConfirm))]
    public async Task ConfirmAsync()
    {
        if (SelectedBarber is null || SelectedSlot is null) return;

        await Shell.Current.DisplayAlert(
            "Reserva",
            $"Servicio: {ServiceName}\nBarbero: {SelectedBarber.Name}\nFecha: {SelectedDate:dd/MM/yyyy}\nHora: {SelectedSlot:hh\\:mm}",
            "OK");
    }
}
