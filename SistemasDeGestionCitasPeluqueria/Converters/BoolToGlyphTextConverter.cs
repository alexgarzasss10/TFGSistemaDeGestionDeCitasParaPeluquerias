using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public class BoolToGlyphTextConverter : IValueConverter
{
    // parameter: texto descriptivo (ej. "Al menos 8 caracteres")
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var ok = value as bool? ?? false;
        var text = parameter?.ToString() ?? string.Empty;
        var glyph = ok ? "✓" : "•";
        return $"{glyph} {text}";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}