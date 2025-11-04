using SistemasDeGestionCitasPeluqueria.PageModels;
using System.Threading;

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
        var review = await ReviewDialogPage.ShowAsync();
        if (review is not null)
        {
            try
            {
                await _vm.AddAsync(review, _cts?.Token ?? CancellationToken.None);
            }
            catch (OperationCanceledException) { }
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