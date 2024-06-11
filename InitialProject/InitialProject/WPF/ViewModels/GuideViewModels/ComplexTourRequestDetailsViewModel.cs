using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System.Collections.ObjectModel;

namespace InitialProject.WPF.ViewModels
{
    public class ComplexTourRequestDetailsViewModel : ViewModelBase
    {
        #region PROPERTIES
        private bool _isAccepted;
        public bool IsAccepted
        {
            get
            {
                return _isAccepted;
            }
            set
            {
                if (_isAccepted != value)
                {
                    _isAccepted = value;
                    OnPropertyChanged(nameof(IsAccepted));
                }
            }
        }
        public ObservableCollection<TourRequest> Requests { get; set; }
        private readonly ComplexTourRequestService _complexTourRequestService;
        private readonly User _guide;
        #endregion
        public ComplexTourRequestDetailsViewModel(ComplexTourRequest complexTourRequest, User guide)
        {
            _guide = guide;

            _complexTourRequestService = new ComplexTourRequestService();

            IsAccepted = false;

            Requests = new ObservableCollection<TourRequest>(_complexTourRequestService.GetAllParts(complexTourRequest));

            AcceptCommand = new RelayCommand(AcceptCommand_Execute, AcceptCommand_CanExecute);
        }

        #region COMMANDS
        public RelayCommand AcceptCommand { get; }

        public void AcceptCommand_Execute(object? parameter)
        {
            var request = parameter as TourRequest;
            var acceptComplexTourRequestPartView = new AcceptComplexTourRequestPartView(request, _guide);
            acceptComplexTourRequestPartView.Show();
            IsAccepted = true;
        }

        public bool AcceptCommand_CanExecute(object? parameter)
        {
            return parameter is not null && !IsAccepted;
        }
        #endregion
    }
}
