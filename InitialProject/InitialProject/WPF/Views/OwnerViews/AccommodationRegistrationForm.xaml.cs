using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.ViewModels.OwnerViewModels;
using InitialProject.WPF.Views.OwnerViews;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
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
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for AccommodationRegistrationForm.xaml
    /// </summary>
    public partial class AccommodationRegistrationForm : Window
    {
        public ObservableCollection<Accommodation> _myAccommodations;

        public AccommodationRegistrationForm(AccommodationRepository accommodationRepository, LocationRepository locationRepository, AccommodationImageRepository imageRepository, int ownerId, UserRepository userRepository, ObservableCollection<Accommodation> MyAccommodations)
        {
            InitializeComponent();
            this.DataContext = new AccommodationRegistrationFormViewModel(this, accommodationRepository, ownerId, locationRepository, imageRepository, userRepository);
            
            _myAccommodations = MyAccommodations;
        }
    }
}
