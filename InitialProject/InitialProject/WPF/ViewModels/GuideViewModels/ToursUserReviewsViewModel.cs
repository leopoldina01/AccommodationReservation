using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class ToursUserReviewsViewModel : ViewModelBase
    {
        #region PROPERTIES
        private TourReview? _selectedReview;
		public TourReview? SelectedReview
		{
			get
			{
				return _selectedReview;
			}
			set
			{
				if (_selectedReview != value)
				{
					_selectedReview = value;
					OnPropertyChanged(nameof(SelectedReview));
				}
			}
		}

        public ObservableCollection<TourReview> Reviews { get; set; }

        private readonly Tour _tour;
        private readonly TourReviewService _tourReviewService;
        #endregion

        public ToursUserReviewsViewModel(Tour tour)
        {
            _tour = tour;

            _tourReviewService = new();
            Reviews = new();

			OpenReviewCommand = new RelayCommand(OpenReviewCommand_Execute, OpenReviewCommand_CanExecute);

            LoadReviews();
        }

        private void LoadReviews()
        {
            Reviews.Clear();
            foreach (var review in _tourReviewService.GetReviewsByTour(_tour))
            {
                Reviews.Add(review);
            }
        }

        #region COMMANDS
        public RelayCommand OpenReviewCommand { get; }
        public RelayCommand CloseWindowCommand { get; }

        public void OpenReviewCommand_Execute(object? parameter)
        {
            var tourReviewDetailsView = new TourReviewDetailsView(parameter as TourReview);
            tourReviewDetailsView.Show();
        }

        public bool OpenReviewCommand_CanExecute(object? parameter)
        {
            return parameter is not null;
        }
        #endregion
    }
}
