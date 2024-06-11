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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.OwnerViews
{
    /// <summary>
    /// Interaction logic for MyAccommodationsPage.xaml
    /// </summary>
    public partial class MyAccommodationsPage : Page
    {
        public MyAccommodationsPage(User user)
        {
            InitializeComponent();
            this.DataContext = new MyAccommodationsPageViewModel(this, user);
        }

        private void DataGridAccommodations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ButtonRenovate.IsEnabled = true;
            ButtonStatistics.IsEnabled = true;
        }
    }
}
