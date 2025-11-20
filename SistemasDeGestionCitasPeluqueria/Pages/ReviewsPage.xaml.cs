using SistemasDeGestionCitasPeluqueria.PageModels;
using System.Threading;
using System.Net.Http;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ReviewsPage : ContentPage
{
    private readonly ReviewsPageModel _vm;
    private CancellationTokenSource? _cts;

    public ReviewsPage(ReviewsPageModel viewModel)
    {
        _vm = viewModel;
        BindingContext = _vm;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        if (_vm.Reviews.Count == 0)
            try
            {
                await _vm.LoadAsync(_cts.Token);
            }
            catch (OperationCanceledException) { }
    }

    private async void OnAddReviewClicked(object sender, EventArgs e)
    {
        // Obtén el nombre del usuario actual (si hay sesión activa)
        string? userName = null;
        try
        {
            userName = await _vm.GetCurrentUserNameAsync(_cts?.Token ?? CancellationToken.None);
        }
        catch { userName = null; }

        var review = await ReviewDialogPage.ShowAsync(userName: userName);
        if (review is not null)
        {
            try
            {
                await _vm.AddAsync(review, _cts?.Token ?? CancellationToken.None);
            }
            catch (OperationCanceledException) { }
            catch (HttpRequestException ex)
            {
                await DisplayAlert("Error al crear reseña", ex.Message, "Aceptar");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error inesperado", ex.Message, "Aceptar");
            }
        }
    }

    protected override void OnDisappearing()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
        base.OnDisappearing();
    }
}