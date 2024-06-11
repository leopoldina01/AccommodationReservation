using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class ComplexTourRequestViewModel : ViewModelBase
    {
        #region PROPERTIES
        public User LoggedUser { get; set; }

        private ObservableCollection<ComplexAndRegularTourRequest> _complexTourRequests;
        public ObservableCollection<ComplexAndRegularTourRequest> ComplexTourRequests
        {
            get => _complexTourRequests;
            set
            {
                if (value != _complexTourRequests)
                {
                    _complexTourRequests = value;
                    OnPropertyChanged(nameof(ComplexTourRequests));
                }
            }
        }

        private ComplexAndRegularTourRequest _selectedComplexTourRequest;
        public ComplexAndRegularTourRequest SelectedComplexTourRequest
        {
            get => _selectedComplexTourRequest;
            set
            {
                if (value != _selectedComplexTourRequest)
                {
                    _selectedComplexTourRequest = value;
                    OnPropertyChanged(nameof(SelectedComplexTourRequest));
                }
            }
        }

        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;
        private readonly UserService _userService;
        private readonly ComplexTourRequestService _complexTourRequestService;
        private readonly ComplexTourPartService _complexTourPartService;
        #endregion


        public ComplexTourRequestViewModel(User user)
        {
            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();
            _userService = new UserService();
            _complexTourRequestService = new ComplexTourRequestService();
            _complexTourPartService = new ComplexTourPartService();
            LoggedUser = user;
            ComplexTourRequests = new ObservableCollection<ComplexAndRegularTourRequest>();

            LoadRequests();

            ShowTourRequestFormCommand = new RelayCommand(ShowTourRequestFormCommand_Execute);
            ViewComplexTourCommand = new RelayCommand(ViewComplexTourCommand_Execute);
        }

        public bool HasNotAcceptedAnyRequests(ComplexTourRequest complexTourRequest)
        {
            foreach(var part in _complexTourPartService.GetAll())
            {
                if(part.ComplexTourId == complexTourRequest.Id)
                {
                    var tempRequest = _tourRequestService.GetById(part.TourRequestId);
                    if (tempRequest.Status == TourRequestStatus.ACCEPTED)
                    {
                        return false;
                    }
                }
            }
                return true;
        }

        public bool HasAcceptedAllRequests(ComplexTourRequest complexTourRequest)
        {
            foreach (var part in _complexTourPartService.GetAll())
            {
                if (part.ComplexTourId == complexTourRequest.Id)
                {
                    var tempRequest = _tourRequestService.GetById(part.TourRequestId);
                    if (tempRequest.Status == TourRequestStatus.ON_HOLD || tempRequest.Status == TourRequestStatus.DECLINED)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void CheckComplexTourStatus(ComplexTourRequest complexTourRequest)
        {
            var tempReq = _tourRequestService.GetById(complexTourRequest.FirstPartId);
            if(DateTime.Compare(DateTime.Now.AddHours(48), tempReq.StartDate) > 0 && tempReq.Status == TourRequestStatus.ON_HOLD)
            {
                if (HasNotAcceptedAnyRequests(complexTourRequest))
                {
                    complexTourRequest.Status = TourRequestStatus.DECLINED;
                    _complexTourRequestService.Update(complexTourRequest);
                }
            }

            if (HasAcceptedAllRequests(complexTourRequest))
            {
                complexTourRequest.Status = TourRequestStatus.ACCEPTED;
                _complexTourRequestService.Update(complexTourRequest);
            }
        }
        public void LoadRequests()
        {
            ComplexAndRegularTourRequest complexAndRegularTourRequest = new ComplexAndRegularTourRequest();
            foreach(var complexReq in _complexTourRequestService.GetAll())
            {
                CheckComplexTourStatus(complexReq);
                complexAndRegularTourRequest.ComplexTourRequestId = complexReq.Id;
                complexAndRegularTourRequest.Status = complexReq.Status;
                complexAndRegularTourRequest.TourRequest = _tourRequestService.GetById(complexReq.FirstPartId);
                ComplexTourRequests.Add(complexAndRegularTourRequest);
            }
        }

        #region COMMANDS

        public RelayCommand ShowTourRequestFormCommand { get; }
        public RelayCommand ViewComplexTourCommand { get; }

        public void ViewComplexTourCommand_Execute(object? parameter)
        {
            
            SelectedComplexTourRequestView selectedComplexTourRequestView = new SelectedComplexTourRequestView(LoggedUser, _complexTourRequestService.GetById(SelectedComplexTourRequest.ComplexTourRequestId));
        }

        public void ShowTourRequestFormCommand_Execute(object? parameter)
        {
            ComplexTourRequestFormView complexTourRequestFormView = new ComplexTourRequestFormView (LoggedUser);
        }

        #endregion
    }
}
