using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class MyAccommodationsPageViewModel : ViewModelBase
    {
        #region PROPERTIES
        private readonly Page _myAccommodationsPage;

        private readonly AccommodationRepository _accommodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly AccommodationImageRepository _accommodationImageRepository;
        private readonly UserRepository _userRepository;

        private readonly AccommodationService _accommodationService;
        private readonly AccommodationImageService _accommodationImageService;
        private readonly MostPopularLocationService _mostPopularLocationService;

        private readonly User _user;

        private Accommodation _selectedAccommodation;
        public Accommodation SelectedAccommodation
        {
            get
            {
                return _selectedAccommodation;
            }
            set
            {
                if (_selectedAccommodation != value)
                {
                    _selectedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                    UploadImages();
                }
            }
        }

        private string _currentImage;
        public string CurrentImage
        {
            get
            {
                return _currentImage;
            }
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));
                }
            }
        }

        private ObservableCollection<String> _images;

        public ObservableCollection<String> Images 
        {
            get
            {
                return _images;
            }
            set
            {
                if (_images != value)
                {
                    _images = value;
                    OnPropertyChanged(nameof(Images));
                }
            }
        }

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {

                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged("AccommodationName");
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string _country;
        public string Country
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        private string _type;

        public string Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private string _mostPopularLocation;

        public string MostPopularLocation
        {
            get => _mostPopularLocation;
            set
            {
                if (value != _mostPopularLocation)
                {
                    _mostPopularLocation = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private string _leastPopularLocation;

        public string LeastPopularLocation
        {
            get => _leastPopularLocation;
            set
            {
                if (value != _leastPopularLocation)
                {
                    _leastPopularLocation = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        public static ObservableCollection<Accommodation> MyAccommodations { get; set; }

        public List<String> Countries { get; set; }
        public ObservableCollection<String> Cities { get; set; }
        public ObservableCollection<String> Types { get; set; }

        private readonly LocationService _locationService;

        #endregion


        public MyAccommodationsPageViewModel(Page myAccommodationsPage, User user)
        {
            _accommodationRepository = new AccommodationRepository();
            _locationRepository = new LocationRepository();
            _userRepository = new UserRepository();
            _accommodationImageRepository = new AccommodationImageRepository();

            _locationService = new LocationService();

            _accommodationService = new AccommodationService();
            _accommodationImageService = new AccommodationImageService();
            _mostPopularLocationService = new MostPopularLocationService();

            _myAccommodationsPage = myAccommodationsPage;

            _user = user;

            Countries = new List<String>();
            Cities = new ObservableCollection<String>();
            Types = new ObservableCollection<String>();
            Images = new ObservableCollection<String>();
            MyAccommodations = new ObservableCollection<Accommodation>();

            LoadData();

            CreateNewAccommodationCommand = new RelayCommand(CreateNewAccommodationCommand_Execute);
            NextImageCommand = new RelayCommand(NextImageCommand_Execute, NextImageCommand_CanExecute);
            PreviousImageCommand = new RelayCommand(PreviousImageCommand_Execute, PreviousImageCommand_CanExecute);
            OpenAccommodationInfoCommand = new RelayCommand(OpenAccommodationInfoCommand_Execute, OpenAccommodationInfoCommand_CanExecute);
            FilterAccommodationsCommand = new RelayCommand(FilterAccommodationsCommand_Execute);
            LoadCitiesCommand = new RelayCommand(LoadCitiesCommand_Execute);
            RenovateAccommodationCommand = new RelayCommand(RenovateAccommodationCommand_Execute);
            AccommodationStatisticsCommand = new RelayCommand(AccommodationStatisticsCommand_Execute);

            UploadImages();
        }

        private void LoadData()
        {
            LoadMostPopularLocation();
            LoadLeastPopularLocation();
            AccommodationRegistrationLoaded();
            LoadAccommodations();
            FillInTypes();
        }

        private void LoadLeastPopularLocation()
        {
            Location leastPopularLocation = _mostPopularLocationService.FindLeastPopular(_user.Id);

            LeastPopularLocation = leastPopularLocation.City + ", " + leastPopularLocation.Country;
        }

        private void LoadMostPopularLocation()
        {
            Location mostPopularLocation = _mostPopularLocationService.FindMostPopular();

            MostPopularLocation = mostPopularLocation.City + ", " + mostPopularLocation.Country;
        }

        private void FilterAccommodations()
        {
            MyAccommodations.Clear();

            foreach (var accommodation in _accommodationService.GetByOwnerId(_user.Id))
            {
                if ((AccommodationName == null || accommodation.Name.ToUpper().Contains(AccommodationName.ToUpper())) &&
                    (Type == null || accommodation.Type.ToString().ToUpper().Contains(Type.ToUpper())) &&
                    (City == null || accommodation.Location.City.ToString().ToUpper().Contains(City.ToUpper())) &&
                    (Country == null || accommodation.Location.Country.ToString().ToUpper().Contains(Country.ToUpper())))
                {
                    MyAccommodations.Add(accommodation);
                }
            }
        }

        public void FillInTypes()
        {
            Types.Clear();
            Types.Add("apartment");
            Types.Add("house");
            Types.Add("cottage");
        }

        private void AccommodationRegistrationLoaded()
        {
            Countries = _locationService.GetAllCountries().ToList();
        }

        public void LoadAccommodations()
        {
            MyAccommodations.Clear();

            foreach (var accommodation in _accommodationService.GetByOwnerId(_user.Id))
            {
                MyAccommodations.Add(accommodation);
            }
        }

        public void UploadImages()
        {
            Images.Clear();

            if (SelectedAccommodation != null)
            {
                foreach (var image in _accommodationImageService.GetAllByAccommodationId(SelectedAccommodation.Id))
                {
                    Images.Add(image.Url);
                }
                
            }

            if (Images.Count > 0)
            {
                CurrentImage = Images[0];
            }
        }

        #region COMMANDS
        public RelayCommand? CreateNewAccommodationCommand { get; }
        public RelayCommand NextImageCommand { get; }
        public RelayCommand PreviousImageCommand { get; }
        public RelayCommand? OpenAccommodationInfoCommand { get; }
        public RelayCommand FilterAccommodationsCommand { get; }
        public RelayCommand LoadCitiesCommand { get; }
        public RelayCommand RenovateAccommodationCommand { get; }
        public RelayCommand AccommodationStatisticsCommand { get; }

        public void CreateNewAccommodationCommand_Execute(object? parameter)
        {
            AccommodationRegistrationForm accommodationRegistration = new AccommodationRegistrationForm(_accommodationRepository, _locationRepository, _accommodationImageRepository, _user.Id, _userRepository, MyAccommodations);
            accommodationRegistration.Show();
        }

        public void PreviousImageCommand_Execute(object? prameter)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                if (CurrentImage == Images[i])
                {
                    CurrentImage = Images[i - 1];
                    return;
                }
            }
        }

        public bool PreviousImageCommand_CanExecute(object? parameter)
        {
            return CurrentImage != null && CurrentImage != Images.First();
        }

        public void NextImageCommand_Execute(object? parameter)
        {
            for (int i = 0; i < Images.Count; i++)
            {
                if (CurrentImage == Images[i])
                {
                    CurrentImage = Images[i + 1];
                    return;
                }
            }
        }

        public bool NextImageCommand_CanExecute(object? parameter)
        {
            return CurrentImage != null && CurrentImage != Images.Last();
        }

        public bool OpenAccommodationInfoCommand_CanExecute(object? parameter)
        {
            return SelectedAccommodation != null;
        }

        public void OpenAccommodationInfoCommand_Execute(object? parameter)
        {
            AccommodationInfoOverview accommodationInfoOverview = new AccommodationInfoOverview(SelectedAccommodation);
            accommodationInfoOverview.Show();
        }

        public void FilterAccommodationsCommand_Execute(object? parameter)
        {
            FilterAccommodations();
        }

        public void LoadCitiesCommand_Execute(object? parameter)
        {
            Cities.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country != Country) continue;
                Cities.Add(location.City);
            }

            MyAccommodations.Clear();

            FilterAccommodations();
            return;
        }

        public void RenovateAccommodationCommand_Execute(object? parameter)
        {
            RenovateAccommodationForm renovateAccommodationForm = new RenovateAccommodationForm(SelectedAccommodation);
            renovateAccommodationForm.Show();
        }

        public void AccommodationStatisticsCommand_Execute(object? parameter)
        {
            AccommodationStatisticsOverviewWindow accommodationStatisticsOverviewWindow = new AccommodationStatisticsOverviewWindow(SelectedAccommodation);
            accommodationStatisticsOverviewWindow.Show();
        }
        #endregion
    }
}
