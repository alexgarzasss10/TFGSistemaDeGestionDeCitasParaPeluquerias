using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels
{
    public partial class LoginPageModel : ObservableObject
    {
        private readonly IBarbershopService _barbershopService;
        private readonly IGalleryService _galleryService;

        public LoginPageModel(IBarbershopService barbershopService, IGalleryService galleryService)
        {
            _barbershopService = barbershopService;
            _galleryService = galleryService;
        }

        [ObservableProperty] private string heroImageUrl = string.Empty;

        public async Task LoadAsync(CancellationToken ct = default)
        {
            if (!string.IsNullOrWhiteSpace(HeroImageUrl)) return;

            var shop = await _barbershopService.GetAsync(ct);
            var candidate = shop?.Images?
                .Select(SanitizePotentialDataUrl)
                .FirstOrDefault(IsSupportedImage);

            if (string.IsNullOrWhiteSpace(candidate))
            {
                var gallery = await _galleryService.GetAllAsync(ct);
                candidate = gallery?
                    .Select(g => SanitizePotentialDataUrl(g.ImageUrl))
                    .FirstOrDefault(IsSupportedImage);
            }

            HeroImageUrl = candidate ?? string.Empty;
        }

        private static bool IsSupportedImage(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            s = s.Trim();
            if (s.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase)) return true;

            if (Uri.TryCreate(s, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                return true;

            return false;
        }

        private static string? SanitizePotentialDataUrl(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            s = s.Trim();
            if (s.StartsWith("data:http", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("data:https", StringComparison.OrdinalIgnoreCase))
            {
                return s[5..];
            }
            return s;
        }
    }
}
