using SistemasDeGestionCitasPeluqueria.Pages;
namespace SistemasDeGestionCitasPeluqueria;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("booking", typeof(Pages.BookingPage));
    }
}
