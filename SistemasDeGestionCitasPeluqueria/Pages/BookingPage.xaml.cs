namespace SistemasDeGestionCitasPeluqueria.Pages;

[QueryProperty(nameof(ServiceId), "ServiceId")]
[QueryProperty(nameof(ServiceName), "ServiceName")]
[QueryProperty(nameof(Price), "Price")]
[QueryProperty(nameof(DurationMinutes), "DurationMinutes")]
public partial class BookingPage : ContentPage
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationMinutes { get; set; }

    public BookingPage()
    {
        InitializeComponent();
        BindingContext = this;
    }
}