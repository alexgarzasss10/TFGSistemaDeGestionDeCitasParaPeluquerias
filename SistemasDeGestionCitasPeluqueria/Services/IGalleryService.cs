using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services;

public interface IGalleryService
{
    Task<IReadOnlyList<GalleryItem>> GetAllAsync(CancellationToken ct = default);
}
