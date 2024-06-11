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
    /// Interaction logic for ReservationChangeRequestsPage.xaml
    /// </summary>
    public partial class ReservationChangeRequestsPage : Page
    {
        public ReservationChangeRequestsPage(ReservationChangeRequestsViewModel changeRequestsViewModel)
        {
            InitializeComponent();
            this.DataContext = changeRequestsViewModel;
            MainWindow.mainWindow.MainPreview.Content = this;
        }
    }
}
