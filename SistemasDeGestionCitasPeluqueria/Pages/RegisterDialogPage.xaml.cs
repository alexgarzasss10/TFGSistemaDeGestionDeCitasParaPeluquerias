using System.Threading.Tasks;
using SistemasDeGestionCitasPeluqueria.PageModels;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class RegisterDialogPage : ContentPage
{
    public RegisterDialogPage(RegisterDialogPageModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public static Task<RegisterRequestDto?> ShowAsync()
    {
        var vm = new RegisterDialogPageModel();
        var page = new RegisterDialogPage(vm);

        if (Shell.Current?.Navigation is not null)
            Shell.Current.Navigation.PushModalAsync(page);
        else
            Application.Current?.MainPage?.Navigation?.PushModalAsync(page);

        return vm.WaitForResultAsync();
    }
}