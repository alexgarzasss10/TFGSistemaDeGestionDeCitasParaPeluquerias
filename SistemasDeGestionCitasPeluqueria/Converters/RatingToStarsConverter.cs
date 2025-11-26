using Microsoft.Maui.Controls;
using System.Globalization;

namespace SistemasDeGestionCitasPeluqueria.Converters;

public sealed class RatingToStarsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var rating = value is double d ? d : value is int i ? i : 0;
        var filled = (int)Math.Round(rating, MidpointRounding.AwayFromZero);
        return new string('★', Math.Clamp(filled, 0, 5)) + new string('☆', Math.Clamp(5 - filled, 0, 5));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
}
