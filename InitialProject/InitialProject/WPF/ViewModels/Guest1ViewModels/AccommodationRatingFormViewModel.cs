using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class AccommodationRatingFormViewModel : ViewModelBase
    {
        #region PROPERTIES
        private string? _comment;
        public string? Comment
        {
            get
            {
                return _comment;
            }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }
        private string? _imageUrl;
        public string? ImageUrl
        {
            get
            {
                return _imageUrl;
            }
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    OnPropertyChanged(nameof(ImageUrl));
                }
            }
        }
        private string? _currentImageUrl;
        public string? CurrentImageUrl
        {
            get
            {
                return _currentImageUrl;
            }
            set
            {
                if (_currentImageUrl != value)
                {
                    _currentImageUrl = value;
                    OnPropertyChanged(nameof(CurrentImageUrl));
                }
            }
        }

        private bool[] _cleanlinessModeArray = new bool[] { false, false, false, false, true };
        public bool[] CleanlinessModeArray
        {
            get { return _cleanlinessModeArray; }
        }
        public int CleanlinessSelectedMode
        {
            get { return Array.IndexOf(_cleanlinessModeArray, true); }
        }

        private bool[] _correctnessModeArray = new bool[] { false, false, false, false, true };
        public bool[] CorrectnessModeArray
        {
            get { return _correctnessModeArray; }
        }
        public int CorrectnessSelectedMode
        {
            get { return Array.IndexOf(_correctnessModeArray, true); }
        }
        public List<string> ImageUrls { get; set; }

        public AccommodationReservation SelectedReservation { get; }
        private readonly Window _accommodationRatingForm;
        private readonly AccommodationRatingService _accommodationRatingService;
        private readonly SetOwnerRoleService _setOwnerRoleService;
        private readonly AccommodationRatingImageService _accommodationRatingImageService;
        #endregion

        public AccommodationRatingFormViewModel(Window accommodationRatingForm, AccommodationReservation reservation)
        {
            _accommodationRatingForm = accommodationRatingForm;
            _accommodationRatingService = new AccommodationRatingService();
            _setOwnerRoleService = new SetOwnerRoleService();
            _accommodationRatingImageService = new AccommodationRatingImageService();
            SelectedReservation = reservation;
            ImageUrls = new List<string>();

            AddImageCommand = new RelayCommand(AddImageCommand_Execute, AddImageCommand_CanExecute);
            RemoveImageCommand = new RelayCommand(RemoveImageCommand_Execute, RemoveImageCommand_CanExecute);
            RateCommand = new RelayCommand(RateCommand_Execute);
            SuggestRenovationCommand = new RelayCommand(SuggestRenovationCommand_Execute);
        }

        #region COMMANDS
        public RelayCommand AddImageCommand { get; }
        public RelayCommand RemoveImageCommand { get; }
        public RelayCommand RateCommand { get; }
        public RelayCommand SuggestRenovationCommand { get; }

        public void AddImageCommand_Execute(object? parameter)
        {
            CurrentImageUrl = ImageUrl;
            ImageUrls.Add(ImageUrl);
        }

        public bool AddImageCommand_CanExecute(object? parameter)
        {
            return CurrentImageUrl != ImageUrl && ImageUrl is not null && ImageUrl != "";
        }

        public void RemoveImageCommand_Execute(object? parameter)
        {
            int index = ImageUrls.IndexOf(ImageUrls.Find(x => x.Equals(CurrentImageUrl)));
            ImageUrls.Remove(ImageUrls.Find(x => x.Equals(CurrentImageUrl)));
            if (index > 0) CurrentImageUrl = ImageUrls[index - 1];
            else CurrentImageUrl = null;
        }

        public bool RemoveImageCommand_CanExecute(object? parameter)
        {
            return ImageUrls.Count != 0;
        }

        public void RateCommand_Execute(object? parameter)
        {
            AccommodationRating accommodationRating = _accommodationRatingService.SaveAccommodationRating(CleanlinessSelectedMode + 1, CorrectnessSelectedMode + 1, Comment, SelectedReservation.Id, SelectedReservation.Accommodation.OwnerId, SelectedReservation.GuestId);
            foreach (var url in ImageUrls)
            {
                _accommodationRatingImageService.SaveImage(url, accommodationRating.Id);
            }

            _setOwnerRoleService.SetOwnerRole(accommodationRating.OwnerId);
            MainWindow.mainWindow.MainPreview.Content = new ReservationsPage(new ReservationsViewModel(SelectedReservation.GuestId));
        }

        public void SuggestRenovationCommand_Execute(object? parameter)
        {
            AccommodationRating accommodationRating = _accommodationRatingService.SaveAccommodationRating(CleanlinessSelectedMode + 1, CorrectnessSelectedMode + 1, Comment, SelectedReservation.Id, SelectedReservation.Accommodation.OwnerId, SelectedReservation.GuestId);
            foreach (var url in ImageUrls)
            {
                _accommodationRatingImageService.SaveImage(url, accommodationRating.Id);
            }

            _setOwnerRoleService.SetOwnerRole(accommodationRating.OwnerId);
            MainWindow.mainWindow.MainPreview.Content = new RenovationSuggestionPage(SelectedReservation);
        }
        #endregion
    }
}
