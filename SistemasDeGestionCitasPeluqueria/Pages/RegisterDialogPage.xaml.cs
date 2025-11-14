using System.Linq;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class RegisterDialogPage : ContentPage
{
    private readonly TaskCompletionSource<RegistrationData?> _tcs = new();

    public RegisterDialogPage()
    {
        InitializeComponent();
    }

    // DTO simple para devolver datos de registro (simulado)
    public record RegistrationData(string? FullName, string? Email, string? Username, string? Password);

    public static Task<RegistrationData?> ShowAsync()
    {
        var page = new RegisterDialogPage();

        // Intentamos empujar el modal por diferentes rutas de navegación; si no hay, abrimos una nueva ventana
        var nav = Shell.Current?.Navigation;
        if (nav is null)
        {
            var window = Application.Current?.Windows.FirstOrDefault();
            if (window?.Page?.Navigation != null)
                nav = window.Page.Navigation;
            else if (Application.Current?.MainPage?.Navigation != null)
                nav = Application.Current.MainPage.Navigation;
        }

        if (nav != null)
            nav.PushModalAsync(page);
        else
            Application.Current?.OpenWindow(new Window(page));

        return page._tcs.Task;
    }

    async void OnRegisterClicked(object sender, EventArgs e)
    {
        // Validación básica (sólo UI)
        if (string.IsNullOrWhiteSpace(UserEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Registro", "Rellena usuario, email y contraseña.", "Aceptar");
            return;
        }

        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            await DisplayAlert("Registro", "Las contraseñas no coinciden.", "Aceptar");
            return;
        }

        var data = new RegistrationData(
            FullNameEntry.Text?.Trim(),
            EmailEntry.Text?.Trim(),
            UserEntry.Text?.Trim(),
            PasswordEntry.Text);

        _tcs.TrySetResult(data);
        await Navigation.PopModalAsync();
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(null);
        await Navigation.PopModalAsync();
    }
}