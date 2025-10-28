using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemasDeGestionCitasPeluqueria.Models;
using SistemasDeGestionCitasPeluqueria.Services;

namespace SistemasDeGestionCitasPeluqueria.PageModels
{
    public partial class ReviewsPageModel(IReviewService reviewService) : ObservableObject
    {
        private readonly IReviewService _reviewService = reviewService;

        [ObservableProperty] private ObservableCollection<ServiceReview> reviews = [];
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string? error;

        [ObservableProperty] private double averageRating;
        [ObservableProperty] private int totalReviews;
        [ObservableProperty] private int count5;
        [ObservableProperty] private int count4;
        [ObservableProperty] private int count3;
        [ObservableProperty] private int count2;
        [ObservableProperty] private int count1;

        [RelayCommand]
        public async Task LoadAsync(CancellationToken ct = default)
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                Error = null;
                var all = await _reviewService.GetAllAsync(ct);
                Reviews = new ObservableCollection<ServiceReview>(all);
                RecalcStats();
            }
            catch (OperationCanceledException) { }
            catch (Exception ex) { Error = ex.Message; }
            finally { IsBusy = false; }
        }

        public async Task AddAsync(ServiceReview review, CancellationToken ct = default)
        {
            await _reviewService.AddAsync(review, ct);
            Reviews.Insert(0, review);
            RecalcStats();
        }

        private void RecalcStats()
        {
            TotalReviews = Reviews.Count;
            if (TotalReviews == 0)
            {
                AverageRating = 0;
                Count1 = Count2 = Count3 = Count4 = Count5 = 0;
                return;
            }

            AverageRating = Math.Round(Reviews.Average(r => r.Rating), 1);
            Count5 = Reviews.Count(r => r.Rating == 5);
            Count4 = Reviews.Count(r => r.Rating == 4);
            Count3 = Reviews.Count(r => r.Rating == 3);
            Count2 = Reviews.Count(r => r.Rating == 2);
            Count1 = Reviews.Count(r => r.Rating == 1);
        }
    }
}
