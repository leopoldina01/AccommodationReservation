using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels.Guest1ViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for RenovationSuggestionPage.xaml
    /// </summary>
    public partial class RenovationSuggestionPage : Page
    {
        public RenovationSuggestionPage(AccommodationReservation reservation)
        {
            InitializeComponent();
            this.DataContext = new RenovationSuggestionViewModel(reservation);
            MainWindow.mainWindow.MainPreview.Content = this;
        }
    }
}
