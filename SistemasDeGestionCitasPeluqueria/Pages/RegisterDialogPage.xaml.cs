using System;
using System.Linq;
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

    // Validación en vivo de contraseña + confirmación
    void OnPasswordChanged(object? sender, TextChangedEventArgs e)
    {
        var pwd = PasswordEntry.Text ?? string.Empty;
        var confirm = ConfirmPasswordEntry.Text ?? string.Empty;

        var (len, upper, lower, digit, symbol) = ValidatePassword(pwd);

        PasswordHints.IsVisible = true;
        UpdateHintLabel(ReqLenLbl, "Al menos 8 caracteres", len);
        UpdateHintLabel(ReqUpperLbl, "Una mayúscula (A-Z)", upper);
        UpdateHintLabel(ReqLowerLbl, "Una minúscula (a-z)", lower);
        UpdateHintLabel(ReqDigitLbl, "Un número (0-9)", digit);
        UpdateHintLabel(ReqSymbolLbl, "Un símbolo (!@#$…)", symbol);

        if (!string.IsNullOrEmpty(confirm))
        {
            var match = pwd == confirm;
            ConfirmHintLbl.Text = match ? "Las contraseñas coinciden" : "Las contraseñas no coinciden";
            ConfirmHintLbl.TextColor = match
                ? (Color)Application.Current!.Resources["SaveAccent"]
                : (Color)Application.Current!.Resources["ErrorAlertText"];
            ConfirmHintLbl.IsVisible = true;
        }
        else
        {
            ConfirmHintLbl.IsVisible = false;
        }
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

        var (len, upper, lower, digit, symbol) = ValidatePassword(password);
        if (!(len && upper && lower && digit && symbol))
        {
            SetError("La contraseña no cumple los requisitos.");
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

    // Reglas de contraseña
    static (bool len, bool upper, bool lower, bool digit, bool symbol) ValidatePassword(string pwd)
        => (pwd.Length >= 8,
            pwd.Any(char.IsUpper),
            pwd.Any(char.IsLower),
            pwd.Any(char.IsDigit),
            pwd.Any(ch => !char.IsLetterOrDigit(ch)));

    void UpdateHintLabel(Label label, string text, bool ok)
    {
        label.Text = $"{(ok ? "✓" : "•")} {text}";
        label.TextColor = ok
            ? (Color)Application.Current!.Resources["SaveAccent"]
            : (Color)Application.Current!.Resources["TextSecondary"];
    }

    void SetError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }

    static bool IsValidEmail(string value) => MailAddress.TryCreate(value, out _);
}