using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Localization;
using InitialProject.Repositories;
using InitialProject.WPF.ViewModels;
using InitialProject.WPF.ViewModels.Guest1ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _themeButton;
        public string ThemeButton
        {
            get => _themeButton;
            set
            {
                if (_themeButton != value)
                {
                    _themeButton = value;
                    OnPropertyChanged("ThemeButton");
                }
            }
        }
        private string _language;
        public string SelectedLanguage
        {
            get => _language;
            set
            {
                if (_language != value)
                {
                    _language = value;
                    OnPropertyChanged("SelectedLanguage");
                }
            }
        }
        public static MainWindow mainWindow;

        private App app;
        private const string SRB = "sr-Latn-RS";
        private const string ENG = "en-US";
        public User LoggedUser { get; set; }
        public readonly AccommodationRepository _accommodationRepository;
        public readonly LocationRepository _locationRepository;
        public readonly AccommodationImageRepository _accommodationImageRepository;
        public readonly AccommodationReservationRepository _accommodationReservationRepository;
        public readonly UserRepository _userRepository;

        private readonly AccommodationRenovationService _accommodationRenovationService;

        public MainWindow(User user, AccommodationRepository accommodationRepository, LocationRepository locationRepository, AccommodationImageRepository accommodationImageRepository, AccommodationReservationRepository accommodationReservationRepository, UserRepository userRepository)
        {
            InitializeComponent();
            this.DataContext = this;
            mainWindow = this;

            _accommodationRepository = accommodationRepository;
            _locationRepository = locationRepository;
            _accommodationImageRepository = accommodationImageRepository;
            _accommodationReservationRepository = accommodationReservationRepository;
            _userRepository = userRepository;
            _accommodationRenovationService = new AccommodationRenovationService();

            SelectedLanguage = "English";
            LoggedUser = user;
            ThemeButton = "OFF";
            app = (App)System.Windows.Application.Current;
            app.ChangeLanguage(ENG);

            MainPreview.Content = new AccommodationsPage(LoggedUser, _accommodationRepository, _locationRepository, _accommodationImageRepository, _accommodationReservationRepository, _userRepository);

            UpdateRenovationInformations();
        }

        private async void UpdateRenovationInformations()
        {
            if (_accommodationRenovationService.GetAllFinishedTodayAndNotMarked().Count() != 0)
            {
                SetAccommodationStatusRenovated(_accommodationRenovationService.GetAllFinishedTodayAndNotMarked());
            }

            if (_accommodationRenovationService.GetAllFinishedInLastYearAndNotMarked().Count() != 0)
            {
                SetAccommodationStatusRenovated(_accommodationRenovationService.GetAllFinishedInLastYearAndNotMarked());
            }

            if (_accommodationRenovationService.GetAllRenovatedBeforeMoreThanAYear().Count() != 0)
            {
                RemoveAccommodationStatusRenovated(_accommodationRenovationService.GetAllRenovatedBeforeMoreThanAYear());
            }

            await Task.Delay(TimeSpan.FromDays(1));
            UpdateRenovationInformations();
        }

        public void RemoveAccommodationStatusRenovated(List<AccommodationRenovation> renovations)
        {
            List<AccommodationRenovation> accommodationRenovations = renovations;

            foreach (var renovation in accommodationRenovations)
            {
                renovation.Accommodation.RenovationStatus = "";
                _accommodationRepository.Update(renovation.Accommodation);
            }
        }

        public void SetAccommodationStatusRenovated(List<AccommodationRenovation> finishedAndNotMarkedRenovations)
        {
            List<AccommodationRenovation> finishedRenovations = finishedAndNotMarkedRenovations;

            foreach (var renovation in finishedRenovations)
            {
                renovation.Accommodation.RenovationStatus = " RENOVATED";
                _accommodationRepository.Update(renovation.Accommodation);
            }
        }

        private void AccommodationsButton_Click(object sender, RoutedEventArgs e)
        {
            MainPreview.Content = new AccommodationsPage(LoggedUser, _accommodationRepository, _locationRepository, _accommodationImageRepository, _accommodationReservationRepository, _userRepository);
        }

        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void ReservationsButton_Click(object sender, RoutedEventArgs e)
        {
            MainPreview.Content = new ReservationsPage(new ReservationsViewModel(LoggedUser.Id));
        }

        private void DarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            var app = (App)System.Windows.Application.Current;
            app.ChangeTheme(new Uri("WPF/Styles/DarkTheme.xaml", UriKind.Relative));
            ThemeButton = "ON";
        }

        private void DarkTheme_Unchecked(object sender, RoutedEventArgs e)
        {
            var app = (App)System.Windows.Application.Current;
            app.ChangeTheme(new Uri("WPF/Styles/LightTheme.xaml", UriKind.Relative));
            ThemeButton = "OFF";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ReviewsButton_Click(object sender, RoutedEventArgs e)
        {
            MainPreview.Content = new ReviewsPage(LoggedUser);
        }

        private void YourProfileButton_Click(object sender, RoutedEventArgs e)
        {
            MainPreview.Content = new YourProfilePage(LoggedUser);
        }

        private void ForumButton_Click(object sender, RoutedEventArgs e)
        {
            MainPreview.Content = new ForumPage(LoggedUser);
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            MainPreview.Content = new NotificationsPage(LoggedUser);
        }

        private void ChangeLanguage(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedLanguage.Equals("English"))
            {
                app.ChangeLanguage(ENG);
            }
            else if (SelectedLanguage.Equals("Serbian"))
            {
                app.ChangeLanguage(SRB);
            }
        }
    }
}
