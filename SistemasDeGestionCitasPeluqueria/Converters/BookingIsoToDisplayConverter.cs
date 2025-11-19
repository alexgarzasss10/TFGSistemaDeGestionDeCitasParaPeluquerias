using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public class BookingIsoToDisplayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string s || string.IsNullOrWhiteSpace(s))
            return string.Empty;

        DateTime dt;
        var formats = new[] { "yyyy-MM-dd'T'HH:mm", "yyyy-MM-dd'T'HH:mm:ss", "s", "o" };
        if (!DateTime.TryParseExact(s, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt))
        {
            if (!DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt))
            {
                return s; // fallback: devolver string original
            }
        }

        var param = (parameter as string)?.ToLowerInvariant() ?? "both";
        return param switch
        {
            "date" => dt.ToString("dd/MM/yyyy"),
            "time" => dt.ToString("HH:mm"),
            _ => dt.ToString("dd/MM/yyyy • HH:mm"),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
