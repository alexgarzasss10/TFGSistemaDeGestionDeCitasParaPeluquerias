using SistemasDeGestionCitasPeluqueria.PageModels;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainPageModel _vm;

    public MainPage(MainPageModel viewModel)
    {
        _vm = viewModel;
        BindingContext = _vm;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Carga inicial de datos
        if (_vm.Services.Count == 0 || _vm.Barbers.Count == 0 || _vm.FeaturedProducts.Count == 0)
        {
            await _vm.LoadHomeAsync();
        }
    }
}
