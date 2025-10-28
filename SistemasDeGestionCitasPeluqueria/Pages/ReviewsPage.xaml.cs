using SistemasDeGestionCitasPeluqueria.PageModels;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ReviewsPage : ContentPage
{
    private readonly ReviewsPageModel _vm;

    public ReviewsPage(ReviewsPageModel viewModel)
    {
        _vm = viewModel;
        BindingContext = _vm;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_vm.Reviews.Count == 0)
            await _vm.LoadAsync();
    }

    private async void OnAddReviewClicked(object sender, EventArgs e)
    {
        var review = await ReviewDialogPage.ShowAsync();
        if (review is not null)
        {
            await _vm.AddAsync(review);
        }
    }
}