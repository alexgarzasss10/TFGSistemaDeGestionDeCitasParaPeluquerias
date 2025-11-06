using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemasDeGestionCitasPeluqueria.Models;

namespace SistemasDeGestionCitasPeluqueria.Services.Fake
{
    public sealed class FakeReviewService : IReviewService
    {
        private static readonly List<ServiceReview> _reviews = new()
        {
            new ServiceReview { Id = 1, UserId = 101, AppointmentId = 2001, Rating = 5, Comment = "Excelente servicio, muy profesionales.", Date = DateTimeOffset.UtcNow.AddDays(-2) },
            new ServiceReview { Id = 2, UserId = 102, AppointmentId = 2002, Rating = 5, Comment = "Trabajo impecable, totalmente recomendado.", Date = DateTimeOffset.UtcNow.AddDays(-7) },
            new ServiceReview { Id = 3, UserId = 103, AppointmentId = 2003, Rating = 4, Comment = "Muy buen trato y resultado.", Date = DateTimeOffset.UtcNow.AddDays(-10) },
            new ServiceReview { Id = 4, UserId = 104, AppointmentId = 2004, Rating = 5, Comment = "Ambiente agradable y atención de primera.", Date = DateTimeOffset.UtcNow.AddDays(-21) },
            new ServiceReview { Id = 5, UserId = 105, AppointmentId = 2005, Rating = 5, Comment = "Siempre salgo muy satisfecho.", Date = DateTimeOffset.UtcNow.AddDays(-30) },
            new ServiceReview { Id = 6, UserId = 106, AppointmentId = 2006, Rating = 3, Comment = "Bien, pero podría mejorar la puntualidad.", Date = DateTimeOffset.UtcNow.AddDays(-45) },
        };

        public async Task<IReadOnlyList<ServiceReview>> GetAllAsync(CancellationToken ct = default)
        {
            await Task.Delay(200, ct);
            
            return _reviews.OrderByDescending(r => r.Date).ToList();
        }

        public async Task AddAsync(ServiceReview review, CancellationToken ct = default)
        {
            await Task.Delay(150, ct);
            review.Id = _reviews.Count == 0 ? 1 : _reviews.Max(r => r.Id) + 1;
            review.Date = DateTimeOffset.UtcNow;
            _reviews.Add(review);
        }
    }
}
