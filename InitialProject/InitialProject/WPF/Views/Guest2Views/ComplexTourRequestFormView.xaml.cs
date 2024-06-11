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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InitialProject.WPF.Views.Guest2Views
{
    /// <summary>
    /// Interaction logic for ComplexTourRequestFormView.xaml
    /// </summary>
    public partial class ComplexTourRequestFormView : Page
    {
        public ComplexTourRequestFormView(User user)
        {
            InitializeComponent();
            this.DataContext = new ComplexTourRequestFormViewModel(user);
            Guest2MainWindow.mainWindow.Guest2MainPreview.Content = this;
        }

        /*private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxCountry.SelectedIndex = -1;
            ComboBoxCity.SelectedIndex = -1;
            ComboBoxGuide.SelectedIndex = -1;
            ComboBoxLanguage.SelectedIndex = -1;
            TextBoxGuestNumber.Clear();
            TextBoxDescription.Clear();
            DatePickerStart.DisplayDate = DateTime.Now;
            DatePickerEnd.DisplayDate = DateTime.Now;
        }*/
    }
}
