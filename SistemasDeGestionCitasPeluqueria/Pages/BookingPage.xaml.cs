using System;
using System.Threading;
using SistemasDeGestionCitasPeluqueria.PageModels;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class BookingPage : ContentPage, IQueryAttributable
{
    private readonly BookingPageModel _vm;

    public BookingPage(BookingPageModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }

    // Reenvía parámetros a la VM
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        _vm.ApplyQueryAttributes(query);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            await _vm.OnAppearingAsync();
        }
        catch (OperationCanceledException) { }
    }

    protected override void OnDisappearing()
    {
        _vm.OnDisappearing();
        base.OnDisappearing();
    }
}