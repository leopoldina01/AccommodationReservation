using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.OwnerViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class ForumOverviewPageViewModel : ViewModelBase
    {
        #region PROPERTIES
        private Forum _selectedForum;
        public Forum SelectedForum
        {
            get
            {
                return _selectedForum;
            }
            set
            {
                if (value != _selectedForum)
                {
                    _selectedForum = value;
                    OnPropertyChanged(nameof(SelectedForum));
                }
            }
        }

        User _owner;
        public ObservableCollection<Forum> Forums { get; set; }

        private readonly ForumService _forumService;
        #endregion

        public ForumOverviewPageViewModel(User owner) 
        {
            Forums = new ObservableCollection<Forum>();
            _owner = owner;

            _forumService = new ForumService();

            LoadForums();

            OpenForumCommand = new RelayCommand(OpenForumCommand_Execute, OpenForumCommand_CanExecute);
        }

        private void LoadForums()
        {
            Forums.Clear();

            foreach(var forum in _forumService.GetAll())
            {
                Forums.Add(forum);
            }
        }

        #region COMMANDS
        public RelayCommand OpenForumCommand { get; }

        public void OpenForumCommand_Execute(object? parameter)
        {
            ForumWindow forumWindow = new ForumWindow(SelectedForum, _owner);
            forumWindow.Show();
        }

        public bool OpenForumCommand_CanExecute(object? parameter)
        {
            return SelectedForum != null;
        }
        #endregion
    }
}
