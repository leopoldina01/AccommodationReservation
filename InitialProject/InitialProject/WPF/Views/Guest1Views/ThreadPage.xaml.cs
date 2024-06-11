using InitialProject.Domain.Models;
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
    /// Interaction logic for ThreadPage.xaml
    /// </summary>
    public partial class ThreadPage : Page
    {
        public ThreadPage(User user, Forum selectedForum)
        {
            InitializeComponent();
            this.DataContext = new ThreadViewModel(user, selectedForum);
            MainWindow.mainWindow.MainPreview.Content = this;
        }
    }
}
