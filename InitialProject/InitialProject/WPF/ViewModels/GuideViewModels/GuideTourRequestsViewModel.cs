using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels
{
    public class GuideTourRequestsViewModel : ViewModelBase
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
                    LoadCities();
                    OnPropertyChanged(nameof(SelectedCountry));
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

        private string _selectedLanguage;
        public string SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        private int _minGuests;
        public int MinGuests
        {
            get
            {
                return _minGuests;
            }
            set
            {
                if (_minGuests != value)
                {
                    _minGuests = value;
                    OnPropertyChanged(nameof(MinGuests));
                }
            }
        }

        private int _maxGuests;
        public int MaxGuests
        {
            get
            {
                return _maxGuests;
            }
            set
            {
                if (_maxGuests != value)
                {
                    _maxGuests = value;
                    OnPropertyChanged(nameof(MaxGuests));
                }
            }
        }

        private DateTime _selectedStartDate;
        public DateTime SelectedStartDate
        {
            get
            {
                return _selectedStartDate;
            }
            set
            {
                if (_selectedStartDate != value)
                {
                    _selectedStartDate = value;
                    OnPropertyChanged(nameof(SelectedStartDate));
                }
            }
        }

        private DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get
            {
                return _selectedEndDate;
            }
            set
            {
                if (_selectedEndDate != value)
                {
                    _selectedEndDate = value;
                    OnPropertyChanged(nameof(SelectedEndDate));
                }
            }
        }
        public List<string> Countries { get; set; }
        public ObservableCollection<string> Cities { get; set; }
        public ObservableCollection<string> Languages { get; set; }
        public ObservableCollection<TourRequest> Requests { get; set; }

        private readonly TourRequestService _tourRequestService;
        private readonly LocationService _locationService;
        private readonly ComplexTourPartService _complexTourPartService;

        private readonly User _guide;

        #endregion
        public GuideTourRequestsViewModel(User guide)
        {
            _guide = guide;

            _tourRequestService = new TourRequestService();
            _locationService = new LocationService();
            _complexTourPartService = new ComplexTourPartService();

            Requests = new ObservableCollection<TourRequest>();
            Countries = new List<string>();
            Cities = new ObservableCollection<string>();
            Languages = new ObservableCollection<string>() { "Afrikaans", "Albanian", "Amharic", "Arabic", "Armenian", "Assamese", "Azerbaijani", "Basque", "Belarusian", "Bengali", "Bosnian", "Bulgarian", "Burmese", "Catalan", "Cebuano", "Chichewa", "Chinese (Mandarin)", "Corsican", "Croatian", "Czech", "Danish", "Dutch", "English", "Esperanto", "Estonian", "Finnish", "French", "Frisian", "Galician", "Georgian", "German", "Greek", "Gujarati", "Haitian Creole", "Hausa", "Hawaiian", "Hebrew", "Hindi", "Hmong", "Hungarian", "Icelandic", "Igbo", "Indonesian", "Irish", "Italian", "Japanese", "Javanese", "Kannada", "Kazakh", "Khmer", "Kinyarwanda", "Korean", "Kurdish (Kurmanji)", "Kyrgyz", "Lao", "Latin", "Latvian", "Lithuanian", "Luxembourgish", "Macedonian", "Malagasy", "Malay", "Malayalam", "Maltese", "Maori", "Marathi", "Mongolian", "Myanmar (Burmese)", "Nepali", "Norwegian", "Odia (Oriya)", "Pashto", "Persian", "Polish", "Portuguese", "Punjabi", "Romanian", "Russian", "Samoan", "Scots Gaelic", "Serbian", "Sesotho", "Shona", "Sindhi", "Sinhala", "Slovak", "Slovenian", "Somali", "Spanish", "Sundanese", "Swahili", "Swedish", "Tagalog (Filipino)", "Tajik", "Tamil", "Tatar", "Telugu", "Thai", "Turkish", "Turkmen", "Ukrainian", "Urdu", "Uyghur", "Uzbek", "Vietnamese", "Welsh", "Xhosa", "Yiddish", "Yoruba", "Zulu" };

            SelectedStartDate = DateTime.MinValue;
            SelectedEndDate = DateTime.MaxValue;
            MinGuests = 0;
            MaxGuests = 10000;

            LoadCountries();
            LoadRequests();

            SearchCommand = new RelayCommand(SearchCommand_Execute);
            ClearCommand = new RelayCommand(ClearCommand_Execute);
            AcceptCommand = new RelayCommand(AcceptCommand_Execute);
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

        private void LoadRequests()
        {
            Requests.Clear();
            foreach (var request in _tourRequestService.GetAllByGuide(_guide))
            {
                if (_complexTourPartService.IsComplexTourPart(request)) continue;
                Requests.Add(request);
            }
        }

        private void FilterRequests(string country, string city, string language, DateTime startDate, DateTime endDate, int minGuests, int maxGuests)
        {
            Requests.Clear();
            foreach (var request in _tourRequestService.GetAllByGuide(_guide))
            {
                if (_complexTourPartService.IsComplexTourPart(request)) continue;
                if (request.Location.Country != country && country is not null) continue;
                if (request.Location.City != city && city is not null) continue;
                if (request.Language != language && language is not null) continue;
                if (request.StartDate.Subtract(startDate) < TimeSpan.Zero) continue;
                if (request.EndDate.Subtract(endDate) > TimeSpan.Zero) continue;
                if (request.GuestsNumber < minGuests || request.GuestsNumber > maxGuests) continue;

                Requests.Add(request);
            }
        }

        private void ResetSearch()
        {
            SelectedCountry = null;
            SelectedCity = null;
            SelectedLanguage = null;
            SelectedStartDate = DateTime.MinValue;
            SelectedEndDate = DateTime.MaxValue;
            MinGuests = 0;
            MaxGuests = 10000;
        }

        #region COMMANDS
        public RelayCommand SearchCommand { get; }
        public RelayCommand ClearCommand { get; }
        public RelayCommand AcceptCommand { get; }

        public void SearchCommand_Execute(object? parameter)
        {
            FilterRequests(SelectedCountry, SelectedCity, SelectedLanguage, SelectedStartDate, SelectedEndDate, MinGuests, MaxGuests);
        }

        public void ClearCommand_Execute(object? parameter)
        {
            ResetSearch();
            LoadRequests();
        }

        public void AcceptCommand_Execute(object? parameter)
        {
            var request = parameter as TourRequest;
            var acceptTourRequestView = new AcceptTourRequestView(request, _guide);
            acceptTourRequestView.Show();
        }
        #endregion
    }
}
