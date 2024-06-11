using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using InitialProject.WPF.Views.Guest1Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ForumViewModel : ViewModelBase
    {
        #region PROPERTIES
        private Forum? _selectedForum;
        public Forum? SelectedForum
        {
            get
            {
                return _selectedForum;
            }
            set
            {
                if (_selectedForum != value)
                {
                    _selectedForum = value;
                    OnPropertyChanged(nameof(SelectedForum));
                }
            }
        }
        public User LoggedUser;
        public ObservableCollection<Forum> Forums { get; set; }

        private readonly ForumService _forumService;
        #endregion
        public ForumViewModel(User user)
        {
            _forumService = new ForumService();

            LoggedUser = user;

            Forums = new ObservableCollection<Forum>();

            NewThreadCommand = new RelayCommand(NewThreadCommand_Execute);
            CloseForumCommand = new RelayCommand(CloseForumCommand_Execute, CloseForumCommand_CanExecute);
            ViewForumCommand = new RelayCommand(ViewForumCommand_Execute);

            LoadForums();
        }

        public void LoadForums()
        {
            Forums.Clear();
            var forums = _forumService.GetAll();
            foreach(Forum forum in forums)
            {
                Forums.Add(forum);
            }
        }

        #region COMMANDS
        public RelayCommand ViewForumCommand { get; }
        public RelayCommand CloseForumCommand { get; }
        public RelayCommand NewThreadCommand { get; }

        public void ViewForumCommand_Execute(object? parameter)
        {
            MainWindow.mainWindow.MainPreview.Content = new ThreadPage(LoggedUser, SelectedForum);
        }

        public void NewThreadCommand_Execute(object? parameter)
        {
            MainWindow.mainWindow.MainPreview.Content = new CreateThreadPage(LoggedUser);
        }

        public void CloseForumCommand_Execute(object? parameter)
        {
            _forumService.Close(SelectedForum);
            LoadForums();
        }

        public bool CloseForumCommand_CanExecute(object? parameter)
        {
            return SelectedForum != null && SelectedForum.CreatorId == LoggedUser.Id && SelectedForum.Status.Equals("Open");
        }
        #endregion
    }
}
