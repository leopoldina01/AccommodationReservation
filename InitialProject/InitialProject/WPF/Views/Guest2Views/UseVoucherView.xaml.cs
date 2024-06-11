﻿using InitialProject.Domain.Models;
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
    /// Interaction logic for UseVoucher.xaml
    /// </summary>
    public partial class UseVoucherView : Page
    {
        public UseVoucherView(User user)
        {
            InitializeComponent();
            this.DataContext = new UseVoucherViewModel(user);
            Guest2MainWindow.mainWindow.Guest2MainPreview.Content = this;
        }
    }
}
