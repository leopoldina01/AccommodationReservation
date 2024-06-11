using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace InitialProject.WPF.ViewModels
{
    public class ComplexTourRequestsViewModel : ViewModelBase
    {
        #region PROPERTIES
        public ObservableCollection<ComplexTourInfo> ComplexRequestInfo { get; set; }
        private readonly User _guide;
        private readonly ComplexTourRequestService _complexTourRequestService;
        private readonly ComplexTourPartService _complexTourPartService;
        private readonly TourRequestService _tourRequestService;
        #endregion
        public ComplexTourRequestsViewModel(User guide)
        {
            _guide = guide;

            _complexTourPartService = new ComplexTourPartService();
            _complexTourRequestService = new ComplexTourRequestService();
            _tourRequestService = new TourRequestService();

            ComplexRequestInfo = new ObservableCollection<ComplexTourInfo>();

            LoadRequests();

            DetailsCommand = new RelayCommand(DetailsCommand_Execute, DetailsCommand_CanExecute);
        }

        private void LoadRequests()
        {
            foreach (var request in _complexTourRequestService.GetAll())
            {
                var parts = _complexTourPartService.GetAllByComplexRequest(request);
                ComplexRequestInfo.Add(new ComplexTourInfo(_tourRequestService.GetById(request.FirstPartId).Location, _tourRequestService.GetById(parts.First().TourRequestId).StartDate, _tourRequestService.GetById(parts.Last().TourRequestId).EndDate, request.Id));
            }
        }

        #region COMMANDS
        public RelayCommand DetailsCommand { get; }

        public void DetailsCommand_Execute(object? parameter)
        {
            var complexTourInfo = parameter as ComplexTourInfo;
            var complexTourRequestDetailsView = new ComplexTourRequestDetailsView(_complexTourRequestService.GetById(complexTourInfo.ComplexTourRequestId), _guide);
            complexTourRequestDetailsView.Show();
        }

        public bool DetailsCommand_CanExecute(object? parameter)
        {
            var complexTourInfo = parameter as ComplexTourInfo;
            return complexTourInfo is not null;
        } 
        #endregion
    }
}
