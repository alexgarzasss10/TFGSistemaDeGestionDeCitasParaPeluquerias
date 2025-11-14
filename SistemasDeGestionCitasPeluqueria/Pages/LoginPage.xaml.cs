using System.Linq;
using Microsoft.Maui.Controls;
using SistemasDeGestionCitasPeluqueria.PageModels;

namespace SistemasDeGestionCitasPeluqueria.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginPageModel _vm;

        public LoginPage(LoginPageModel vm)
        {
            InitializeComponent();
            _vm = vm;
            BindingContext = _vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            // Cargar la imagen al aparecer
            await _vm.LoadAsync();
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            var window = Application.Current?.Windows.FirstOrDefault();
            if (window is not null)
            {
                window.Page = new AppShell();
            }
            else
            {
                Application.Current?.OpenWindow(new Window(new AppShell()));
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            var reg = await RegisterDialogPage.ShowAsync();
            if (reg is not null)
            {
                // Resultado simulado (aquí integrarías tu lógica de registro)
                await DisplayAlert("Registro (simulado)", $"Usuario: {reg.Username}\nEmail: {reg.Email}", "Aceptar");
            }
        }
    }
}