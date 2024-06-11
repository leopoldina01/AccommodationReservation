using InitialProject.Domain.Models;
using InitialProject.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for GuestsOverview.xaml
    /// </summary>
    public partial class GuestsOverview : Window
    {
        public int _ownerId;

        public ObservableCollection<AccommodationReservation> Reservations { get; set; }

        public readonly AccommodationReservationRepository _reservationRepository;
        public readonly AccommodationRepository _accommodationRepository;
        public readonly UserRepository _userRepository;
        public readonly GuestRatingRepository _ratingRepository;

        public AccommodationReservation SelectedReservation { get; set; }


        public GuestsOverview(int ownerId, AccommodationReservationRepository accommodationReservationRepository, AccommodationRepository accommodationRepository, UserRepository userRepository, GuestRatingRepository ratingRepository)
        {
            InitializeComponent();
            this.DataContext = this;
            _ownerId = ownerId;
            _reservationRepository = accommodationReservationRepository;
            _accommodationRepository = accommodationRepository;
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;

            LoadReservations();
        }

        public void LoadReservations()
        {
            List<AccommodationReservation> _ownerReservations = _reservationRepository.GetAllByOwnerId(_ownerId, _accommodationRepository, _userRepository);

            Reservations = new ObservableCollection<AccommodationReservation>(_reservationRepository.GetAllByOwnerId(_ownerId, _accommodationRepository, _userRepository));
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DataGridGuests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                ButtonRate.IsEnabled = true;

                if ((DateTime.Now - SelectedReservation.EndDate).Days > 5)
                {
                    MessageBox.Show("Selected reservation can't be rated", "It's been more than 5 days", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if ((DateTime.Now - SelectedReservation.EndDate).Days < 0)
                {
                    MessageBox.Show("Selected reservation can't be rated", "Guest hasn't left yet", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (_ratingRepository.GetAll().Find(r => r.ReservationId == SelectedReservation.Id) != null)
                {
                    MessageBox.Show("Selected reservation can't be rated", "It is already rated", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        

        private void ButtonRate_Click(object sender, RoutedEventArgs e)
        {


            if ((DateTime.Now - SelectedReservation.EndDate).Days < 0)
            {
                MessageBox.Show("Selected reservation can't be rated", "Guest hasn't left yet", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (SelectedReservation != null && (DateTime.Now - SelectedReservation.EndDate).Days <= 5 && _ratingRepository.GetAll().Find(r => r.ReservationId == SelectedReservation.Id) == null)
            {
                RatingGuestForm ratingGuestForm = new RatingGuestForm(_ratingRepository, SelectedReservation, _ownerId);
                ratingGuestForm.ShowDialog();
            }
            else if ((DateTime.Now - SelectedReservation.EndDate).Days > 5)
            {
                MessageBox.Show("Selected reservation can't be rated", "It's been more than 5 days", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (_ratingRepository.GetAll().Find(r => r.ReservationId == SelectedReservation.Id) != null)
            {
                MessageBox.Show("Selected reservation can't be rated", "It is already rated", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
