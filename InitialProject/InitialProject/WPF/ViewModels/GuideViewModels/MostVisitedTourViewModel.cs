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
using System.Windows;
using System.Windows.Media.Imaging;

namespace InitialProject.WPF.ViewModels
{
    public class MostVisitedTourViewModel : ViewModelBase
    {
		#region PROPERTIES
		private bool _isAllTimeRBChecked;
		public bool IsAllTimeRBChecked
		{
			get
			{
				return _isAllTimeRBChecked;
			}
			set
			{
				if (_isAllTimeRBChecked != value)
				{
					_isAllTimeRBChecked = value;
					OnPropertyChanged(nameof(IsAllTimeRBChecked));
					LoadData();
				}
			}
		}

		private bool _isYearlyRBChecked;
		public bool IsYearlyRBChecked
		{
			get
			{
				return _isYearlyRBChecked;
			}
			set
			{
				if (_isYearlyRBChecked != value)
				{
					_isYearlyRBChecked = value;
					OnPropertyChanged(nameof(IsYearlyRBChecked));
				}
			}
		}

		private BitmapImage _selectedImage;
		public BitmapImage SelectedImage
		{
			get
			{
				return _selectedImage;
			}
			set
			{
				if (_selectedImage != value)
				{
					_selectedImage = value;
					OnPropertyChanged(nameof(SelectedImage));
				}
			}
		}

		private Tour _displayedTour;
		public Tour DisplayedTour
		{
			get
			{
				return _displayedTour;
			}
			set
			{
				if (_displayedTour != value)
				{
					_displayedTour = value;
					OnPropertyChanged(nameof(DisplayedTour));
				}
			}
		}

		private int? _selectedYear;
		public int? SelectedYear
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
					OnPropertyChanged(nameof(SelectedYear));
					LoadData();
				}
			}
		}

		public ObservableCollection<DateTime> StartTimes { get; set; }
		public ObservableCollection<Checkpoint> Checkpoints { get; set; }
		public ObservableCollection<BitmapImage> Images { get; set; }
		public ObservableCollection<int> PossibleYears { get; set; }

		private readonly MostVisitedTourService _mostVisitedTourService;
		private readonly CheckpointService _checkpointService;
		private readonly TourService _tourService;
		private readonly TourImageService _tourImageService;
		#endregion

		public MostVisitedTourViewModel()
        {
            _mostVisitedTourService = new MostVisitedTourService();
            _checkpointService = new CheckpointService();
			_tourService = new TourService();
			_tourImageService = new TourImageService();


            PossibleYears = new ObservableCollection<int>(_mostVisitedTourService.GetYearsThatHaveTours());
            Checkpoints = new ObservableCollection<Checkpoint>();
			StartTimes = new ObservableCollection<DateTime>();
			Images = new ObservableCollection<BitmapImage>();

            IsAllTimeRBChecked = true;
            IsYearlyRBChecked = false;

            LoadData();

			OpenStatsCommand = new RelayCommand(OpenStatsCommand_Execute);
			NextImageCommand = new RelayCommand(NextImageCommand_Execute, NextImageCommand_CanExecute);
			PreviousImageCommand = new RelayCommand(PreviousImageCommand_Execute, PreviousImageCommand_CanExecute);
        }

        private void LoadData()
        {
            LoadDisplayedTour();
            LoadCheckpoints();
			LoadStartTimes();
			LoadImages();
			LoadSelectedImage();
        }

        private void LoadSelectedImage()
        {
			if (Images.Count != 0)
			{
				SelectedImage = Images[0];
			}
        }

        private void LoadImages()
        {
			Images.Clear();
			foreach (var image in _tourImageService.GetAllByTour(DisplayedTour))
			{
				Images.Add(new BitmapImage(new Uri(image.Url)));
			}
        }

        private void LoadStartTimes()
        {
			StartTimes.Clear();
			foreach (var startTime in _tourService.GetAllStartTimesForTour(DisplayedTour))
			{
				StartTimes.Add(startTime);
			}
        }

        private void LoadDisplayedTour()
		{
			if (IsAllTimeRBChecked || SelectedYear is null)
			{
				DisplayedTour = _mostVisitedTourService.GetAllTimeMostVisitedTour();
			}
			else
			{
				DisplayedTour = _mostVisitedTourService.GetMostVisitedTourByYear((int)SelectedYear);
			}
		}

		private void LoadCheckpoints()
		{
			Checkpoints.Clear();
			foreach (var checkpoint in _checkpointService.GetAllByTour(DisplayedTour))
			{
				Checkpoints.Add(checkpoint);
			}
		}

		#region COMMANDS
		public RelayCommand OpenStatsCommand { get; }
		public RelayCommand CloseWindowCommand { get; }
		public RelayCommand NextImageCommand { get; }
		public RelayCommand PreviousImageCommand { get; }

		public void PreviousImageCommand_Execute(object? prameter)
		{
            for (int i = 0; i < Images.Count; i++)
            {
                if (SelectedImage == Images[i])
                {
                    SelectedImage = Images[i - 1];
                    return;
                }
            }
        }

		public bool PreviousImageCommand_CanExecute(object? parameter)
		{
			return SelectedImage != Images.First();
		}

		public void NextImageCommand_Execute(object? parameter)
		{
			for (int i = 0; i < Images.Count; i++)
			{
				if (SelectedImage == Images[i])
				{
					SelectedImage = Images[i + 1];
					return;
				}
			}
		}

		public bool NextImageCommand_CanExecute(object? parameter)
		{
			return SelectedImage != Images.Last();
		}

		public void OpenStatsCommand_Execute(object? parameter)
		{
			var tourStatisticsView = new TourStatisticsView(DisplayedTour);
			tourStatisticsView.Show();
		}
		#endregion
	}
}
