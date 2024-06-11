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
    /// Interaction logic for AcceptComplexTourRequestPartView.xaml
    /// </summary>
    public partial class AcceptComplexTourRequestPartView : Window
    {
        public AcceptComplexTourRequestPartView(TourRequest tourRequest, User guide)
        {
            InitializeComponent();
            this.DataContext = new AcceptComplexTourRequestPartViewModel(tourRequest, guide, this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
