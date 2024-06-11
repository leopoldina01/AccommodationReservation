using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest2Views;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class TourRequestViewModel : ViewModelBase
    {
        #region PROPERTIES
        public User LoggedUser { get; set; }

        private ObservableCollection<TourRequest> _tourRequests;
        public ObservableCollection<TourRequest> TourRequests
        {
            get => _tourRequests;
            set
            {
                if (value != _tourRequests)
                {
                    _tourRequests = value;
                    OnPropertyChanged(nameof(TourRequests));
                }
            }
        }

        private TourRequest _selectedTourRequest;
        public TourRequest SelectedTourRequest
        {
            get => _selectedTourRequest;
            set
            {
                if (value != _selectedTourRequest)
                {
                    _selectedTourRequest = value;
                    OnPropertyChanged(nameof(SelectedTourRequest));
                }
            }
        }

        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;
        private readonly UserService _userService;
        #endregion
        public TourRequestViewModel(User user)
        {
            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();
            _userService = new UserService();
            TourRequests = new ObservableCollection<TourRequest>();
            LoggedUser = user;

            ShowTourRequestFormCommand = new RelayCommand(ShowTourRequestFormCommand_Execute);
            HomeCommand = new RelayCommand(HomeCommand_Execute);

            LoadRequests();
        }

        public void LoadRequests()
        {
            foreach(var request in _tourRequestService.GetAll())
            {
                request.Location = _locationService.GetLocationById(request.LocationId);
                request.Guide = _userService.GetById(request.GuideId);
                
                if (DateTime.Compare(request.RequestArrivalDate.AddDays(2), DateTime.Now) >= 0)
                {
                    TourRequests.Add(request);
                }
                else
                {
                    request.Status = TourRequestStatus.DECLINED;
                    _tourRequestService.Update(request);
                    TourRequests.Add(request);
                }
            }
        }

        #region COMMANDS
        public RelayCommand ShowTourRequestFormCommand { get; }
        public RelayCommand HomeCommand { get; }

        public void HomeCommand_Execute(object? parameter)
        {
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
        }

        public void ShowTourRequestFormCommand_Execute(object? parameter)
        {
            TourRequestFormView tourRequestFormView = new TourRequestFormView(LoggedUser);
            //tourRequestFormView.Show();
        }
        #endregion
    }
}
