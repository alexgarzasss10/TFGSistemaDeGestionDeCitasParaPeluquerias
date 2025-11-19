using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.Pages;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels;

public partial class ProfilePageModel : ObservableObject
{
    private readonly IAuthService _authService;
    private readonly IServiceProvider _services;
    private readonly IUserService _userService;
    private readonly IBookingService _bookingService;

    public ProfilePageModel(IAuthService authService, IServiceProvider services, IUserService userService, IBookingService bookingService)
    {
        _authService = authService;
        _services = services;
        _userService = userService;
        _bookingService = bookingService;
    }

    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? error;

    // Datos de perfil
    [ObservableProperty] private string username = string.Empty;
    [ObservableProperty] private string? name;
    [ObservableProperty] private string? email;
    [ObservableProperty] private string? phone;
    [ObservableProperty] private string clientSinceText = string.Empty;

    // Nueva: URL de la foto (http/https o data:image/...;base64)
    [ObservableProperty] private string? photoUrl;

    // Fecha de nacimiento (usada por DatePicker)
    [ObservableProperty] private DateTime birthDate = DateTime.Today.AddYears(-30);

    // Rango válido para fecha de nacimiento
    public DateTime MinBirthDate { get; } = DateTime.Today.AddYears(-120);
    public DateTime MaxBirthDate { get; } = DateTime.Today;

    // Historial de citas del usuario
    [ObservableProperty] private ObservableCollection<BookingDto> bookings = new();

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
                BirthDate = me.BirthDate ?? DateTime.Today.AddYears(-30);
                PhotoUrl = me.PhotoUrl;

                // Cargar historial de citas del usuario
                await LoadBookingsInternalAsync(ct);

                // NUEVO: cargar próximas citas
                await LoadUpcomingInternalAsync(ct);
            }
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) { Error = ex.Message; }
        finally { IsBusy = false; }
    }

    private async Task LoadBookingsInternalAsync(CancellationToken ct = default)
    {
        try
        {
            var list = await _bookingService.GetMineAsync(ct);
            Bookings = new ObservableCollection<BookingDto>(list);
        }
        catch (OperationCanceledException) { }
        catch (Exception ex) { Error = ex.Message; }
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

    // Nuevo: seleccionar y subir foto
    [RelayCommand]
    public async Task ChangePhotoAsync(CancellationToken ct = default)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Error = null;

            string? fileName;
            Stream? stream;

            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                var file = await MediaPicker.Default.PickPhotoAsync();
                if (file is null) return;

                fileName = file.FileName;
                stream = await file.OpenReadAsync();
            }
            else
            {
                var file = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecciona una foto",
                    FileTypes = FilePickerFileType.Images
                });
                if (file is null) return;

                fileName = file.FileName;
                stream = await file.OpenReadAsync();
            }

            using (stream)
            {
                var url = await _userService.UploadPhotoAsync(stream, fileName!, ct);
                if (!string.IsNullOrWhiteSpace(url))
                {
                    PhotoUrl = url;
                    await Application.Current!.MainPage!.DisplayAlert("Perfil", "Foto actualizada.", "Aceptar");
                }
            }
        }
        catch (FeatureNotSupportedException)
        {
            await Application.Current!.MainPage!.DisplayAlert("No soportado", "La selección de fotos no está soportada en este dispositivo.", "Aceptar");
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

    // Limpia los datos sensibles de la vista de perfil inmediatamente
    private void ClearSensitiveData()
    {
        Username = string.Empty;
        Name = null;
        Email = null;
        Phone = null;
        ClientSinceText = string.Empty;
        PhotoUrl = null;
        BirthDate = DateTime.Today.AddYears(-30);
        Error = null;
    }

    [RelayCommand]
    public async Task LogoutAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            Error = null;

            // 1) Limpiar UI inmediatamente para evitar mostrar datos antiguos
            ClearSensitiveData();

            // 2) Navegar al Login de forma inmediata en el hilo UI
            var loginPage = _services.GetRequiredService<LoginPage>();
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Application.Current!.MainPage = loginPage;
            });

            // 3) Borrar tokens/estado en almacenamiento y avisar al AuthService
            // (AuthService probablemente ya lo haga; repetir es seguro)
            if (_services.GetService<ITokenStore>() is ITokenStore tokenStore)
            {
                await tokenStore.ClearAsync();
            }

            await _authService.LogoutAsync();
        }
        catch (Exception ex)
        {
            // si algo falla, dejamos el error para mostrarlo (no restauramos datos)
            Error = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
