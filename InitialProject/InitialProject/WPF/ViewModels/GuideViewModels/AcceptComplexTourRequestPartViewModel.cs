using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class AcceptComplexTourRequestPartViewModel : ViewModelBase
    {
        #region PROPERTIES
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged(nameof(SelectedDate));
                }
            }
        }

        public TourRequest TourRequest { get; }

        private readonly User _guide;

        private readonly TourService _tourService;
        private readonly CheckpointService _checkpointService;
        private readonly UserService _userService;
        private readonly TourRequestService _tourRequestService;
        private readonly Window _view;
        #endregion
        public AcceptComplexTourRequestPartViewModel(TourRequest tourRequest, User guide, Window view)
        {
            _tourService = new TourService();
            _checkpointService = new CheckpointService();
            _userService = new UserService();
            _tourRequestService = new TourRequestService();

            _guide = guide;
            _view = view;

            TourRequest = tourRequest;
            SelectedDate = TourRequest.StartDate;

            ConfirmCommand = new RelayCommand(ConfirmCommand_Execute, ConfirmCommand_CanExecute);
        }

        #region COMMANDS
        public RelayCommand ConfirmCommand { get; }

        public void ConfirmCommand_Execute(object? parameter)
        {
            if (MessageBox.Show("Are you sure you want to accept this tour request?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.No) return;

            var tour = _tourService.Create($"Tour for user \"{(_userService.GetById(TourRequest.UserId)).Username}\"", TourRequest.Location.Country, TourRequest.Location.City, TourRequest.Description, TourRequest.Language, TourRequest.GuestsNumber, SelectedDate, 1, null, _guide.Id);
            _checkpointService.Create($"Start checkpoint for tour {tour.Name}", tour);
            _checkpointService.Create($"End checkpoint for tour {tour.Name}", tour);
            _tourRequestService.AcceptRequest(TourRequest);

            MessageBox.Show("Tour request accepted", "Success", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
            _view.Close();
        }

        public bool ConfirmCommand_CanExecute(object? parameter)
        {
            return _tourService.IsGuideFree(_guide, SelectedDate);
        }
        #endregion

    }
}
