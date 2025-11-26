namespace SistemasDeGestionCitasPeluqueria.Services;

internal static class UrlHelper
{
    public static string? EnsureAbsolute(string? url, Uri? baseAddress)
    {
        if (string.IsNullOrWhiteSpace(url))
            return null;
        url = url.Trim();
        if (url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("data:image"))
            return url;
        if (baseAddress is null)
            return url;
        var ba = baseAddress.ToString().TrimEnd('/');
        return $"{ba}/{url.TrimStart('/')}";
    }
}