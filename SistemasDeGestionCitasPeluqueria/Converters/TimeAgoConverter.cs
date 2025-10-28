using Microsoft.Maui.Controls;
using System.Globalization;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public sealed class TimeAgoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not DateTimeOffset dto) return string.Empty;
        var span = DateTimeOffset.UtcNow - dto.ToUniversalTime();

        return span switch
        {
            { TotalMinutes: < 1 } => "Hace un momento",
            { TotalMinutes: < 60 } => $"Hace {(int)span.TotalMinutes} min",
            { TotalHours: < 24 } => $"Hace {(int)span.TotalHours} h",
            { TotalDays: < 7 } => $"Hace {(int)span.TotalDays} días",
            { TotalDays: < 30 } => $"Hace {(int)(span.TotalDays / 7)} semana(s)",
            _ => $"Hace {(int)(span.TotalDays / 30)} mes(es)"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
}
