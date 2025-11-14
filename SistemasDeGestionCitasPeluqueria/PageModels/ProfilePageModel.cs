using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SistemasDeGestionCitasPeluqueria.Pages;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels;

public partial class ProfilePageModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _services;

    public ProfilePageModel(IAuthService authService, IServiceProvider services)
    {
        _authService = authService;
        _services = services;
    }

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string? error;

    [RelayCommand]
    public async Task LogoutAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Error = null;

            await _authService.LogoutAsync();

            // Restablece la MainPage al LoginPage resuelta desde DI
            var loginPage = _services.GetRequiredService<LoginPage>();
            Application.Current!.MainPage = loginPage;
        }
        catch (Exception ex)
        {
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
