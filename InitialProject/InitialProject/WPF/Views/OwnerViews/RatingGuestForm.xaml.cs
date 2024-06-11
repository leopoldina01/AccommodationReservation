using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using InitialProject.Domain.RepositoryInterfaces;
using InitialProject.Repositories;
using InitialProject.WPF.ViewModels.OwnerViewModels;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for RatingGuestForm.xaml
    /// </summary>
    public partial class RatingGuestForm : Window
    {
        public RatingGuestForm(GuestRatingRepository ratingRepository, AccommodationReservation selectedReservation, int ownerId)
        {
            InitializeComponent();
            this.DataContext = new RatingGuestFormViewModel(this, ratingRepository, selectedReservation, ownerId);
        }
    }
}
