using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest2Views;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using InitialProject.Application.UseCases;
using System.Windows.Threading;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class TourRequestFormViewModel : ViewModelBase
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

        private string _demoButtonContent;
        public string DemoButtonContent
        {
            get
            {
                return _demoButtonContent;
            }
            set
            {
                if (_demoButtonContent != value)
                {
                    _demoButtonContent = value;
                    OnPropertyChanged(nameof(DemoButtonContent));
                }
            }
        }

        public DispatcherTimer Timer { get; set; }

        private readonly LocationService _locationService;
        private readonly UserService _userService;
        private readonly TourRequestService _tourRequestService;

        #endregion
        public TourRequestFormViewModel(User user)
        {
            LoggedUser = user;

            _locationService = new LocationService();
            _userService = new UserService();
            _tourRequestService = new TourRequestService();
            Countries = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            Guides = new ObservableCollection<string>();

            ShowInitialTourOptions();

            DemoButtonContent = "Demo";

            HomeCommand = new RelayCommand(HomeCommand_Execute);
            RequestTourCommand = new RelayCommand(RequestTourCommand_Execute);
            ShowTourRequestsCommand = new RelayCommand(ShowTourRequestsCommand_Execute);
            DemoCommand = new RelayCommand(DemoCommand_Execute);

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

        private void RunDemo()
        {
            var demoSelectedCountry = "Japan";
            var demoSelectedCity = "Tokyo";
            var demoSelectedLanguage = "Serbian";
            var demoSelectedGuide = "Mika";
            var demoDescription = "Jako lep opis";
            var demoStartDate = DateTime.Parse("1.1.2023.");
            var demoEndDate = DateTime.Parse("15.1.2023.");
            var demoGuestsNumber = 15;

            Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(0.2)
            };

            this.ResetFields();
            int i = 0;

            Timer.Tick += delegate (object? sender, EventArgs e)
            {
                if (i < 5)
                {
                    SelectedCountry += demoSelectedCountry[i];
                }
                else if (i < 10)
                {
                    SelectedCity += demoSelectedCity[i - 5];
                }
                else if (i < 17)
                {
                    SelectedLanguage += demoSelectedLanguage[i - 10];
                }
                else if (i < 30)
                {
                    Description += demoDescription[i - 17];
                }
                else if (i < 45)
                {
                    SelectedGuide = demoSelectedGuide;
                }
                else if (i < 55)
                {
                    StartDate = demoStartDate;
                }
                else if (i < 65)
                {
                    EndDate = demoEndDate;
                }
                else if (i < 75)
                {
                    GuestsNumber = demoGuestsNumber;
                }
                else if (i < 85)
                {
                    this.RequestTourCommand_Execute(null);
                    StopDemo();
                }
                else
                {
                    Timer.Stop();
                }
                i++;
            };
            Timer.Start();
            DemoButtonContent = "Stop";
        }

        public void StopDemo()
        {
            DemoButtonContent = "Demo";
            Timer.Stop();
            ResetFields();
        }

        private void ResetFields()
        {
            SelectedCountry = "";
            SelectedCity = "";
            SelectedLanguage = "";
            SelectedGuide = "";
            Description = "";
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            GuestsNumber = 0;
        }

        #region COMMANDS

        public RelayCommand HomeCommand { get; }
        public RelayCommand RequestTourCommand { get; }
        public RelayCommand ShowTourRequestsCommand { get; }
        public RelayCommand DemoCommand { get; }

        public void DemoCommand_Execute(object? parameter)
        {
            if (DemoButtonContent.Equals("Demo"))
            {
                RunDemo();
            }
            else
            {
                StopDemo();
            }
        }

        public void ShowTourRequestsCommand_Execute(object? parameter)
        {
            TourRequestView tourRequestView = new TourRequestView(LoggedUser);
            //tourRequestView.Show();
            //_tourRequestFormView.Close();
        }

        public void RequestTourCommand_Execute(object? parameter)
        {
            if (IsValidForRequest())
            {
                FillTourRequestFields();
                TourRequest tourRequest = new TourRequest(RequestArrivalDate, Location, LocationId, Description, SelectedLanguage, (int)GuestsNumber, StartDate, EndDate, Status, GuideId, Guide, LoggedUser.Id); ;
                _tourRequestService.Save(tourRequest);

                Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
                //guest2TourView.Show();
                //_tourRequestFormView.Close();
            }
        }


        public void HomeCommand_Execute(object? parameter)
        {
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
            //_tourRequestFormView.Close();
        }

        #endregion
    }
}
