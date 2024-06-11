using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class AccommodationRegistrationFormViewModel : ViewModelBase
    {
        #region PROPERTIES
        public User LoggedInUser { get; set; }

        private string _accommodationName;
        public string AccommodationName
        {
            get => _accommodationName;
            set
            {

                if (value != _accommodationName)
                {
                    _accommodationName = value;
                    OnPropertyChanged("Name");
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

        private string _capacity;
        public string Capacity
        {
            get => _capacity;
            set
            {
                if (_capacity != value)
                {
                    _capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }

        private string _minDaysForStay;
        public string MinDaysForStay
        {
            get => _minDaysForStay;
            set
            {
                if (_minDaysForStay != value)
                {
                    _minDaysForStay = value;
                    OnPropertyChanged("MinDaysForStay");
                }
            }
        }

        private string _minDaysBeforeCancel;
        public string MinDaysBeforeCancel
        {
            get => _minDaysBeforeCancel;
            set
            {
                if (_minDaysBeforeCancel != value)
                {
                    _minDaysBeforeCancel = value;
                    OnPropertyChanged("MinDaysBeforeCancel");
                }
            }
        }

        private string _superOwnerMark;
        public string SuperOwnerMark
        {
            get => _superOwnerMark;
            set
            {
                if (_superOwnerMark != value)
                {
                    _superOwnerMark = value;
                    OnPropertyChanged("SuperOwnerMark");
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

        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            set
            {
                if (_url != value)
                {
                    _url = value;
                    OnPropertyChanged(nameof(Url));

                    if (_imageNumber == 0)
                    {
                        throw new Exception("Minimum 1 image is required!");
                    }
                }
            }
        }

        private string _pictureSaved;
        public string PictureSaved
        {
            get
            {
                return _pictureSaved;
            }
            set
            {
                if (_pictureSaved != value)
                {
                    _pictureSaved = value;
                    OnPropertyChanged(nameof(PictureSaved));
                }
            }
        }

        public ObservableCollection<String> Images { get; set; }
        public List<String> Countries { get; set; }
        public ObservableCollection<String> Cities { get; set; }
        public ObservableCollection<String> Types { get; set; }

        private readonly Window _accommodationRegistrationForm;

        private readonly AccommodationRepository _accommodationRepository;
        private readonly LocationRepository _locationRepository;
        private readonly AccommodationImageRepository _imageRepository;
        private readonly UserRepository _userRepository;

        private readonly LocationService _locationService;
        private readonly AccommodationYearStatisticsService _accommodationYearStatisticsService;
        private readonly AccommodationMonthStatisticsService _accommodationMonthStatisticsService;

        private int _ownerId;
        private int _imageNumber;

        private Regex _UrlRegex = new Regex("^https?:\\/\\/[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-zA-Z0-9()]{1,6}\\b(?:[-a-zA-Z0-9()@:%_\\+.~#?&\\/=]*)$");
        #endregion

        public AccommodationRegistrationFormViewModel(Window accommodationRegistrationForm, AccommodationRepository accommodationRepository, int ownerId, LocationRepository locationRepository, AccommodationImageRepository imageRepository, UserRepository userRepository)
        {
            _accommodationRegistrationForm = accommodationRegistrationForm;

            MinDaysBeforeCancel = "1";

            Images = new ObservableCollection<String>();
            Countries = new List<String>();
            Cities = new ObservableCollection<String>();
            Types = new ObservableCollection<String>();

            _locationRepository = locationRepository;
            _accommodationRepository = accommodationRepository;
            _imageRepository = imageRepository;
            _userRepository = userRepository;

            _locationService = new LocationService();
            _accommodationYearStatisticsService = new AccommodationYearStatisticsService();
            _accommodationMonthStatisticsService = new AccommodationMonthStatisticsService();

            _ownerId = ownerId;
            _imageNumber = 0;

            Countries.Clear();
            Images.Clear();
            FillInTypes();

            AccommodationRegistrationLoaded();

            AddImageCommand = new RelayCommand(AddImageCommand_Execute, AddImageCommand_CanExecute);
            RemoveImageCommand = new RelayCommand(RemoveImageCommand_Execute, RemoveImageCommand_CanExecute);
            NextImageCommand = new RelayCommand(NextImageCommand_Execute, NextImageCommand_CanExecute);
            PreviousImageCommand = new RelayCommand(PreviousImageCommand_Execute, PreviousImageCommand_CanExecute);
            CancelCommand = new RelayCommand(CancelCommand_Execute);
            RegisterCommand = new RelayCommand(RegisterCommand_Execute, RegisterCommand_CanExecute);
            LoadCitiesCommand = new RelayCommand(LoadCitiesCommand_Execute);
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

        public void FillInCities()
        {
            Cities.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country == Country)
                {
                    Cities.Add(location.City);
                }
                
            }
        }

        public AccommodationType FindType(string type)
        {
            switch (type)
            {
                case "apartment":
                    return AccommodationType.apartment;
                case "house":
                    return AccommodationType.house;
                case "cottage":
                    return AccommodationType.cottage;
                default:
                    return 0;
            }
        }

        private void SetSuperOwnerMark()
        {
            SuperOwnerMark = " ";

            User owner = _userRepository.GetById(_ownerId);

            if (owner.Role == UserRole.SUPER_OWNER)
            {
                SuperOwnerMark = "*";
            }
        }

        private Regex _Number = new Regex(@"^[1-9][0-9]*$");

        public bool IsValid
        {
            get
            {
                if (AccommodationName == null || AccommodationName == "")
                {
                    return false;
                }
                else if (Country == null)
                {
                    return false;
                }
                else if (City == null)
                {
                    return false;
                }
                else if (Type == null)
                {
                    return false;
                }
                else if (Capacity == null || Capacity == "")
                {
                    return false;
                }
                else if (MinDaysForStay == null || MinDaysForStay == "")
                {
                    return false;
                }
                else if (MinDaysBeforeCancel == null || MinDaysBeforeCancel == "")
                {
                    return false;
                }
                else if (_imageNumber == 0)
                {
                    return false;
                }

                Match capacityMatch = _Number.Match(Capacity);
                Match minDaysForStay = _Number.Match(MinDaysForStay);
                Match minDayBeforeCancel = _Number.Match(MinDaysBeforeCancel);

                if (!capacityMatch.Success || !minDaysForStay.Success || !minDayBeforeCancel.Success)
                {
                    return false;
                }

                return true;
            }
        }

        private void SaveMonthStatistics(AccommodationYearStatistic yearStatistic)
        {
            for (int i = 1; i <= 12; i++)
            {
                _accommodationMonthStatisticsService.Save(i, yearStatistic, yearStatistic.Id, 0, 0, 0, 0);
            }
        }

        #region COMMANDS
        public RelayCommand AddImageCommand { get; }
        public RelayCommand RemoveImageCommand { get; }
        public RelayCommand NextImageCommand { get; }
        public RelayCommand PreviousImageCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand LoadCitiesCommand { get; }

        public bool AddImageCommand_CanExecute(object? parameter)
        {
            if (Url != null)
            {
                AccommodationImage image = _imageRepository.GetAll().Find(i => i.Url == Url && i.AccommodationId == -1);

                Match urlMatch = _UrlRegex.Match(Url);

                return Url != "" && image == null && urlMatch.Success;
            }


            return Url != null && Url != "";
        }

        public void AddImageCommand_Execute(object? parameter)
        {
            _imageNumber++;
            CurrentImage = Url;
            PictureSaved = "Image added, if you want to add more images, type another url and click button 'Add images'";
            Images.Add(CurrentImage);
        }

        public bool RemoveImageCommand_CanExecute(object? parameter)
        {
            return CurrentImage != null && CurrentImage != "";
        }

        public void RemoveImageCommand_Execute(object? parameter)
        {
            PictureSaved = "Picture removed, if you want to add more images, type another url and click button 'Add images'";
            for (int i = 0; i < Images.Count; i++)
            {
                if (CurrentImage == Images[i])
                {
                    Images.Remove(CurrentImage);

                    _imageNumber--;

                    if (i != Images.Count)
                    {
                        CurrentImage = Images[i];
                    }
                    else if (Images.Count != 0)
                    {
                        CurrentImage = Images[i - 1];
                    }
                    else if (Images.Count == 0)
                    {
                        CurrentImage = "";
                        return;
                    }
                    return;
                }
            }
        }

        public bool NextImageCommand_CanExecute(object? parameter)
        {
            return CurrentImage != null && CurrentImage != "" && CurrentImage != Images.Last();
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

        public bool PreviousImageCommand_CanExecute(object? parameter)
        {
            return CurrentImage != null && CurrentImage != "" && CurrentImage != Images.First();
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

        public void CancelCommand_Execute(object? parameter)
        {
            _imageRepository.RemovePicturesForCanceledAccommodation();
            _accommodationRegistrationForm.Close();
        }

        public bool RegisterCommand_CanExecute(object? parameter)
        {
            Location location = _locationRepository.GetByCountryAndCity(Country, City);

            return location != null && IsValid;
        }

        public void RegisterCommand_Execute(object? parameter)
        {
            Location location = _locationRepository.GetByCountryAndCity(Country, City);
            AccommodationType accommodationType = FindType(Type);

            SetSuperOwnerMark();
            Accommodation newAccommodation = _accommodationRepository.Save(AccommodationName, location, accommodationType, Capacity, MinDaysForStay, MinDaysBeforeCancel, _ownerId, SuperOwnerMark, _locationRepository);
            int accommodationId = newAccommodation.Id;

            foreach (var url in Images)
            {
                _imageRepository.Save(url, accommodationId);
            }

            MyAccommodationsPageViewModel.MyAccommodations.Add(newAccommodation);
            AccommodationYearStatistic yearStatistic =  _accommodationYearStatisticsService.Save(DateTime.Now.Year, newAccommodation, newAccommodation.Id, 0, 0, 0, 0);
            //_accommodationMonthStatisticsService.Save(DateTime.Now.Month, yearStatistic, yearStatistic.Id, 0, 0, 0, 0);

            SaveMonthStatistics(yearStatistic);


            _accommodationRegistrationForm.Close();
        }

        public void LoadCitiesCommand_Execute(object? parameter)
        {
            Cities.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country != Country) continue;
                Cities.Add(location.City);
            }
        }
        #endregion
    }
}
