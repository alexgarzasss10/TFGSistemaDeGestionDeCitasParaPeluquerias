using System.Globalization;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public sealed class DurationMinutesToTextConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not int minutes) return null;
        if (minutes <= 0) return "—";

        var h = minutes / 60;
        var m = minutes % 60;

        if (h > 0 && m > 0) return $"{h} h {m} min";
        if (h > 0)           return $"{h} h";
        return $"{m} min";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}