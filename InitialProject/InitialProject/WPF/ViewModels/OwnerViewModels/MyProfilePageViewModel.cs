using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class MyProfilePageViewModel : ViewModelBase
    {
        #region PROPERTIES
        private int _numberOfRatings;
        public int NumberOfRagings
        {
            get
            {
                return _numberOfRatings;
            }
            set
            {
                if (_numberOfRatings != value)
                {
                    _numberOfRatings = value;
                    OnPropertyChanged(nameof(NumberOfRagings));
                }
            }
        }

        private double _totalRating;
        public double TotalRating
        {
            get
            {
                return _totalRating;
            }
            set
            {
                if (_totalRating != value)
                {
                    _totalRating = value;
                    OnPropertyChanged(nameof(TotalRating));
                }
            }
        }

        private User _owner;

        public User Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged(nameof(Owner));
                }
            }
        }

        private readonly int _ownerId;

        private readonly AccommodationRatingService _accommodationRatingService;
        private readonly UserService _userService;
        private readonly SetOwnerRoleService _setOwnerRoleService;

        public SeriesCollection SeriesCollection { get; set; }
        public Func<int, string> Formatter { get; set; }
        public string[] Labels { get; set; }
        private HashSet<int> formattedValues = new HashSet<int>();
        #endregion

        public MyProfilePageViewModel(int ownerId)
        {
            _accommodationRatingService = new AccommodationRatingService();
            _setOwnerRoleService = new SetOwnerRoleService();
            _userService = new UserService();

            _ownerId = ownerId;

            Labels = new string[0];

            LoadColumns();
            LoadReviews();
            SetOwnerRole();
            FindOwner();
        }

        private void LoadColumns()
        {
            SeriesCollection = new SeriesCollection() {
                new ColumnSeries
                {
                    Title = "Cleanliness",
                    Values = new ChartValues<int>(),
                    ColumnPadding = -10
                },
                new ColumnSeries
                {
                    Title = "Correctness",
                    Values = new ChartValues<int>(),
                    ColumnPadding = -10
                }
            };
        }

        private void LoadReviews()
        {
            for (int i = 1; i <= 5; i++)
            {
                int numberForCorrectness = _accommodationRatingService.CalculateNumberOfRatingsForCorrectness(i, _ownerId);
                int numberForCleanliness = _accommodationRatingService.CalculateNumberOfRatingsForCleanliness(i, _ownerId);

                Labels = Labels.Concat(new[] { i.ToString() }).ToArray();

                SeriesCollection[0].Values.Add(numberForCleanliness);
                SeriesCollection[1].Values.Add(numberForCorrectness);

                Formatter = value =>
                {
                    if (!formattedValues.Contains(value))
                    {
                        formattedValues.Add(value);
                        return value.ToString("N0");
                    }
                    else
                    {
                        return string.Empty;
                    }
                };
            }
        }

        private void CalculateNumberOfRatings()
        {
            NumberOfRagings = _setOwnerRoleService.CalculateNumberOfRatings(_ownerId);
        }

        private void CalculateTotalRating()
        {
            TotalRating = Math.Round(_setOwnerRoleService.CalculateTotalRating(_ownerId), 2);
        }

        private void FindOwner()
        {
            Owner = _userService.FindOwnerById(_ownerId);
        }

        private void SetOwnerRole()
        {
            CalculateNumberOfRatings();
            CalculateTotalRating();

            _setOwnerRoleService.SetOwnerRole(_ownerId);
        }
    }
}
