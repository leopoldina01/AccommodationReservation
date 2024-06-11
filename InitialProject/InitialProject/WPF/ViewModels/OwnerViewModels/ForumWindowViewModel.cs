using InitialProject.Application.UseCases;
using InitialProject.Commands;
using InitialProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InitialProject.WPF.ViewModels.OwnerViewModels
{
    public class ForumWindowViewModel : ViewModelBase
    {
        #region PROPERTIES
        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

        private string _messageLabel;
        public string MessageLabel
        {
            get => _messageLabel;
            set
            {
                if (value != _messageLabel)
                {
                    _messageLabel = value;
                    OnPropertyChanged("MessageLabel");
                }
            }
        }

        private Comment _selectedComment;
        public Comment SelectedComment
        {
            get
            {
                return _selectedComment;
            }
            set
            {
                if (_selectedComment != value)
                {
                    _selectedComment = value;
                    OnPropertyChanged(nameof(SelectedComment));
                }
            }
        }

        public static ObservableCollection<Comment> Comments { get; set; }
        private Forum _selectedForum;
        private User _owner;

        private readonly CommentService _commentService;
        private readonly UserLocationService _userLocationService;
        private readonly ReportedCommentsService _reportedCommentsService;
        #endregion

        public ForumWindowViewModel(Forum selectedForum, User owner)
        {
            Comments = new ObservableCollection<Comment>();

            _selectedForum = selectedForum;
            _owner = owner;

            _commentService = new CommentService();
            _userLocationService = new UserLocationService();
            _reportedCommentsService = new ReportedCommentsService();

            LoadComments();
            LoadMessageLabel();

            SendCommentCommand = new RelayCommand(SendCommentCommand_Execute, SendCommentCommand_CanExecute);
            ReportCommentCommand = new RelayCommand(ReportCommentCommand_Execute, ReportCommentCommand_CanExecute);
        }

        private void LoadMessageLabel()
        {
            if (!_userLocationService.IsThisOwnersLocation(_owner.Id, _selectedForum.LocationId))
            {
                MessageLabel = "You can't leave comments on this forum!";
            }
            else if (_selectedForum.Status == "Closed")
            {
                MessageLabel = "This forum is closed";
            }
            else
            {
                MessageLabel = "";
            }

            
        }

        private void LoadComments()
        {
            Comments.Clear();

            foreach (Comment comment in _commentService.GetAllByForumId(_selectedForum.Id)) 
            {
                Comments.Add(comment);
            }
        }

        #region COMMANDS
        public RelayCommand SendCommentCommand { get; }
        public RelayCommand ReportCommentCommand { get; }

        public bool SendCommentCommand_CanExecute(object? parameter)
        {
            return true;
                   //_userLocationService.IsThisOwnersLocation(_owner.Id, _selectedForum.LocationId) && Comment != "" && Comment != null && _selectedForum.Status == "Open";
        }

        public void SendCommentCommand_Execute(object? parameter)
        {
            MessageBox.Show(Comment);
            Comment ownersComment = _commentService.Save(_selectedForum.Id, Comment, _owner.Id);
            ownersComment.User = _owner;
            Comments.Add(ownersComment);
            Comment = "";
        }

        public void ReportCommentCommand_Execute(object? parameter)
        {
            _commentService.ReportComment(SelectedComment);
            _reportedCommentsService.Save(SelectedComment.Id, _owner.Id);
            LoadComments();
        }

        public bool ReportCommentCommand_CanExecute(object? parameter)
        {
            return SelectedComment != null && !_userLocationService.WasUserOnThisLocation(SelectedComment.UserId, _selectedForum.LocationId, _owner.Id)
                    && _reportedCommentsService.GetByOwnerId(_owner.Id, SelectedComment.Id) == null;
        }
        #endregion

    }
}
