using InitialProject.Domain.Models;
using InitialProject.Repositories;
using InitialProject.WPF.ViewModels.GuideViewModels;
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
    /// Interaction logic for GuideMenuView.xaml
    /// </summary>
    public partial class GuideMenuView : Window
    {
        public GuideMenuView(User guide)
        {
            InitializeComponent();
            this.DataContext = new GuideMenuViewModel(guide);
        }

        private void ButtonLogOut_Click(object sender, RoutedEventArgs e)
        {
            SignInForm signInForm = new SignInForm();
            signInForm.Show();
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F11)
            {
                this.ButtonLogOut_Click(sender, e);
            }
            else if (e.Key == Key.F12)
            {
                this.ButtonClose_Click(sender, e);
            }
        }
    }
}
