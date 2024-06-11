using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels.OwnerViewModels;
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
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for AccommodationMonthlyStatisticsOverview.xaml
    /// </summary>
    public partial class AccommodationMonthlyStatisticsOverview : Window
    {
        public AccommodationMonthlyStatisticsOverview(Accommodation selectedAccommodation, AccommodationYearStatistic selectedYearStatistic)
        {
            InitializeComponent();
            this.DataContext = new AccommodationMonthlyStatisticsOverviewViewModel(selectedAccommodation, selectedYearStatistic);

        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.O && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                this.Close();
            }
        }
    }
}
