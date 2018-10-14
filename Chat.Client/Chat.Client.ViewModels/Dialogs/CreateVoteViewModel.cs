using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using Chat.Client.Framework;
using Chat.Shared.Models;

namespace Chat.Client.ViewModels.Dialogs
{
    public class CreateVoteViewModel : ViewModelBase
    {
        private readonly string _username;

        private string _question;
        public string Question
        {
            get { return _question; }
            set { _question = value; OnPropertyChanged(); CreateVoteCommand.RaiseCanExecuteChanged(); }
        }

        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set { _answer = value; OnPropertyChanged(); AddAnswerCommand.RaiseCanExecuteChanged(); }
        }

        public ObservableCollection<string> Answers { get; set; } = new ObservableCollection<string>(); 

        public RelayCommand AddAnswerCommand { get; }
        public RelayCommand CreateVoteCommand { get; }

        public event EventHandler<SimpleVote> VoteCreated;

        public CreateVoteViewModel(string username)
        {
            _username = username;
            CreateVoteCommand = new RelayCommand(CreateVote, CanCreateVote);
            AddAnswerCommand = new RelayCommand(AddAnswer, CanAddAnswer);
        }

        private bool CanAddAnswer()
        {
            return !string.IsNullOrWhiteSpace(_answer);
        }

        private void AddAnswer()
        {
            Answers.Add(_answer);
        }

        private bool CanCreateVote()
        {
            return !string.IsNullOrWhiteSpace(_question) &&
                   Answers.Any();
        }

        private void CreateVote()
        {
            VoteCreated?.Invoke(this, new SimpleVote(_question, _username, Answers.ToArray()));
        }
    }
}
