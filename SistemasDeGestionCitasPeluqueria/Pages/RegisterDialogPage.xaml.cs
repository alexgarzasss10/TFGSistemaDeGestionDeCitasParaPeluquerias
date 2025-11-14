using System;
using System.Net.Mail;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.Pages;

public partial class RegisterDialogPage : ContentPage
{
    private readonly TaskCompletionSource<RegisterRequestDto?> _tcs = new();

    public RegisterDialogPage()
    {
        InitializeComponent();
    }

    public static Task<RegisterRequestDto?> ShowAsync()
    {
        var page = new RegisterDialogPage();
        if (Shell.Current?.Navigation is not null)
            Shell.Current.Navigation.PushModalAsync(page);
        else
            Application.Current?.MainPage?.Navigation?.PushModalAsync(page);

        return page._tcs.Task;
    }

    async void OnRegisterClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        var username = UserEntry.Text?.Trim() ?? string.Empty;
        var password = PasswordEntry.Text ?? string.Empty;
        var confirm = ConfirmPasswordEntry.Text ?? string.Empty;
        var emailText = EmailEntry.Text?.Trim();
        var fullName = string.IsNullOrWhiteSpace(FullNameEntry.Text) ? null : FullNameEntry.Text.Trim();

        if (username.Length < 3)
        {
            SetError("El usuario debe tener al menos 3 caracteres.");
            return;
        }

        if (password.Length < 8)
        {
            SetError("La contraseña debe tener al menos 8 caracteres.");
            return;
        }

        if (password != confirm)
        {
            SetError("Las contraseñas no coinciden.");
            return;
        }

        string? email = null;
        if (!string.IsNullOrWhiteSpace(emailText))
        {
            if (!IsValidEmail(emailText))
            {
                SetError("Introduce un email válido o déjalo vacío.");
                return;
            }
            email = emailText;
        }

        var dto = new RegisterRequestDto
        {
            Username = username,
            Password = password,
            Email = email,
            Name = fullName
        };

        _tcs.TrySetResult(dto);
        await Navigation.PopModalAsync();
    }

    async void OnCancelClicked(object sender, EventArgs e)
    {
        _tcs.TrySetResult(null);
        await Navigation.PopModalAsync();
    }

    void SetError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }

    static bool IsValidEmail(string value) => MailAddress.TryCreate(value, out _);
}