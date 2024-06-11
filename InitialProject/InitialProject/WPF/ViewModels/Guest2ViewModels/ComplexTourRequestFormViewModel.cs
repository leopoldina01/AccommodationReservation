using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class ComplexTourRequestFormViewModel : ViewModelBase
    {
        #region PROPERTIES
        
        public User LoggedUser { get; set; }
        private ObservableCollection<string> _countries;
        public ObservableCollection<string> Countries
        {
            get => _countries;
            set
            {
                if (_countries != value)
                {
                    _countries = value;
                    OnPropertyChanged(nameof(Countries));
                }
            }
        }

        private ObservableCollection<string> _cities;
        public ObservableCollection<string> Cities
        {
            get => _cities;
            set
            {
                if (value != _cities)
                {
                    _cities = value;
                    OnPropertyChanged(nameof(Cities));
                }
            }
        }

        private ObservableCollection<string> _guides;
        public ObservableCollection<string> Guides
        {
            get => _guides;
            set
            {
                if (value != _guides)
                {
                    _guides = value;
                    OnPropertyChanged(nameof(Guides));
                }
            }
        }

        private ObservableCollection<string> _languages;
        public ObservableCollection<string> Languages
        {
            get => _languages;
            set
            {
                if (value != _languages)
                {
                    _languages = value;
                    OnPropertyChanged(nameof(Languages));
                }
            }
        }

        private DateTime _requestArrivalDate;
        public DateTime RequestArrivalDate
        {
            get => _requestArrivalDate;
            set
            {
                if (_requestArrivalDate != value)
                {
                    _requestArrivalDate = value;
                    OnPropertyChanged(nameof(RequestArrivalDate));
                }
            }
        }

        private Location _location;
        public Location Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        private int _locationId;
        public int LocationId
        {
            get => _locationId;
            set
            {
                if (_locationId != value)
                {
                    _locationId = value;
                    OnPropertyChanged(nameof(LocationId));
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        private string _country;
        public string SelectedCountry
        {
            get => _country;
            set
            {
                if (_country != value)
                {
                    _country = value;
                    OnPropertyChanged(nameof(SelectedCountry));
                    CountrySelectionChanged();
                }
            }
        }

        private string _city;
        public string SelectedCity
        {
            get => _city;
            set
            {
                if (value != _city)
                {
                    _city = value;
                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }

        private bool _cityComboBoxIsEnabled;
        public bool CityComboBoxIsEnabled
        {
            get => _cityComboBoxIsEnabled;
            set
            {
                if (value != _cityComboBoxIsEnabled)
                {
                    _cityComboBoxIsEnabled = value;
                    OnPropertyChanged(nameof(CityComboBoxIsEnabled));
                }
            }
        }

        private int? _guestsNumber;
        public int? GuestsNumber
        {
            get => _guestsNumber;
            set
            {
                if (_guestsNumber != value)
                {
                    _guestsNumber = value;
                    OnPropertyChanged(nameof(GuestsNumber));
                }
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged(nameof(StartDate));
                }
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged(nameof(EndDate));
                }
            }
        }

        private TourRequestStatus _status;
        public TourRequestStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        private string _selectedGuide;
        public string SelectedGuide
        {
            get => _selectedGuide;
            set
            {
                if (_selectedGuide != value)
                {
                    _selectedGuide = value;
                    OnPropertyChanged(nameof(SelectedGuide));
                }
            }
        }

        private User _guide;
        public User Guide
        {
            get => _guide;
            set
            {
                if (_guide != value)
                {
                    _guide = value;
                    OnPropertyChanged(nameof(Guide));
                }
            }
        }

        private int _guideId;
        public int GuideId
        {
            get => _guideId;
            set
            {
                if (_guideId != value)
                {
                    _guideId = value;
                    OnPropertyChanged(nameof(GuideId));
                }
            }
        }

        /*private bool _requestIsEnabled;
        public bool RequestIsEnabled
        {
            get => _requestIsEnabled;
            set
            {
                if(_requestIsEnabled != value)
                {
                    _requestIsEnabled = value;
                    OnPropertyChanged(nameof(RequestIsEnabled));
                }
            }
        }*/

        private bool _homeIsEnabled;
        public bool HomeIsEnabled
        {
            get => _homeIsEnabled;
            set
            {
                if (_homeIsEnabled != value)
                {
                    _homeIsEnabled = value;
                    OnPropertyChanged(nameof(HomeIsEnabled));
                }
            }
        }

        

        private int counter;
        private ComplexTourRequest _complexTourRequest;
        private List<TourRequest> _tourRequests;
        private TourRequest _firstTourRequest;

        private readonly ComplexTourPartService _complexTourPartService;
        private readonly ComplexTourRequestService _complexTourRequestService;
        private readonly LocationService _locationService;
        private readonly UserService _userService;
        private readonly TourRequestService _tourRequestService;

        #endregion
        public ComplexTourRequestFormViewModel(User user)
        {
            LoggedUser = user;
            _complexTourPartService = new ComplexTourPartService();
            _complexTourRequestService = new ComplexTourRequestService();
            _locationService = new LocationService();
            _userService = new UserService();
            _tourRequestService = new TourRequestService();
            Countries = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            Guides = new ObservableCollection<string>();
            _tourRequests = new List<TourRequest>();

            ShowInitialTourOptions();
            //RequestIsEnabled = false;
            HomeIsEnabled = true;
            counter = 0;

            HomeCommand = new RelayCommand(HomeCommand_Execute);
            RequestTourCommand = new RelayCommand(RequestTourCommand_Execute, RequestTourCommand_CanExecute);
            AddComplexedPartCommand = new RelayCommand(AddComplexedPartCommand_Execute);
            ShowComplexTourRequestsCommand = new RelayCommand(ShowComplexTourRequestsCommand_Execute);

        }

        public void FillTourRequestFields()
        {
            Location = _tourRequestService.FillLocation(SelectedCountry, SelectedCity);
            LocationId = Location.Id;

            Guide = _userService.GetUserByName(SelectedGuide);
            GuideId = Guide.Id;

            RequestArrivalDate = DateTime.Now;

            Status = TourRequestStatus.ON_HOLD;
        }

        public bool IsValidForRequest()
        {
            var isNullOrEmpty = String.IsNullOrEmpty(SelectedCountry) || String.IsNullOrEmpty(SelectedCity) || String.IsNullOrEmpty(SelectedLanguage) || String.IsNullOrEmpty(GuestsNumber.ToString()) || String.IsNullOrEmpty(Description) || String.IsNullOrEmpty(SelectedGuide) ||
                String.IsNullOrEmpty(StartDate.ToString()) || String.IsNullOrEmpty(EndDate.ToString());
            if (isNullOrEmpty)
            {
                MessageBox.Show("All fields have to be filled.");
                return false;
            }
            else if (GuestsNumber <= 0)
            {
                MessageBox.Show("Number of Guests can't be less than 1.");
                return false;
            }
            else if (DateTime.Compare(StartDate, EndDate) >= 0 || DateTime.Compare(DateTime.Now, StartDate) >= 0)
            {
                MessageBox.Show("End date must be AFTER Start Date and Start Date can't be in past.");
                return false;
            }
            return true;
        }

        private void ShowInitialTourOptions()
        {
            CityComboBoxIsEnabled = false;
            FillCountries();
            FillLanguages();
            FillGuides();
        }

        public void FillCountries()
        {
            foreach (var country in _locationService.GetAllCountries())
            {
                Countries.Add(country);
            }
        }

        public void FillCities()
        {
            foreach (var location in _locationService.GetLocations())
            {
                if (SelectedCountry == location.Country)
                {
                    Cities.Add(location.City);
                }
            }
        }

        public void FillGuides()
        {
            foreach (var guide in _userService.GetAllGuidesNames())
            {
                Guides.Add(guide);
            }
        }

        public void FillLanguages()
        {
            Languages = new ObservableCollection<string> { "Serbian", "Hungarian", "German", "Thai", "French", "Italian", "Turkish", "Chinese", "Bulgarian", "Swedish", "Finish", "Croatian", "Bosnian", "Japanese", "Eren Yeager", "Danish", "English", "Romanian", "Greek", "Albanian", "Ukranian", "Russian", "Slovenian", "Slovakian", "Belgian", "Dutch", "Portuguese", "Spanish", "Lithuanian", "Estonian" };
        }

        public void CountrySelectionChanged()
        {
            CityComboBoxIsEnabled = true;
            Cities.Clear();
            FillCities();
            if (SelectedCountry == null)
            {
                CityComboBoxIsEnabled = false;
            }
        }

        public void PrepareForNextAddition()
        {
            counter++;
            resetFields();
            MessageBox.Show("Mini tour has been requested");
        }

        public void SortByDate()
        {
            _tourRequests.Sort((req1, req2) => DateTime.Compare(req1.StartDate, req2.StartDate));
        }

        public bool AreDatesOverlapping(DateTime endDate, DateTime startDate)
        {
            foreach(var req in _tourRequests)
            {
                if(DateTime.Compare(startDate, req.StartDate) > 0 && DateTime.Compare(startDate, req.EndDate) < 0)
                {
                    return true;                     
                }
                else if(DateTime.Compare(endDate, req.StartDate) > 0 && DateTime.Compare(endDate, req.EndDate) < 0)
                {
                    return true;
                }
            }
            return false;
        }

        public void resetFields()
        {
            SelectedCountry = null;
            SelectedCity = null;
            SelectedLanguage = null;
            SelectedGuide = null;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            Description = string.Empty;
            GuestsNumber = 0;
        }

        #region COMMANDS
        public RelayCommand HomeCommand { get; }
        public RelayCommand RequestTourCommand { get; }
        public RelayCommand AddComplexedPartCommand { get; }
        public RelayCommand ShowComplexTourRequestsCommand {  get; }


        public void ShowComplexTourRequestsCommand_Execute(object? parameter)
        {
            ComplexTourRequestView complexTourRequestView = new ComplexTourRequestView(LoggedUser);
        }

        public void AddComplexedPartCommand_Execute(object? parameter)
        {
            if (IsValidForRequest())
            {
                FillTourRequestFields();

                TourRequest tourRequest = new TourRequest(RequestArrivalDate, Location, LocationId, Description, SelectedLanguage, (int)GuestsNumber, StartDate, EndDate, Status, GuideId, Guide, LoggedUser.Id);

                if (!AreDatesOverlapping(StartDate, EndDate))
                {
                    _tourRequests.Add(tourRequest);

                    HomeIsEnabled = false;
                    PrepareForNextAddition();
                }
                else
                {
                    MessageBox.Show("Dates are overlapping with another complex tour part.");
                }
            }
        }

        public void RequestTourCommand_Execute(object? parameter)
        {
            SortByDate();


            List<TourRequest> newTourRequests = new List<TourRequest>();
            foreach(var req in _tourRequests)
            {
                var reqTemp = _tourRequestService.Save(req);
                newTourRequests.Add(reqTemp);
            }

            _firstTourRequest = newTourRequests[0];
            ComplexTourRequest complexTourRequest = new ComplexTourRequest(LoggedUser.Id, TourRequestStatus.ON_HOLD, _firstTourRequest.Id);
            complexTourRequest = _complexTourRequestService.Save(complexTourRequest);

            foreach (var req in newTourRequests)
            {
                ComplexTourPart complexTourPart = new ComplexTourPart(req.Id, req, complexTourRequest.Id, complexTourRequest);
                _complexTourPartService.Save(complexTourPart);
            }

            MessageBox.Show("Your complex tour has been requested.");
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
        }

        public bool RequestTourCommand_CanExecute(object? parameter)
        {
            return _tourRequests.Count >= 2;
        }


        public void HomeCommand_Execute(object? parameter)
        {
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
            //_tourRequestFormView.Close();
        }
        #endregion
    }
}
