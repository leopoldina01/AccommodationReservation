using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest2Views;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class VouchersViewModel : ViewModelBase
    {
        #region PROPERTIES
        public User LoggedUser { get; set; }

        public ObservableCollection<Voucher> Vouchers { get; set; }
        private readonly VoucherService _voucherService;
        

        #endregion

        public VouchersViewModel(User user)
        {
            LoggedUser = user;
            Vouchers = new ObservableCollection<Voucher>();
            _voucherService = new VoucherService();
            LoadVouchers();
            HomeCommand = new RelayCommand(HomeCommand_Execute);
        }

        public void LoadVouchers()
        {
            foreach(var voucher in _voucherService.LoadAllById(LoggedUser.Id))
            {
                Vouchers.Add(voucher);
            }
        }

        #region COMMANDS
        public RelayCommand HomeCommand { get; }

        public void HomeCommand_Execute(object? parameter)
        {
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
        }
        #endregion
    }
}
