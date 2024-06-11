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
    /// Interaction logic for ForumsOverviewPage.xaml
    /// </summary>
    public partial class ForumsOverviewPage : Page
    {
        public ForumsOverviewPage(User owner)
        {
            InitializeComponent();
            this.DataContext = new ForumOverviewPageViewModel(owner);
        }
    }
}
