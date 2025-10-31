using Microsoft.Maui.Controls;
using System.Globalization;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public sealed class AreEqualConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is null || values.Length < 2)
            return false;

        var a = values[0];
        var b = values[1];

        // Misma referencia o ambos null
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;

        // Comparaciones específicas y seguras
        if (a is Barber ba && b is Barber bb)
            return ba.Id == bb.Id;

        if (a is TimeSpan ta && b is TimeSpan tb)
            return ta == tb;

        if (a is DateTime da && b is DateTime db)
            return da == db;

        if (a is DateTimeOffset dao && b is DateTimeOffset dbo)
            return dao == dbo;

        // Cadenas y tipos primitivos
        if (a is string || b is string)
            return string.Equals(a?.ToString(), b?.ToString(), StringComparison.Ordinal);

        // Fallback: usa Equals por si alguno implementa IEquatable<T>
        return Equals(a, b);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
