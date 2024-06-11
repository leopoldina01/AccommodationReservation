using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class OwnerProfileOverviewViewModel : ViewModelBase
    {
        #region PROPERTIES
        private int _numberOfRatings;
        public int NumberOfRagings
        {
            get
            {
                return _numberOfRatings;
            }
            set
            {
                if (_numberOfRatings != value)
                {
                    _numberOfRatings = value;
                    OnPropertyChanged(nameof(NumberOfRagings));
                }
            }
        }

        private double _totalRating;
        public double TotalRating
        {
            get
            {
                return _totalRating;
            }
            set
            {
                if (_totalRating != value)
                {
                    _totalRating = value;
                    OnPropertyChanged(nameof(TotalRating));
                }
            }
        }

        private User _owner;

        public User Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged(nameof(Owner));
                }
            }
        }

        private readonly int _ownerId;

        private readonly Window _ownerProfileOverview;
        private readonly AccommodationRatingService _accommodationRatingService;
        private readonly UserService _userService;
        private readonly SetOwnerRoleService _setOwnerRoleService;
        #endregion

        public OwnerProfileOverviewViewModel(Window ownerProfileOverview, int ownerId)
        {
            _ownerProfileOverview = ownerProfileOverview;
            _accommodationRatingService = new AccommodationRatingService();
            _setOwnerRoleService = new SetOwnerRoleService();
            _userService = new UserService();
            _ownerId = ownerId;
            
            SetOwnerRole();
            FindOwner();

            CloseWindowCommand = new RelayCommand(CloseWindowCommand_Execute);
        }

        private void CalculateNumberOfRatings()
        {
            NumberOfRagings = _setOwnerRoleService.CalculateNumberOfRatings(_ownerId);
        }

        private void CalculateTotalRating()
        {
            TotalRating = Math.Round(_setOwnerRoleService.CalculateTotalRating(_ownerId), 2);
        }

        private void FindOwner()
        {
            Owner = _userService.FindOwnerById(_ownerId);
        }

        private void SetOwnerRole()
        {
            CalculateNumberOfRatings();
            CalculateTotalRating();

            _setOwnerRoleService.SetOwnerRole(_ownerId);
        }

        #region COMMANDS
        public RelayCommand CloseWindowCommand { get; }
        
        public void CloseWindowCommand_Execute(object? parameter)
        {
            _ownerProfileOverview.Close();
        }
        #endregion
    }
}
