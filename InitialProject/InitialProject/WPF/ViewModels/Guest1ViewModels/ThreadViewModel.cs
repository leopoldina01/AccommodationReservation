using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.WPF.ViewModels.Guest1ViewModels
{
    public class ThreadViewModel : ViewModelBase
    {
        #region PROPERTIES
        private string _newComment;
        public string NewComment
        {
            get
            {
                return _newComment;
            }
            set
            {
                if (_newComment != value)
                {
                    _newComment = value;
                    OnPropertyChanged(nameof(NewComment));
                }
            }
        }
        public Forum SelectedForum { get; set; }
        public User LoggedUser { get; set; }
        public ObservableCollection<Comment> Comments { get; set; }

        private readonly CommentService _commentService;
        #endregion
        public ThreadViewModel(User user, Forum selectedForum)
        {
            _commentService = new CommentService();

            SelectedForum = selectedForum;
            LoggedUser = user;

            Comments = new ObservableCollection<Comment>();

            PostCommentCommand = new RelayCommand(PostCommentCommand_Execute, PostCommentCommand_CanExecute);

            LoadComments();
        }

        public void LoadComments()
        {
            Comments.Clear();
            var comments =_commentService.GetAllByForumId(SelectedForum.Id);
            comments = _commentService.CheckIfUsersHaveBeenOnLocation(comments, SelectedForum.LocationId);
            foreach(Comment comment in comments)
            {
                Comments.Add(comment);
            }
        }

        #region COMMANDS
        public RelayCommand PostCommentCommand { get; }

        public void PostCommentCommand_Execute(object? parameter)
        {
            _commentService.Save(SelectedForum.Id, NewComment, LoggedUser.Id);
            LoadComments();
        }

        public bool PostCommentCommand_CanExecute(object? parameter)
        {
            return SelectedForum.Status.Equals("Open") && !string.IsNullOrEmpty(NewComment);
        }
        #endregion
    }
}
