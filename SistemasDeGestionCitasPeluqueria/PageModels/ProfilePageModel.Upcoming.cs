using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.PageModels;

public partial class ProfilePageModel
{
    [ObservableProperty] private ObservableCollection<BookingDto> upcomingBookings = new();
    [ObservableProperty] private BookingDto? nextBooking;
    [ObservableProperty] private bool hasUpcoming;

    // Estadísticas calculadas
    [ObservableProperty] private int totalBookings;
    [ObservableProperty] private int upcomingBookingsCount;
    [ObservableProperty] private int completedBookingsCount;
    [ObservableProperty] private int cancelledBookingsCount;

    private ObservableCollection<BookingDto>? _subscribedUpcoming;

    partial void OnUpcomingBookingsChanged(ObservableCollection<BookingDto> value)
    {
        if (!ReferenceEquals(_subscribedUpcoming, value))
        {
            if (_subscribedUpcoming is not null)
                _subscribedUpcoming.CollectionChanged -= UpcomingCollectionChanged;
            _subscribedUpcoming = value;
            if (_subscribedUpcoming is not null)
                _subscribedUpcoming.CollectionChanged += UpcomingCollectionChanged;
        }
        UpdateUpcomingSnapshot();
        RecalcStats();
    }

    partial void OnBookingsChanged(ObservableCollection<BookingDto> value)
    {
        // Cuando cambia el histórico recalculamos estadísticas
        RecalcStats();
    }

    partial void OnNextBookingChanged(BookingDto? value)
    {
        HasUpcoming = value is not null;
    }

    private void UpcomingCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateUpcomingSnapshot();
        RecalcStats();
    }

    private void UpdateUpcomingSnapshot()
    {
        NextBooking = UpcomingBookings?.FirstOrDefault();
    }

    private async Task LoadUpcomingInternalAsync(CancellationToken ct = default)
    {
        try
        {
            var list = await _bookingService.GetMyUpcomingAsync(10, new[] { "confirmed", "confirmada", "pendiente", "pendiente" }, ct);
            UpcomingBookings = new ObservableCollection<BookingDto>(list);
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) { Error = ex.Message; }
    }

    private static string NormalizeStatus(string? s)
        => string.IsNullOrWhiteSpace(s) ? "" : s.Trim().ToLowerInvariant();

    private void RecalcStats()
    {
        // Total
        TotalBookings = Bookings?.Count ?? 0;
        // Próximas
        UpcomingBookingsCount = UpcomingBookings?.Count ?? 0;

        // Contar completadas / canceladas con variantes posibles (es/en)
        var completedSet = new[] { "completed", "completada" };
        var cancelledSet = new[] { "cancelled", "cancelada" };

        CompletedBookingsCount = Bookings?.Count(b => completedSet.Contains(NormalizeStatus(b.Status))) ?? 0;
        CancelledBookingsCount = Bookings?.Count(b => cancelledSet.Contains(NormalizeStatus(b.Status))) ?? 0;
    }
}