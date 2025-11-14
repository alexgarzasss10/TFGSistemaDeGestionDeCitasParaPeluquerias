using SistemasDeGestionCitasPeluqueria.PageModels;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly ProfilePageModel _vm;

    public ProfilePage(ProfilePageModel vm)
    {
        InitializeComponent();
        BindingContext = _vm = vm;
    }
}