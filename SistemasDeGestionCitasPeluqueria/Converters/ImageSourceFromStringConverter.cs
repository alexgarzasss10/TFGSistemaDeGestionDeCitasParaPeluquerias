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

        s = s.Trim();

        try
        {
            // Corrige entradas mal formadas como "data:https://..." o "data:http://..."
            if (s.StartsWith("data:http", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("data:https", StringComparison.OrdinalIgnoreCase))
            {
                s = s[5..]; // quita "data:"
            }

            // Data URI válida: data:image/<type>;base64,<payload>
            if (s.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                if (!Regex.IsMatch(s, @"^data:image\/[a-zA-Z0-9.+-]+;base64,", RegexOptions.IgnoreCase))
                    return null;

                var comma = s.IndexOf(',');
                if (comma < 0) return null;

                var base64 = s[(comma + 1)..];
                base64 = Regex.Replace(base64, @"\s+", string.Empty);
                var bytes = System.Convert.FromBase64String(base64);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }

            // URL absoluta http/https (sin caché)
            if (Uri.TryCreate(s, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                return new UriImageSource
                {
                    Uri = uri,
                    CachingEnabled = false,
                    CacheValidity = TimeSpan.Zero
                };
            }

            // Base64 “pura”
            if (LooksLikeBase64(s))
            {
                var cleaned = Regex.Replace(s, @"\s+", string.Empty);
                var bytes = System.Convert.FromBase64String(cleaned);
                return ImageSource.FromStream(() => new MemoryStream(bytes));
            }

            // Archivo local
            return ImageSource.FromFile(s);
        }
        catch
        {
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