using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest1Views;
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

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for Guest2MainWindow.xaml
    /// </summary>
    /// 
    public partial class Guest2MainWindow : Window
    {
        public static Guest2MainWindow mainWindow;
        public User LoggedUser { get; set; }

        public Guest2MainWindow(User user)
        {
            InitializeComponent();
            this.DataContext = this;
            mainWindow = this;

            LoggedUser = user;
            Guest2MainPreview.Content = new Guest2TourView(user);
        }

        private void ToursButton_Click(object sender, RoutedEventArgs e)
        {
            Guest2MainPreview.Content = new Guest2TourView(LoggedUser);
        }

        private void ReservedTours_Click(object sender, RoutedEventArgs e)
        {
            Guest2MainPreview.Content = new ReservedToursView(LoggedUser);
        }

        private void TourRequests_Click(object sender, RoutedEventArgs e)
        {
            Guest2MainPreview.Content = new TourRequestFormView(LoggedUser);
        }

        private void ComplexTours_Click(object sender, RoutedEventArgs e)
        {
            ComplexTourRequestFormView complexTourRequestFormView = new ComplexTourRequestFormView(LoggedUser);
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            Guest2MainPreview.Content = new RequestedTourStatisticsView(LoggedUser);
        }

        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            Guest2MainPreview.Content = new VouchersView(LoggedUser);
        }

        private void Notifications_Click(object sender, RoutedEventArgs e)
        {
            Guest2MainPreview.Content = new TourNotificationsView(LoggedUser);
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            Close();
        }
    }
}
