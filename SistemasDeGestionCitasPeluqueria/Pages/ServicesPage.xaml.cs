using SistemasDeGestionCitasPeluqueria.PageModels;
using System.Threading; // para CancellationTokenSource

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ServicesPage : ContentPage
{
    private readonly ServicesPageModel _vm;
    private CancellationTokenSource? _cts;

    public ServicesPage(ServicesPageModel viewModel)
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

        if (_vm.Services.Count == 0)
        {
            try
            {
                await _vm.LoadAsync(_cts.Token);
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