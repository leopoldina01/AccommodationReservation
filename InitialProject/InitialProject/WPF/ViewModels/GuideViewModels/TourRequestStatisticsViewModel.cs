using InitialProject.Application.UseCases;
using InitialProject.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels
{
    public class TourRequestStatisticsViewModel : ViewModelBase
    {
		#region PROPERTIES
		private string _selectedCountryYearly;
		public string SelectedCountryYearly
		{
			get
			{
				return _selectedCountryYearly;
			}
			set
			{
				if (_selectedCountryYearly != value)
				{
					_selectedCountryYearly = value;
					LoadCitiesYearly();
					OnPropertyChanged(nameof(SelectedCountryYearly));
				}
			}
		}

		private string _selectedCityYearly;
		public string SelectedCityYearly
		{
			get
			{
				return _selectedCityYearly;
			}
			set
			{
				if (_selectedCityYearly != value)
				{
					_selectedCityYearly = value;
					LoadYearlyStatisticsFiltered();
                    OnPropertyChanged(nameof(SelectedCityYearly));
				}
			}
		}

		private string _selectedLanguageYearly;
		public string SelectedLanguageYearly
		{
			get
			{
				return _selectedLanguageYearly;
			}
			set
			{
				if (_selectedLanguageYearly != value)
				{
					_selectedLanguageYearly = value;
                    LoadYearlyStatisticsFiltered();
                    OnPropertyChanged(nameof(SelectedLanguageYearly));
				}
			}
		}

		private string _selectedCountryMonthly;
		public string SelectedCountryMonthly
		{
			get
			{
				return _selectedCountryMonthly;
			}
			set
			{
				if (_selectedCountryMonthly != value)
				{
					_selectedCountryMonthly = value;
					LoadCitiesMonthly();
                    OnPropertyChanged(nameof(SelectedCountryMonthly));
				}
			}
		}

		private string _selectedCityMonthly;
		public string SelectedCityMonthly
		{
			get
			{
				return _selectedCityMonthly;
			}
			set
			{
				if (_selectedCityMonthly != value)
				{
					_selectedCityMonthly = value;
					LoadMonthlyStatisticsFiltered();
                    OnPropertyChanged(nameof(SelectedCityMonthly));
				}
			}
		}

		private string _selectedLanguageMonthly;
		public string SelectedLanguageMonthly
		{
			get
			{
				return _selectedLanguageMonthly;
			}
			set
			{
				if (_selectedLanguageMonthly != value)
				{
					_selectedLanguageMonthly = value;
					LoadMonthlyStatisticsFiltered();
                    OnPropertyChanged(nameof(SelectedLanguageMonthly));
				}
			}
		}

		private int _selectedYear;
		public int SelectedYear
		{
			get
			{
				return _selectedYear;
			}
			set
			{
				if (_selectedYear != value)
				{
					_selectedYear = value;
					LoadMonthlyStatisticsFiltered();
                    OnPropertyChanged(nameof(SelectedYear));
				}
			}
		}

		public List<string> Countries { get; set; }
		public ObservableCollection<string> CitiesYearly { get; set; }
		public List<string> Languages { get; set; }
		public List<int> Years { get; set; }

		private readonly LocationService _locationService;
		private readonly TourRequestStatisticsService _tourRequestStatisticsService;

		public ObservableCollection<TourRequestYearlyStatistics> YearlyStatistics { get; set; }
		public ObservableCollection<TourRequestMonthlyStatistics> MonthlyStatistics { get; set; }
		#endregion
		public TourRequestStatisticsViewModel()
        {
			_locationService = new LocationService();
			_tourRequestStatisticsService = new TourRequestStatisticsService();
			YearlyStatistics = new ObservableCollection<TourRequestYearlyStatistics>();
			MonthlyStatistics = new ObservableCollection<TourRequestMonthlyStatistics>();

			Countries = new List<string>();
			CitiesYearly = new ObservableCollection<string>();
            Languages = new List<string>() { "Afrikaans", "Albanian", "Amharic", "Arabic", "Armenian", "Assamese", "Azerbaijani", "Basque", "Belarusian", "Bengali", "Bosnian", "Bulgarian", "Burmese", "Catalan", "Cebuano", "Chichewa", "Chinese (Mandarin)", "Corsican", "Croatian", "Czech", "Danish", "Dutch", "English", "Esperanto", "Estonian", "Finnish", "French", "Frisian", "Galician", "Georgian", "German", "Greek", "Gujarati", "Haitian Creole", "Hausa", "Hawaiian", "Hebrew", "Hindi", "Hmong", "Hungarian", "Icelandic", "Igbo", "Indonesian", "Irish", "Italian", "Japanese", "Javanese", "Kannada", "Kazakh", "Khmer", "Kinyarwanda", "Korean", "Kurdish (Kurmanji)", "Kyrgyz", "Lao", "Latin", "Latvian", "Lithuanian", "Luxembourgish", "Macedonian", "Malagasy", "Malay", "Malayalam", "Maltese", "Maori", "Marathi", "Mongolian", "Myanmar (Burmese)", "Nepali", "Norwegian", "Odia (Oriya)", "Pashto", "Persian", "Polish", "Portuguese", "Punjabi", "Romanian", "Russian", "Samoan", "Scots Gaelic", "Serbian", "Sesotho", "Shona", "Sindhi", "Sinhala", "Slovak", "Slovenian", "Somali", "Spanish", "Sundanese", "Swahili", "Swedish", "Tagalog (Filipino)", "Tajik", "Tamil", "Tatar", "Telugu", "Thai", "Turkish", "Turkmen", "Ukrainian", "Urdu", "Uyghur", "Uzbek", "Vietnamese", "Welsh", "Xhosa", "Yiddish", "Yoruba", "Zulu" };
            Years = new List<int>(_tourRequestStatisticsService.GetAllYearsWithRequests());
			Years.Sort();
			SelectedYear = Years.First();

			LoadCountries();
			LoadYearlyStatistics();
			LoadMonthlyStatistics();
        }
        private void LoadCountries()
		{
            Countries = _locationService.GetAllCountries().ToList();
            Countries.Sort();
        }

        private void LoadCitiesYearly()
        {
            CitiesYearly.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country != SelectedCountryYearly) continue;
                CitiesYearly.Add(location.City);
            }
        }

		private void LoadCitiesMonthly()
		{
            CitiesYearly.Clear();
            foreach (var location in _locationService.GetAll())
            {
                if (location.Country != SelectedCountryMonthly) continue;
                CitiesYearly.Add(location.City);
            }
        }

		private void LoadYearlyStatistics()
		{
			foreach (var year in _tourRequestStatisticsService.GetAllYearsWithRequests())
			{
				var requestNumber = _tourRequestStatisticsService.GetNumberOfRequestsForYear(year);
				var tourRequestYearlyStatistics = new TourRequestYearlyStatistics(year, requestNumber);
				YearlyStatistics.Add(tourRequestYearlyStatistics);
			}
		}

		private void LoadMonthlyStatistics()
		{
			MonthlyStatistics.Clear();
            for (int i = 1; i < 13; i++)
            {
                var requestNumber = _tourRequestStatisticsService.GetNumberOfRequestsForMonth(i, SelectedYear);
                var tourRequestMonthlyStatistics = new TourRequestMonthlyStatistics(DateTimeFormatInfo.CurrentInfo.GetMonthName(i), requestNumber);
                MonthlyStatistics.Add(tourRequestMonthlyStatistics);
            }
        }

		private void LoadYearlyStatisticsFiltered()
		{
			YearlyStatistics.Clear();
            foreach (var year in _tourRequestStatisticsService.FilterYearsWithRequests(SelectedCountryYearly, SelectedCityYearly, SelectedLanguageYearly))
            {
                var requestNumber = _tourRequestStatisticsService.FilterNumberOfRequestsForYear(year, SelectedCountryYearly, SelectedCityYearly, SelectedLanguageYearly);
                var tourRequestYearlyStatistics = new TourRequestYearlyStatistics(year, requestNumber);
                YearlyStatistics.Add(tourRequestYearlyStatistics);
            }
        }

		private void LoadMonthlyStatisticsFiltered()
		{
			MonthlyStatistics.Clear();
            for (int i = 1; i < 13; i++)
            {
                var requestNumber = _tourRequestStatisticsService.FilterNumberOfRequestsForMonth(i, SelectedYear, SelectedCountryMonthly, SelectedCityMonthly, SelectedLanguageMonthly);
                var tourRequestMonthlyStatistics = new TourRequestMonthlyStatistics(DateTimeFormatInfo.CurrentInfo.GetMonthName(i), requestNumber);
                MonthlyStatistics.Add(tourRequestMonthlyStatistics);
            }
        }
    }
}
