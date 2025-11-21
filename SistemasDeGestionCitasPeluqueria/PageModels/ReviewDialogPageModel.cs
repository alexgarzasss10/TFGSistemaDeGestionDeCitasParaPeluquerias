using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.PageModels;

public class ReviewDialogPageModel : INotifyPropertyChanged
{
    private readonly TaskCompletionSource<ServiceReview?> _tcs;
    private readonly INavigation _navigation;

    private int _rating;
    private string? _comment;

    public int? BarberId { get; }
    public int? ServiceId { get; }
    public string? UserName { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ReviewDialogPageModel(INavigation navigation,
                                 TaskCompletionSource<ServiceReview?> tcs,
                                 int? barberId = null,
                                 int? serviceId = null,
                                 string? userName = null)
    {
        _navigation = navigation;
        _tcs = tcs;
        BarberId = barberId;
        ServiceId = serviceId;
        UserName = string.IsNullOrWhiteSpace(userName) ? null : userName;

        // Cambio: usar Command<object> y convertir el parámetro.
        SetRatingCommand = new Command<object>(SetRatingFromParameter);
        PublishCommand = new Command(async () => await PublishAsync(), () => CanPublish);
        CancelCommand = new Command(async () => await CancelAsync());
    }

    private void SetRatingFromParameter(object? param)
    {
        if (param is int i)
            Rating = i;
        else if (param is string s && int.TryParse(s, out var v))
            Rating = v;
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        if (name == nameof(Rating))
        {
            OnPropertyChanged(nameof(Star1Text));
            OnPropertyChanged(nameof(Star2Text));
            OnPropertyChanged(nameof(Star3Text));
            OnPropertyChanged(nameof(Star4Text));
            OnPropertyChanged(nameof(Star5Text));
            OnPropertyChanged(nameof(CanPublish));
            (PublishCommand as Command)?.ChangeCanExecute();
        }
    }

    public int Rating
    {
        get => _rating;
        set
        {
            if (_rating == value) return;
            _rating = value;
            OnPropertyChanged();
        }
    }

    public string? Comment
    {
        get => _comment;
        set
        {
            if (_comment == value) return;
            _comment = value;
            OnPropertyChanged();
        }
    }

    public string Star1Text => Rating >= 1 ? "★" : "☆";
    public string Star2Text => Rating >= 2 ? "★" : "☆";
    public string Star3Text => Rating >= 3 ? "★" : "☆";
    public string Star4Text => Rating >= 4 ? "★" : "☆";
    public string Star5Text => Rating >= 5 ? "★" : "☆";

    public bool CanPublish => Rating > 0;

    public ICommand SetRatingCommand { get; }
    public ICommand PublishCommand { get; }
    public ICommand CancelCommand { get; }

    private async Task PublishAsync()
    {
        if (!CanPublish)
        {
            await Application.Current.MainPage.DisplayAlert("Reseña", "Selecciona una valoración (1-5).", "Aceptar");
            return;
        }

        var review = new ServiceReview
        {
            Rating = Rating,
            Comment = string.IsNullOrWhiteSpace(Comment) ? null : Comment.Trim(),
            UserId = null,
            AppointmentId = null,
            BarberId = BarberId,
            ServiceId = ServiceId,
            UserName = UserName,
            Date = DateTimeOffset.UtcNow
        };

        _tcs.TrySetResult(review);
        await _navigation.PopModalAsync();
    }

    private async Task CancelAsync()
    {
        _tcs.TrySetResult(null);
        await _navigation.PopModalAsync();
    }

    public static Task<ServiceReview?> ShowAsync(int? barberId = null, int? serviceId = null, string? userName = null)
    {
        var navigation = Shell.Current?.Navigation ?? throw new InvalidOperationException("Shell.Current no está disponible.");
        var tcs = new TaskCompletionSource<ServiceReview?>();
        var vm = new ReviewDialogPageModel(navigation, tcs, barberId, serviceId, userName);
        var page = new Pages.ReviewDialogPage(vm);
        navigation.PushModalAsync(page);
        return tcs.Task;
    }
}
