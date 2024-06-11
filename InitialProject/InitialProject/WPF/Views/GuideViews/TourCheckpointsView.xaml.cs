using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
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

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for TourCheckpointsView.xaml
    /// </summary>
    public partial class TourCheckpointsView : Window
    {
        public TourCheckpointsView(Tour tour, TodaysToursViewModel todaysToursViewModel)
        {
            InitializeComponent();
            this.DataContext = new TourCheckpointsViewModel(tour, todaysToursViewModel);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
