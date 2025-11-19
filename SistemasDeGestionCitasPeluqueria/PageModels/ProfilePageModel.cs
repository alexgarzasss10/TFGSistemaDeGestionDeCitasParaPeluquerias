using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.Pages;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels;

public partial class ProfilePageModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _services;
    private readonly IUserService _userService;

    public ProfilePageModel(IAuthService authService, IServiceProvider services, IUserService userService)
    {
        _authService = authService;
        _services = services;
        _userService = userService;
    }

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? error;

    // Datos de perfil
    [ObservableProperty] private string username = string.Empty;
    [ObservableProperty] private string? name;
    [ObservableProperty] private string? email;
    [ObservableProperty] private string? phone;
    [ObservableProperty] private string clientSinceText = string.Empty;

    // Fecha de nacimiento (usada por DatePicker)
    [ObservableProperty] private DateTime birthDate = DateTime.Today.AddYears(-30);

    // Rango válido para fecha de nacimiento
    public DateTime MinBirthDate { get; } = DateTime.Today.AddYears(-120);
    public DateTime MaxBirthDate { get; } = DateTime.Today;

    public string DisplayName => !string.IsNullOrWhiteSpace(Name) ? Name! : Username;
    partial void OnNameChanged(string? value) => OnPropertyChanged(nameof(DisplayName));
    partial void OnUsernameChanged(string value) => OnPropertyChanged(nameof(DisplayName));

    [RelayCommand]
    public async Task LoadAsync(CancellationToken ct = default)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Error = null;

            var me = await _userService.GetMeAsync(ct);
            if (me is not null)
            {
                Username = me.Username;
                Name = me.Name;
                Email = me.Email;
                Phone = me.Phone;
                ClientSinceText = me.CreatedAt.Year > 0 ? $"Cliente desde {me.CreatedAt.Year}" : string.Empty;

                // Asignar la fecha de nacimiento recibida (o usar un valor por defecto)
                BirthDate = me.BirthDate ?? DateTime.Today.AddYears(-30);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    [RelayCommand]
    public async Task SaveAsync(CancellationToken ct = default)
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            Error = null;

            var req = new UpdateUserProfileRequest
            {
                Name = string.IsNullOrWhiteSpace(Name) ? null : Name!.Trim(),
                Email = string.IsNullOrWhiteSpace(Email) ? null : Email!.Trim(),
                Phone = string.IsNullOrWhiteSpace(Phone) ? null : Phone!.Trim(),
                // Ahora sí enviamos la fecha de nacimiento (solo la parte de fecha)
                BirthDate = BirthDate == default ? null : BirthDate.Date
            };

            await _userService.UpdateMeAsync(req, ct);

            // Refrescar datos desde el backend
            await LoadAsync(ct);

            await Application.Current!.MainPage!.DisplayAlert("Perfil", "Los cambios se guardaron correctamente.", "Aceptar");
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            Error = ex.Message;
            await Application.Current!.MainPage!.DisplayAlert("Error", ex.Message, "Aceptar");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task LogoutAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Error = null;

            await _authService.LogoutAsync();

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
