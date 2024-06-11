using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for AccommodationsPage.xaml
    /// </summary>
    public partial class AccommodationsPage : Page, INotifyPropertyChanged
    {
        public User LoggedUser { get; set; }
        public readonly AccommodationRepository _accommodationRepository;
        public readonly LocationRepository _locationRepository;
        public readonly AccommodationImageRepository _accommodationImageRepository;
        public readonly AccommodationReservationRepository _accommodationReservationRepository;
        public readonly UserRepository _userRepository;

        private ObservableCollection<Accommodation> _accommodations;
        public ObservableCollection<Accommodation> Accommodations
        {
            get => _accommodations;
            set
            {
                if (_accommodations != value)
                {
                    _accommodations = value;
                    OnPropertyChanged("Accommodations");
                }
            }
        }

        private ObservableCollection<string> _countries;
        public ObservableCollection<string> Countries
        {
            get => _countries;
            set
            {
                if (_countries != value)
                {
                    _countries = value;
                    OnPropertyChanged("Countries");
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
                    OnPropertyChanged("Cities");
                }
            }
        }

        private Accommodation _accommodation;
        public Accommodation SelectedAccommodation
        {
            get => _accommodation;
            set
            {
                if (_accommodation != value)
                {
                    _accommodation = value;
                    OnPropertyChanged("SelectedAccommodation");
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
                    OnPropertyChanged("SelectedCountry");
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
                    OnPropertyChanged("SelectedCity");
                }
            }
        }

        private string _guestNumber;
        public string GuestNumber
        {
            get => _guestNumber;
            set
            {
                if (_guestNumber != value)
                {
                    _guestNumber = value;
                    OnPropertyChanged("GuestNumber");
                }
            }
        }

        private string _lenghtOfStay;
        public string LenghtOfStay
        {
            get => _lenghtOfStay;
            set
            {
                if (_lenghtOfStay != value)
                {
                    _lenghtOfStay = value;
                    OnPropertyChanged("LenghtOfStay");
                }
            }
        }

        public string AccommodationName { get; set; }
        public bool IsApartment { get; set; }
        public bool IsHouse { get; set; }
        public bool IsCottage { get; set; }
        public bool BookButton { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AccommodationsPage(User user, AccommodationRepository accommodationRepository, LocationRepository locationRepository, AccommodationImageRepository accommodationImageRepository, AccommodationReservationRepository accommodationReservationRepository, UserRepository userRepository)
        {
            InitializeComponent();
            DataContext = this;
            BookButton = false;

            _accommodationRepository = accommodationRepository;
            _locationRepository = locationRepository;
            _accommodationImageRepository = accommodationImageRepository;
            _accommodationReservationRepository = accommodationReservationRepository;
            _userRepository = userRepository;

            LoggedUser = user;
            Countries = new ObservableCollection<string>();
            Cities = new ObservableCollection<string>();
            ShowInitialOptions();
        }

        private void ShowInitialOptions()
        {
            foreach (var location in _locationRepository.GetAll())
            {
                if (!Countries.Contains(location.Country))
                {
                    Countries.Add(location.Country);
                }
            }

            Accommodations = new ObservableCollection<Accommodation>();
            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                InsertLocation(accommodation);
                Accommodations.Add(accommodation);
            }

            SuperOwnerAccommodationPriority();
        }

        private void ComboBoxCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxCity.IsEnabled = true;
            Cities.Clear();

            foreach (var location in _locationRepository.GetAll())
            {
                if (SelectedCountry == location.Country)
                {
                    Cities.Add(location.City);
                }
            }

            if (SelectedCountry == null)
            {
                ComboBoxCity.IsEnabled = false;
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation == null) BookButton = false;
            if (!IsInputValid()) return;

            Accommodations.Clear();
            foreach (var accommodation in _accommodationRepository.GetAll())
            {
                InsertLocation(accommodation);
                Accommodations.Add(accommodation);
                RemoveByName(accommodation);
                RemoveByGuestNumber(accommodation);
                RemoveByLenghtOfStay(accommodation);
                RemoveByCountry(accommodation);
                RemoveByCity(accommodation);
                RemoveByType(accommodation);
            }

            SuperOwnerAccommodationPriority();
        }

        private void SuperOwnerAccommodationPriority()
        {
            List<Accommodation> ordinaryAccommodations = new List<Accommodation>();
            List<Accommodation> superOwnerAccommodations = new List<Accommodation>();

            foreach (var accommodation in Accommodations)
            {
                User owner = _userRepository.GetById(accommodation.OwnerId);

                if (owner.Role == UserRole.SUPER_OWNER)
                {
                    superOwnerAccommodations.Add(accommodation);
                    continue;
                }

                ordinaryAccommodations.Add(accommodation);
            }

            JoinSuperOwnerAndOrdinaryAccommodations(ordinaryAccommodations, superOwnerAccommodations);
        }

        private void JoinSuperOwnerAndOrdinaryAccommodations(List<Accommodation> ordinaryAccommodations, List<Accommodation> superOwnerAccommodations)
        {
            Accommodations.Clear();

            foreach (var accommodation in superOwnerAccommodations)
            {
                Accommodations.Add(accommodation);
            }

            foreach (var accommodation in ordinaryAccommodations)
            {
                Accommodations.Add(accommodation);
            }
        }

        private void InsertLocation(Accommodation accommodation)
        {
            foreach (var location in _locationRepository.GetAll())
            {
                if (location.Id == accommodation.LocationId)
                {
                    accommodation.Location = _locationRepository.GetById(location.Id);
                }
            }
        }

        private void RemoveByName(Accommodation newAccommodation)
        {
            if (AccommodationName != null && AccommodationName != "")
            {
                if (!newAccommodation.Name.ToUpper().Contains(AccommodationName.ToUpper()))
                {
                    Accommodations.Remove(newAccommodation);
                }
            }
        }

        private Regex _NaturalNumberRegex = new Regex("^[1-9][0-9]*$");
        private void RemoveByGuestNumber(Accommodation newAccommodation)
        {
            if (GuestNumber != null && GuestNumber != "")
            {
                if (_NaturalNumberRegex.Match(GuestNumber).Success)
                {
                    if (Convert.ToInt32(GuestNumber) > newAccommodation.Capacity)
                    {
                        Accommodations.Remove(newAccommodation);
                    }
                }
            }
        }

        private void RemoveByLenghtOfStay(Accommodation newAccommodation)
        {
            if (LenghtOfStay != null && LenghtOfStay != "")
            {
                if (_NaturalNumberRegex.Match(LenghtOfStay).Success)
                {
                    if (Convert.ToInt32(LenghtOfStay) < newAccommodation.MinDaysForStay)
                    {
                        Accommodations.Remove(newAccommodation);
                    }
                }
            }
        }

        private void RemoveByCountry(Accommodation newAccommodation)
        {
            if (SelectedCountry != null && SelectedCountry != "")
            {
                if (SelectedCountry != newAccommodation.Location.Country)
                {
                    Accommodations.Remove(newAccommodation);
                }
            }
        }

        private void RemoveByCity(Accommodation newAccommodation)
        {
            if (SelectedCity != null && SelectedCity != "")
            {
                if (SelectedCity != newAccommodation.Location.City)
                {
                    Accommodations.Remove(newAccommodation);
                }
            }
        }

        private void RemoveByType(Accommodation newAccommodation)
        {
            if (!IsApartment && !IsHouse && !IsCottage)
            {
                return;
            }

            if (!IsApartment)
            {
                if (newAccommodation.Type == AccommodationType.apartment)
                {
                    Accommodations.Remove(newAccommodation);
                }
            }
            if (!IsHouse)
            {
                if (newAccommodation.Type == AccommodationType.house)
                {
                    Accommodations.Remove(newAccommodation);
                }
            }
            if (!IsCottage)
            {
                if (newAccommodation.Type == AccommodationType.cottage)
                {
                    Accommodations.Remove(newAccommodation);
                }
            }
        }

        private bool IsInputValid()
        {
            if (LenghtOfStay != null && LenghtOfStay != "")
            {
                if (!_NaturalNumberRegex.Match(LenghtOfStay).Success)
                {
                    MessageBox.Show("Please enter a valid value.");
                    return false;
                }
            }

            if (GuestNumber != null && GuestNumber != "")
            {
                if (!_NaturalNumberRegex.Match(GuestNumber).Success)
                {
                    MessageBox.Show("Please enter a valid value.");
                    return false;
                }
            }

            return true;
        }

        private void BookButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                MainWindow.mainWindow.MainPreview.Content = new AccommodationReservationFormPage(LoggedUser, _accommodationRepository, _locationRepository, _accommodationImageRepository, _accommodationReservationRepository, _userRepository, SelectedAccommodation);
            }
        }
        private void DataGridAccommodations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedAccommodation != null) BookButton = true;
        }

        private void AnytimeAnywhereButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindow.MainPreview.Content = new AnytimeAnywherePage(LoggedUser);
        }
    }
}
