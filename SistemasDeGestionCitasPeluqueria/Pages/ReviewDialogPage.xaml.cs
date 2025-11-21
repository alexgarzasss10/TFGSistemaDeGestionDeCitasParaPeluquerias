using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.PageModels;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class ReviewDialogPage : ContentPage
{
    // Constructor usado internamente por el ViewModel.
    public ReviewDialogPage(ReviewDialogPageModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    // Método estático conservado por compatibilidad.
    public static Task<SistemasDeGestionCitasPeluqueria.Models.ServiceReview?> ShowAsync(int? barberId = null, int? serviceId = null, string? userName = null)
        => ReviewDialogPageModel.ShowAsync(barberId, serviceId, userName);
}