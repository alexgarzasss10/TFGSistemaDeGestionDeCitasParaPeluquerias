using SistemasDeGestionCitasPeluqueria.Pages;
namespace SistemasDeGestionCitasPeluqueria
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            InitializeRouting();
        }

        private void InitializeRouting()
        {
            //Routing.RegisterRoute("home/person_details", typeof(PersonDetailPage));
            //Routing.RegisterRoute("home/character_details", typeof(CharacterDetailPage));
            //Routing.RegisterRoute("home/artist_details", typeof(ArtistDetailPage));
        }
    }
}
