using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
using System.Windows;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for ToursUserReviewsView.xaml
    /// </summary>
    public partial class ToursUserReviewsView : Window
    {
        public ToursUserReviewsView(Tour tour)
        {
            InitializeComponent();
            this.DataContext = new ToursUserReviewsViewModel(tour);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
