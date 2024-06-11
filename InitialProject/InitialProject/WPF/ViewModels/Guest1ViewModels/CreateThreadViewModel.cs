using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class CreateThreadViewModel : ViewModelBase
    {
        #region PROPERTIES
        private string _selectedCountry;
        public string SelectedCountry
        {
            get
            {
                return _selectedCountry;
            }
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    OnPropertyChanged(nameof(SelectedCountry));
                    LoadCities();
                }
            }
        }

        private string _selectedCity;
        public string SelectedCity
        {
            get
            {
                return _selectedCity;
            }
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }

        private string _comment;
        public string Comment
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
        public List<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public User LoggedUser { get; set; }

        private readonly ForumService _forumService;
        private readonly LocationService _locationService;
        private readonly CommentService _commentService;
        private readonly AccommodationNotificationService _accommodationNotificationService;
        #endregion
        public CreateThreadViewModel(User user)
        {
            _forumService = new ForumService();
            _locationService = new LocationService();
            _commentService = new CommentService();
            _accommodationNotificationService = new AccommodationNotificationService();

            LoggedUser = user;

            Countries = new List<string>();
            Cities = new ObservableCollection<string>();

            CreateThreadCommand = new RelayCommand(CreateThreadCommand_Execute, CreateThreadCommand_CanExecute);

            LoadCountries();
        }

        private void LoadCountries()
        {
            Countries = _locationService.GetAllCountries().ToList();
            Countries.Sort();
        }

        private void LoadCities()
        {
            Cities.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country != SelectedCountry) continue;
                Cities.Add(location.City);
            }
        }

        #region COMMANDS
        public RelayCommand CreateThreadCommand { get; }

        public void CreateThreadCommand_Execute(object? parameter)
        {
            _forumService.Save("Open", _locationService.GetByCountryAndCity(SelectedCountry, SelectedCity).Id, LoggedUser.Id, Comment);
            _accommodationNotificationService.NotifyAllOwners(_locationService.GetByCountryAndCity(SelectedCountry, SelectedCity).Id, LoggedUser.Id, LoggedUser.Username, _locationService.GetByCountryAndCity(SelectedCountry, SelectedCity));
            MainWindow.mainWindow.MainPreview.Content = new ForumPage(LoggedUser);
        }

        public bool CreateThreadCommand_CanExecute(object? parameter)
        {
            return !string.IsNullOrEmpty(SelectedCountry) && !string.IsNullOrEmpty(SelectedCity) && !string.IsNullOrEmpty(Comment);
        }
        #endregion
    }
}
