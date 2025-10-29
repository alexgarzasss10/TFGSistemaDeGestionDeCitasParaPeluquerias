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

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("ServiceId", out var id)) _vm.ServiceId = (int)id;
        if (query.TryGetValue("ServiceName", out var name)) _vm.ServiceName = (string)name;
        if (query.TryGetValue("Price", out var price)) _vm.Price = (decimal)price;
        if (query.TryGetValue("DurationMinutes", out var dur)) _vm.DurationMinutes = (int)dur;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.LoadAsync();
    }
}