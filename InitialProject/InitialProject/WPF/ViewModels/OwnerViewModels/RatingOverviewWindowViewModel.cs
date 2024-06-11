using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RatingOverviewWindowViewModel : ViewModelBase
    {
        #region PROPERTIES
        private AccommodationReservation _selectedAccommodationReservation;
        public AccommodationReservation SelectedAccommodationReservation
        {
            get
            {
                return _selectedAccommodationReservation;
            }
            set
            {
                if (_selectedAccommodationReservation != value)
                {
                    _selectedAccommodationReservation = value;
                    OnPropertyChanged(nameof(SelectedAccommodationReservation));
                }
            }
        }

        private GuestRating _ownerRated;
        public GuestRating OwnerRated
        {
            get
            { 
                return _ownerRated; 
            }
            set
            {
                if (_ownerRated != value)
                {
                    _ownerRated = value;
                    OnPropertyChanged(nameof(OwnerRated));
                }
            }
        }

        private AccommodationRating _guestRated;

        public AccommodationRating GuestRated
        {
            get
            {
                return _guestRated;
            }
            set
            {
                if (_guestRated != value)
                {
                    _guestRated = value;
                    OnPropertyChanged(nameof(GuestRated)); 
                }
            }
        }

        private string _currentImage;
        public string CurrentImage
        {
            get
            {
                return _currentImage;
            }
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }

        private string _reserveImage;
        public string ReserveImage
        {
            get
            {
                return _reserveImage;
            }
            set
            {
                if (_reserveImage != value)
                {
                    _reserveImage = value;
                    OnPropertyChanged(nameof(ReserveImage));
                }
            }
        }

        public ObservableCollection<String> Images { get; set; }

        private readonly GuestRatingService _ratingService;
        private readonly AccommodationRatingService _accommodationRatingService;
        private readonly AccommodationRatingImageService _accommodationRatingImageService;
        #endregion

        public RatingOverviewWindowViewModel(AccommodationReservation selectedAccommodationReservation)
        {
            SelectedAccommodationReservation = selectedAccommodationReservation;
            _ratingService = new GuestRatingService();
            _accommodationRatingService = new AccommodationRatingService();
            _accommodationRatingImageService = new AccommodationRatingImageService();

            Images = new ObservableCollection<String>();

            FindOwnerRating();
            FindGuestRating();
            UploadImages();

            NextImageCommand = new RelayCommand(NextImageCommand_Execute, NextImageCommand_CanExecute);
            PreviousImageCommand = new RelayCommand(PreviousImageCommand_Execute, PreviousImageCommand_CanExecute);
        }

        private void FindOwnerRating()
        {
            OwnerRated = _ratingService.FindRatingByReservationId(SelectedAccommodationReservation.Id);

            if (OwnerRated == null)
            {
                OwnerRated = new GuestRating();
                OwnerRated.Cleanliness = 0;
                OwnerRated.FollowingTheRules = 0;
                OwnerRated.Comment = "nije ocenjen";
            }
        }

        
        private void FindGuestRating()
        {
            GuestRated = _accommodationRatingService.FindAccommodationRatingByReservationId(SelectedAccommodationReservation.Id);

            if (GuestRated == null)
            {
                GuestRated = new AccommodationRating();
                GuestRated.Cleanliness = 0;
                GuestRated.Correctness = 0;
                GuestRated.Comment = "nije ocenjen";
            }

        }

        public void UploadImages()
        {
            Images.Clear();

            AccommodationRating accommodationRating = _accommodationRatingService.FindAccommodationRatingByReservationId(SelectedAccommodationReservation.Id);

            if (accommodationRating != null)
            {
                foreach (var image in _accommodationRatingImageService.GetAllByRatingId(accommodationRating.Id))
                {
                    Images.Add(image.Url);
                }
            }

            if (Images.Count > 0)
            {
                CurrentImage = Images[0];
            }

            if (Images.Count == 0)
            {
                ReserveImage = "https://t4.ftcdn.net/jpg/04/99/93/31/360_F_499933117_ZAUBfv3P1HEOsZDrnkbNCt4jc3AodArl.jpg";
            }
        }

        #region COMMANDS
        public RelayCommand NextImageCommand { get; }
        public RelayCommand PreviousImageCommand { get; }

        public void PreviousImageCommand_Execute(object? prameter)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                if (CurrentImage == Images[i])
                {
                    CurrentImage = Images[i - 1];
                    return;
                }
            }
        }

        public bool PreviousImageCommand_CanExecute(object? parameter)
        {
            return CurrentImage != null && CurrentImage != Images.First();
        }

        public void NextImageCommand_Execute(object? parameter)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                if (CurrentImage == Images[i])
                {
                    CurrentImage = Images[i + 1];
                    return;
                }
            }
        }

        public bool NextImageCommand_CanExecute(object? parameter)
        {
            return CurrentImage != null && CurrentImage != Images.Last();
        }
        #endregion
    }
}
