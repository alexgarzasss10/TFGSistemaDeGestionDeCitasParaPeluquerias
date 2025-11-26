using SistemasDeGestionCitasPeluqueria.PageModels;
using System.Threading;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly ProfilePageModel _vm;
    private CancellationTokenSource? _cts;

    public ProfilePage(ProfilePageModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        try { await _vm.LoadAsync(_cts.Token); }
        catch (OperationCanceledException) { }
    }

    protected override void OnDisappearing()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
        base.OnDisappearing();
    }
}