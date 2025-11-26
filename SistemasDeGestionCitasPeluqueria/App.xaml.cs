using Microsoft.Extensions.DependencyInjection;
using SistemasDeGestionCitasPeluqueria.Pages;

namespace SistemasDeGestionCitasPeluqueria
{
    public partial class App : Application
    {
        private readonly IServiceProvider _services;

        public App(IServiceProvider services)
        {
            InitializeComponent();
            _services = services;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var loginPage = _services.GetRequiredService<LoginPage>();
            return new Window(loginPage);
        }
    }
}