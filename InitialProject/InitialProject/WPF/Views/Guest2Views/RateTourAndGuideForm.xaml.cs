using InitialProject.Domain.DTOs;
using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels.Guest2ViewModels;
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
    /// Interaction logic for RateTourAndGuideForm.xaml
    /// </summary>
    public partial class RateTourAndGuideForm : Page
    {
        public RateTourAndGuideForm(int tourId ,User user)
        {
            InitializeComponent();
            this.DataContext = new RateTourAndGuideFormViewModel(tourId, user);
            Guest2MainWindow.mainWindow.Guest2MainPreview.Content = this;
        }
    }
}
