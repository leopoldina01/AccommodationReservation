using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class MyRenovationsPageViewModel : ViewModelBase
    {
        #region PROPERTIES  
        private AccommodationRenovation _selectedRenovation;
        public AccommodationRenovation SelectedRenovation
        {
            get
            {
                return _selectedRenovation;
            }
            set
            {
                if (value != _selectedRenovation)
                {
                    _selectedRenovation = value;
                    OnPropertyChanged(nameof(SelectedRenovation));
                }
            }
        }

        private readonly AccommodationRenovationService _accommodationRenovationService;
        public static ObservableCollection<AccommodationRenovation> AccommodationRenovations { get; set; }

        private readonly int _ownerId;
        #endregion

        public MyRenovationsPageViewModel(int ownerId) 
        {
            _accommodationRenovationService = new AccommodationRenovationService();

            AccommodationRenovations = new ObservableCollection<AccommodationRenovation>();

            _ownerId = ownerId;

            LoadAccommodationRenovations();

            DeclineRenovationCommand = new RelayCommand(DeclineRenovationCommand_Execute, DeclineRenovationCommand_CanExecute);
        }

        private void LoadAccommodationRenovations()
        {
            AccommodationRenovations.Clear();

            foreach(var accommodationRenovation in _accommodationRenovationService.GetByOwnerId(_ownerId))
            {
                AccommodationRenovations.Add(accommodationRenovation);
            }
        }

        #region COMMANDS
        public RelayCommand DeclineRenovationCommand { get; }

        public bool DeclineRenovationCommand_CanExecute(object? parameter)
        {
            if (SelectedRenovation == null)
            {
                return false;
            }

            if ((SelectedRenovation.StartDate - DateTime.Now).Days < 5)
            {
                return false;
            }

            return true;
        }

        public void DeclineRenovationCommand_Execute (object? parameter)
        {
            _accommodationRenovationService.Delete(SelectedRenovation);
            LoadAccommodationRenovations();
        }
        #endregion
    }
}
