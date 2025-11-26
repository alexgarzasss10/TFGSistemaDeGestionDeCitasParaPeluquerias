using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public sealed class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => !(value as bool? ?? false);

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => !(value as bool? ?? false);
}