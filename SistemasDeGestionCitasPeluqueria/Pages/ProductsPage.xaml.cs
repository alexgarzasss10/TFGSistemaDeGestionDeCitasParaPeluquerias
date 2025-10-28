using SistemasDeGestionCitasPeluqueria.PageModels;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ProductsPage : ContentPage
{
    private readonly ProductsPageModel _vm;

    public ProductsPage(ProductsPageModel viewModel)
    {
        _vm = viewModel;
        BindingContext = _vm;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_vm.Products.Count == 0)
        {
            await _vm.LoadAsync();
        }
    }
}