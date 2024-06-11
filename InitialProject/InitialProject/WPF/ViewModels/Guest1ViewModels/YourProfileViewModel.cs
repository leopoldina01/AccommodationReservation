using InitialProject.Application.UseCases;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class YourProfileViewModel : ViewModelBase
    {
        #region PROPERTIES
        private User _user;
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    OnPropertyChanged(nameof(User));
                }
            }
        }
        private SuperGuest _superGuest;
        public SuperGuest SuperGuest
        {
            get
            {
                return _superGuest;
            }
            set
            {
                if (_superGuest != value)
                {
                    _superGuest = value;
                    OnPropertyChanged(nameof(SuperGuest));
                }
            }
        }
        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        private readonly SuperGuestService _superGuestService;
        #endregion

        public YourProfileViewModel(User user)
        {
            User = user;
            _superGuestService = new SuperGuestService();

            LoadProfile();
        }

        public void LoadProfile()
        {
            _superGuestService.ManageGuestRole(User);
            SuperGuest = _superGuestService.GetGuestByUserId(User.Id);
            if (SuperGuest == null)
                Status = "Normal guest";
            else
                Status = "Super guest";           
        }

    }
}
