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
    public class TourReviewDetailsViewModel : ViewModelBase
    {
        #region PROPERTIES
        public TourReview Review { get; set; }
        public string UserInfo { get; set; }

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

        public ObservableCollection<BitmapImage> Images;

        private readonly TourReviewImageService _tourReviewImageService;
        #endregion

        public TourReviewDetailsViewModel(TourReview review)
        {
            _tourReviewImageService = new TourReviewImageService();

            Review = review;

            UserInfo = $"User \"{Review.Arrival.Reservation.User.Username}\" arrived at checkpoint \"{Review.Arrival.Checkpoint.Name}\"";

            Images = new ObservableCollection<BitmapImage>();

            LoadImages();
            LoadSelectedImage();

            NextImageCommand = new RelayCommand(NextImageCommand_Execute, NextImageCommand_CanExecute);
            PreviousImageCommand = new RelayCommand(PreviousImageCommand_Execute, PreviousImageCommand_CanExecute);
        }

        private void LoadSelectedImage()
        {
            SelectedImage = Images[0];
        }

        private void LoadImages()
        {
            Images.Clear();
            foreach (var image in _tourReviewImageService.GetAllByReview(Review))
            {
                Images.Add(new BitmapImage(new Uri(image.Url)));
            }
        }

        #region COMMANDS
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
        #endregion
    }
}
