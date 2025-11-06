using System.Globalization;
using Microsoft.Maui.Controls;
using System.Text.RegularExpressions;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public sealed class ImageSourceFromStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string s || string.IsNullOrWhiteSpace(s))
            return null;

        try
        {
            // data:[mime];base64,xxxxx
            if (s.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                var comma = s.IndexOf(',');
                if (comma >= 0)
                    s = s[(comma + 1)..];

                // Limpia espacios/CR/LF que rompen el FromBase64String
                s = s.Trim();
                s = Regex.Replace(s, @"\s+", string.Empty);

                var bytes = System.Convert.FromBase64String(s);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }

            // URL absoluta (http/https)
            if (Uri.TryCreate(s, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return ImageSource.FromUri(uri);
            }

            // Heurística: base64 sin prefijo
            if (LooksLikeBase64(s))
            {
                var cleaned = Regex.Replace(s.Trim(), @"\s+", string.Empty);
                var bytes = System.Convert.FromBase64String(cleaned);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }

            // Fichero/alias local
            return ImageSource.FromFile(s);
        }
        catch
        {
            // Evita romper el binding y no muestra imágenes aleatorias
            return null;
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();

    private static bool LooksLikeBase64(string s)
    {
        if (s.Contains("://", StringComparison.Ordinal)) return false;
        if (s.Length < 64) return false;
        if (s.Length % 4 != 0) return false;

        foreach (var ch in s)
        {
            if (char.IsLetterOrDigit(ch) || ch is '+' or '/' or '=' or '\r' or '\n')
                continue;
            return false;
        }
        return true;
    }
}