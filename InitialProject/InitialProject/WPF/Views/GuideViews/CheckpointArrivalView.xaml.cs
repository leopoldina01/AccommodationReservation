using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CheckpointsArrivalView.xaml
    /// </summary>
    public partial class CheckpointArrivalView : Window
    {
        public CheckpointArrivalView(Checkpoint currentCheckpoint, Tour currentTour)
        {
            InitializeComponent();
            this.DataContext = new CheckpointArrivalViewModel(currentCheckpoint, currentTour);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
