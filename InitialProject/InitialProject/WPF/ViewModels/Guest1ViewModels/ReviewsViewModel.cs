using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ReviewsViewModel : ViewModelBase
    {
        #region PROPERTIES
        public ObservableCollection<GuestRating> Ratings { get; set; }

        private readonly GuestRatingService _guestRatingService;
        private readonly User _user;
        #endregion

        public ReviewsViewModel(User user)
        {
            _user = user;
            _guestRatingService = new GuestRatingService();

            Ratings = new ObservableCollection<GuestRating>();

            LoadReviews();
        }

        private void LoadReviews()
        {
            var reviews = _guestRatingService.FindAllRatingsByGuestId(_user.Id);
            reviews = _guestRatingService.RemoveNonmutualRatings(reviews);
            _guestRatingService.LoadReservations(reviews);
            foreach (var review in reviews)
            {
                Ratings.Add(review);
            }
        }
    }
}
