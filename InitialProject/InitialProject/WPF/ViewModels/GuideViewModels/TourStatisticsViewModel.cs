using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels
{
    public class TourStatisticsViewModel : ViewModelBase
    {
        private int? _youngerThanEighteen;
        public int? YoungerThanEighteen
        {
            get
            {
                return _youngerThanEighteen;
            }
            set
            {
                if (_youngerThanEighteen != value)
                {
                    _youngerThanEighteen = value;
                    OnPropertyChanged(nameof(YoungerThanEighteen));
                }
            }
        }

        private int? _eighteenToFifty;
        public int? EighteenToFifty
        {
            get
            {
                return _eighteenToFifty;
            }
            set
            {
                if (_eighteenToFifty != value)
                {
                    _eighteenToFifty = value;
                    OnPropertyChanged(nameof(EighteenToFifty));
                }
            }
        }

        private int? _olderThanFifty;
        public int? OlderThanFifty
        {
            get
            {
                return _olderThanFifty;
            }
            set
            {
                if (_olderThanFifty != value)
                {
                    _olderThanFifty = value;
                    OnPropertyChanged(nameof(OlderThanFifty));
                }
            }
        }

        private float? _withVoucher;
        public float? WithVoucher
        {
            get
            {
                return _withVoucher;
            }
            set
            {
                if (_withVoucher != value)
                {
                    _withVoucher = value;
                    OnPropertyChanged(nameof(WithVoucher));
                }
            }
        }

        private float? _withoutVoucher;
        public float? WithoutVoucher
        {
            get
            {
                return _withoutVoucher;
            }
            set
            {
                if (_withoutVoucher != value)
                {
                    _withoutVoucher = value;
                    OnPropertyChanged(nameof(WithoutVoucher));
                }
            }
        }

        private readonly TourStatisticsService _tourStatisticsService;

        public TourStatisticsViewModel(Tour tour)
        {
            _tourStatisticsService = new TourStatisticsService();

            YoungerThanEighteen = _tourStatisticsService.GetNumberOfGusetsInAgeRange(tour, 0, 18);
            EighteenToFifty = _tourStatisticsService.GetNumberOfGusetsInAgeRange(tour, 18, 50);
            OlderThanFifty = _tourStatisticsService.GetNumberOfGusetsInAgeRange(tour, 18, 200);
        }
    }
}
