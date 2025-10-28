using SistemasDeGestionCitasPeluqueria.PageModels;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ServicesPage : ContentPage
{
    private readonly ServicesPageModel _vm;

    public ServicesPage(ServicesPageModel viewModel)
    {
        _vm = viewModel;
        BindingContext = _vm;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_vm.Services.Count == 0)
        {
            await _vm.LoadAsync();
        }
    }
}