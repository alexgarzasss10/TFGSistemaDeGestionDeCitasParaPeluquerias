using SistemasDeGestionCitasPeluqueria.PageModels;
namespace SistemasDeGestionCitasPeluqueria.Pages;
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel viewModel)
        {
            BindingContext = viewModel;
            InitializeComponent();
    }
}
