using HarfBuzzSharp;
using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.WPF.Views;
using InitialProject.WPF.Views.Guest2Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class SelectedTourViewModel : ViewModelBase
    {
        #region PROPERTIES

        User LoggedUser { get; set; }
        

        private ObservableCollection<Tour> _availableTours;
        public ObservableCollection<Tour> AvailableTours
        {
            get => _availableTours;
            set
            {
                if (value != _availableTours)
                {
                    _availableTours = value;
                    OnPropertyChanged(nameof(AvailableTours));
                }
            }
        }

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get => _selectedTour;
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));
                }
            }
        }

        private int? _numberOfNewGuests;
        public int? NumberOfNewGuests
        {
            get => _numberOfNewGuests;
            set
            {
                if (_numberOfNewGuests != value)
                {
                    _numberOfNewGuests = value;
                    OnPropertyChanged(nameof(NumberOfNewGuests));
                }
            }
        }

        private int? _availableSlots;
        public int? AvailableSlots
        {
            get => _availableSlots;
            set
            {
                if (_availableSlots != value)
                {
                    _availableSlots = value;
                    OnPropertyChanged(nameof(AvailableSlots));
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

        private readonly VoucherService _voucherService;
        private readonly TourService _tourService;
        private readonly TourReservationService _tourReservationService;
        #endregion

        public SelectedTourViewModel(User user, Tour selectedTour)
        {
            LoggedUser = user;
            SelectedTour = selectedTour;

            _voucherService = new VoucherService();
            _tourService = new TourService();
            _tourReservationService = new TourReservationService();

            DemoButtonContent = "Demo";

            HomeCommand = new RelayCommand(HomeCommand_Execute);
            ReserveTourCommand = new RelayCommand(ReserveTourCommand_Execute);
            DemoCommand = new RelayCommand(DemoCommand_Execute);
        }

        public void UseVoucher()
        {
            UseVoucherView useVoucherView = new UseVoucherView(LoggedUser);
            //useVoucherView.Show();
        }

        public void OfferOtherTours()
        {
            AlternativeTourOffersView alternativeTourOffersView = new AlternativeTourOffersView(LoggedUser, SelectedTour);
            //alternativeTourOffersView .Show();
           // _selectedTourView.Close();
        }

        private void MakeNewReservation()
        {
            TourReservation tourReservation = new TourReservation();
            _tourReservationService.SaveReservation(SelectedTour.Id, LoggedUser.Id, (int)NumberOfNewGuests, 0); //to-do: add option to enter age
            SelectedTour.MaxGuests = SelectedTour.MaxGuests - (int)NumberOfNewGuests;
            _tourService.UpdateTour(SelectedTour);
        }

        public void CheckForYearlyVoucher(User user)
        {
            int count = 0;
            string ThisYear = "";
            List<TourReservation> _reservations = new List<TourReservation>();
            _reservations = _tourReservationService.GetAllByUserId(LoggedUser.Id);

            foreach (var reservation in _tourReservationService.GetAllByUserId(LoggedUser.Id))
            {
                reservation.Tour = _tourService.GetById(reservation.TourId);
                
                if(reservation.Tour.StartTime.Year == DateTime.Now.Year)
                {
                    ThisYear = "Year " + DateTime.Now.Year.ToString() + " Voucher";
                    count++;
                }
            }

            if (count == 5)
            {
                //TODO Daj vaucer
                Voucher voucher = new Voucher(ThisYear, DateTime.Now.AddMonths(6), LoggedUser.Id, 0);
                _voucherService.Save(voucher);
            }
        }


        private void RunDemo()
        {
            var demoNumberOfNewGuests = 2;

            Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(0.5)
            };

            this.ResetFields();
            int i = 0;

            Timer.Tick += delegate (object? sender, EventArgs e)
            {
                if (i < 10)
                {
                    NumberOfNewGuests = demoNumberOfNewGuests;
                }
                else if (i < 20)
                {
                    this.ReserveTourCommand_Execute(null);
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
            NumberOfNewGuests = 0;
        }

        #region COMMANDS

        public RelayCommand HomeCommand { get; }
        public RelayCommand ReserveTourCommand { get; }
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


        public void ReserveTourCommand_Execute(object? parameter)
        {
            if (SelectedTour.MaxGuests - NumberOfNewGuests < 0)
            {
                MessageBox.Show("There is no enough slots for this reservation!");
                OfferOtherTours();
            }
            else if (NumberOfNewGuests == 0 || NumberOfNewGuests == null || NumberOfNewGuests < 0)
            {
                MessageBox.Show("This field can't be empty!");
            }
            else
            {
                UseVoucher();
                MakeNewReservation();
                CheckForYearlyVoucher(LoggedUser);
                //_selectedTourView.Close();
            }
        }

        public void HomeCommand_Execute(object? parameter)
        {
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
            //guest2TourView.Show();
            //_selectedTourView.Close();
        }

        #endregion
    }
}
