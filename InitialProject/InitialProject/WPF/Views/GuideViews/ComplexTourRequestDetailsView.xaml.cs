using InitialProject.Domain.Models;
using InitialProject.WPF.ViewModels;
using System.Windows;

namespace InitialProject.WPF.Views
{
    /// <summary>
    /// Interaction logic for ComplexTourRequestDetailsView.xaml
    /// </summary>
    public partial class ComplexTourRequestDetailsView : Window
    {
        public ComplexTourRequestDetailsView(ComplexTourRequest complexTourRequest, User guide)
        {
            InitializeComponent();
            this.DataContext = new ComplexTourRequestDetailsViewModel(complexTourRequest, guide);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
