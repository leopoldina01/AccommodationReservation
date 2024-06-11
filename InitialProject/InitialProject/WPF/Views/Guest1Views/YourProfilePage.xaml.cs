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
using InitialProject.WPF.ViewModels.Guest1ViewModels;
using InitialProject.Domain.Models;

namespace InitialProject.WPF.Views.Guest1Views
{
    /// <summary>
    /// Interaction logic for YourProfilePage.xaml
    /// </summary>
    public partial class YourProfilePage : Page
    {
        public YourProfilePage(User user)
        {
            InitializeComponent();
            this.DataContext = new YourProfileViewModel(user);
            MainWindow.mainWindow.MainPreview.Content = this;
        }
    }
}
