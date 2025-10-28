using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ReviewDialogPage : ContentPage
{
    private readonly TaskCompletionSource<ServiceReview?> _tcs = new();
    private int _rating = 0;

    public ReviewDialogPage()
    {
        InitializeComponent();
    }

    public static Task<ServiceReview?> ShowAsync()
    {
        var page = new ReviewDialogPage();
        Shell.Current.Navigation.PushModalAsync(page);
        return page._tcs.Task;
    }

    void SetRating(int value)
    {
        _rating = value;
        Star1.Text = value >= 1 ? "★" : "☆";
        Star2.Text = value >= 2 ? "★" : "☆";
        Star3.Text = value >= 3 ? "★" : "☆";
        Star4.Text = value >= 4 ? "★" : "☆";
        Star5.Text = value >= 5 ? "★" : "☆";
    }

    async void OnPublishClicked(object sender, EventArgs e)
    {
        if (_rating <= 0)
        {
            await DisplayAlert("Reseña", "Selecciona una valoración (1-5).", "Aceptar");
            return;
        }

        var review = new ServiceReview
        {
            Rating = _rating,
            Comment = string.IsNullOrWhiteSpace(CommentEditor.Text) ? null : CommentEditor.Text.Trim(),
            UserId = 0,
            AppointmentId = 0
        };

        _tcs.TrySetResult(review);
        await Navigation.PopModalAsync();
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(null);
        await Navigation.PopModalAsync();
    }

    void OnStar1Tapped(object? s, TappedEventArgs e) => SetRating(1);
    void OnStar2Tapped(object? s, TappedEventArgs e) => SetRating(2);
    void OnStar3Tapped(object? s, TappedEventArgs e) => SetRating(3);
    void OnStar4Tapped(object? s, TappedEventArgs e) => SetRating(4);
    void OnStar5Tapped(object? s, TappedEventArgs e) => SetRating(5);
}