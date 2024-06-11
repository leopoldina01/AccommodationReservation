using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class RatingGuestFormViewModel : ViewModelBase, IDataErrorInfo
    {
        #region PROPERTIES
        public AccommodationReservation SelectedReservation { get; set; }

        private string _cleanliness;
        public string Cleanliness
        {
            get => _cleanliness;
            set
            {
                if (value != _cleanliness)
                {
                    _cleanliness = value;
                    OnPropertyChanged("Cleanliness");
                }
            }
        }

        private string _followingTheRules;
        public string FollowingTheRules
        {
            get => _followingTheRules;
            set
            {
                if (value != _followingTheRules)
                {
                    _followingTheRules = value;
                    OnPropertyChanged("FollowingTheRules");
                }
            }
        }

        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        private int _theOneWhoIsRatedId;
        private int _reservationId;
        private int _raterId;

        private readonly string[] _validatedProperties =
        {
            "Cleanliness",
            "FollowingTheRules",
            "Comment"
        };

        private readonly ReservationRequestService _requestService;
        private readonly GuestRatingRepository _ratingRepository;

        private readonly Window _ratingGuestForm;
        #endregion

        public RatingGuestFormViewModel(Window ratingGuestForm, GuestRatingRepository ratingRepository, AccommodationReservation selectedReservation, int ownerId) 
        { 
            SelectedReservation = selectedReservation;
            _theOneWhoIsRatedId = SelectedReservation.GuestId;
            _reservationId = SelectedReservation.Id;
            _raterId = ownerId;

            _requestService = new ReservationRequestService();
            _ratingRepository = ratingRepository;

            _ratingGuestForm = ratingGuestForm;

            LoadAccommodation();

            RateGuestCommand = new RelayCommand(RateGuestCommand_Execute, RateGuestCommand_CanExecute);
            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
        }

        private void LoadAccommodation()
        {
            SelectedReservation = _requestService.LoadAccommodation(SelectedReservation);
        }

        public bool IsValid
        {
            get
            {
                if (Cleanliness == null)
                {
                    return false;
                }
                else if (FollowingTheRules == null)
                {
                    return false;
                }
                else if (Comment == null)
                {
                    return false;
                }
                else if (Comment == "")
                {
                    return false;
                }

                return true;
            }
            
        }

        #region VALIDATION
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                if (columnName.Equals(nameof(Cleanliness)))
                {
                    if (string.IsNullOrEmpty(Cleanliness))
                    {
                        return "Review is required";
                    }
                }
                else if (columnName.Equals(nameof(FollowingTheRules)))
                {
                    if (string.IsNullOrEmpty(FollowingTheRules))
                    {
                        return "Review is required";
                    }
                }
                else if (columnName.Equals(nameof(Comment)))
                {
                    if (string.IsNullOrEmpty(Comment))
                    {
                        return "Comment is required";
                    }
                }
                return null;
            }
        }
        #endregion
        
        #region COMMANDS
        public RelayCommand RateGuestCommand { get; }
        public RelayCommand CloseWindowCommand { get; }

        public bool RateGuestCommand_CanExecute(object? parameter)
        {
            return IsValid && _ratingRepository.GetAll().Find(r => r.ReservationId == _reservationId) == null;
        }

        public void RateGuestCommand_Execute(object? parameter)
        {
            _ratingRepository.Save(Cleanliness, FollowingTheRules, Comment, _theOneWhoIsRatedId, _raterId, _reservationId);
            _ratingGuestForm.Close();
        }

        public void CloseWindowCommand_Execute(object? parameter)
        {
            _ratingGuestForm.Close();
        }

        #endregion
    }
}
