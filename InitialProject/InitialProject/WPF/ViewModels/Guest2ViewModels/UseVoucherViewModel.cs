using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;

namespace InitialProject.WPF.ViewModels.Guest2ViewModels
{
    public class UseVoucherViewModel : ViewModelBase
    {
        #region PROPERTIES

        public User LoggedUser { get; set; }

        private Voucher _selectedVoucher;
        public Voucher SelectedVoucher
        {
            get => _selectedVoucher;
            set
            {
                if (_selectedVoucher != value)
                {
                    _selectedVoucher = value;
                    OnPropertyChanged("SelectedVoucher");
                }
            }
        }

        private bool _isEnabledUseVoucher;
        public bool IsEnabledUseVoucher
        {
            get => _isEnabledUseVoucher;
            set
            {
                if (_isEnabledUseVoucher != value)
                {
                    _isEnabledUseVoucher = value;
                    OnPropertyChanged("IsEnableUseVoucher");
                }
            }
        }


        public ObservableCollection<Voucher> Vouchers { get; set; }
        private readonly VoucherService _voucherService;

        #endregion

        public UseVoucherViewModel(User user)
        {
            Vouchers = new ObservableCollection<Voucher>();
            _voucherService = new VoucherService();
            LoggedUser = user;
            
            LoadVouchers();
            //HasVouchers();

            CancelCommand = new RelayCommand(CancelCommand_Execute);
            UseVoucherCommand = new RelayCommand(UseVoucherCommand_Execute);
        }

        public void LoadVouchers()
        {
            foreach (var voucher in _voucherService.LoadAllById(LoggedUser.Id))
            {
                Vouchers.Add(voucher);
            }
        }

        #region COMMANDS
        public RelayCommand CancelCommand { get; }
        public RelayCommand UseVoucherCommand { get; }
        public void UseVoucherCommand_Execute(object? parameter)
        {
            if(SelectedVoucher != null)
            {
                MessageBox.Show("Your reservation was successful.");
                _voucherService.RemoveVoucher(SelectedVoucher);
                MessageBox.Show("Congratulations, you've used one voucher!");
                Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
                //_useVouchersView.Close();
            }
        }
        public void CancelCommand_Execute(object? parameter)
        {
            MessageBox.Show("Your reservation was successful.");
            Guest2TourView guest2TourView = new Guest2TourView(LoggedUser);
            //_useVouchersView.Close();
        }
        #endregion
    }
}
