using SistemasDeGestionCitasPeluqueria.PageModels;
using System.Threading;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ProductsPage : ContentPage
{
    private readonly ProductsPageModel _vm;
    private CancellationTokenSource? _cts;

    public ProductsPage(ProductsPageModel viewModel)
    {
        InitializeComponent();
        _vm = viewModel;
        BindingContext = _vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        if (_vm.Products.Count == 0)
        {
            try
            {
                await _vm.LoadAsync(_cts.Token);
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ProductsPage load error: {ex}");
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