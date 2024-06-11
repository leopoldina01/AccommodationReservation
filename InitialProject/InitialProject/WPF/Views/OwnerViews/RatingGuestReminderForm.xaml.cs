using InitialProject.Repositories;
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
    /// Interaction logic for RatingGuestReminderForm.xaml
    /// </summary>
    public partial class RatingGuestReminderForm : Window
    {
        public RatingGuestReminderForm()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void ButtonRemindMeLater_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
