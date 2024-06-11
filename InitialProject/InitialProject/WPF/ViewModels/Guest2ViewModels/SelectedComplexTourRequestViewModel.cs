using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class SelectedComplexTourRequestViewModel : ViewModelBase
    {
        #region PROPERTIES
        public User LoggedUser { get; set; }
        

        private ObservableCollection<TourRequest> _acceptedComplexTourParts;
        public ObservableCollection<TourRequest> AcceptedComplexTourParts
        {
            get => _acceptedComplexTourParts;
            set
            {
                if (value != _acceptedComplexTourParts)
                {
                    _acceptedComplexTourParts = value;
                    OnPropertyChanged(nameof(AcceptedComplexTourParts));
                }
            }
        }

        private ObservableCollection<TourRequest> _unnacceptedComplexTourParts;
        public ObservableCollection<TourRequest> UnacceptedComplexTourParts
        {
            get => _unnacceptedComplexTourParts;
            set
            {
                if (value != _unnacceptedComplexTourParts)
                {
                    _unnacceptedComplexTourParts = value;
                    OnPropertyChanged(nameof(UnacceptedComplexTourParts));
                }
            }
        }

        private ComplexTourRequest selectedComplexTourRequest;

        private readonly ComplexTourPartService _complexTourPartService;
        private readonly ComplexTourRequestService _complexTourRequestService;
        private readonly TourRequestService _tourRequestService;
        private readonly TourService _tourService;
        #endregion

        public SelectedComplexTourRequestViewModel(User user, ComplexTourRequest complexTourRequest)
        {
            LoggedUser = user;
            selectedComplexTourRequest = complexTourRequest;
            _complexTourPartService = new ComplexTourPartService();
            _complexTourRequestService = new ComplexTourRequestService();
            _tourRequestService = new TourRequestService();
            _tourService = new TourService();

            AcceptedComplexTourParts = new ObservableCollection<TourRequest>();
            UnacceptedComplexTourParts = new ObservableCollection<TourRequest>();

            HomeButtonCommand = new RelayCommand(HomeButtonCommand_Execute);

            LoadAcceptedRequests();
            LoadUnacceptedRequests();
        }

        public void LoadAcceptedRequests()
        {
            foreach(var part in _complexTourPartService.GetAll())
            {
                if(part.ComplexTourId == selectedComplexTourRequest.Id)
                {
                    var tempRequest = _tourRequestService.GetById(part.TourRequestId);
                    if(tempRequest.Status == TourRequestStatus.ACCEPTED)
                    {
                        foreach(var tour in _tourService.GetTours())
                        {
                            if(tempRequest.LocationId == tour.LocationId && tempRequest.Language == tour.Language && tempRequest.GuestsNumber < tour.MaxGuests && DateTime.Compare(tempRequest.StartDate, tour.StartTime) < 0 && DateTime.Compare(tempRequest.EndDate, tour.StartTime.AddHours(tour.Duration)) > 0)
                            {
                                tempRequest.StartDate = tour.StartTime;
                                tempRequest.EndDate = tour.StartTime.AddDays(tour.Duration);
                                AcceptedComplexTourParts.Add(tempRequest);
                            }
                        }
                    }
                }
            }
        }

        public void LoadUnacceptedRequests()
        {
            foreach(var part in _complexTourPartService.GetAll())
            {
                if (part.ComplexTourId == selectedComplexTourRequest.Id)
                {
                    var tempRequest = _tourRequestService.GetById(part.TourRequestId);
                    if(tempRequest.Status != TourRequestStatus.ACCEPTED)
                    UnacceptedComplexTourParts.Add(tempRequest);
                }
            }
        }


        #region COMMANDS
        public RelayCommand HomeButtonCommand { get; }

        public void HomeButtonCommand_Execute(object? parameter)
        {
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
        }
        #endregion

    }
}
