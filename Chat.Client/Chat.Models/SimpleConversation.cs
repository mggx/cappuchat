using System.ComponentModel;
using System.Runtime.CompilerServices;
using Chat.Models.Annotations;

namespace Chat.Models
{
    public class SimpleConversation : INotifyPropertyChanged
    {
        private int _newMessages;
        public int NewMessages
        {
            get { return _newMessages; }
            set { _newMessages = value; OnPropertyChanged(); }
        }

        private string _lastMessage;
        public string LastMessage
        {
            get { return _lastMessage; }
            set { _lastMessage = value; OnPropertyChanged(); }
        }

        public int Id { get; set; }
        public string TargetUsername { get; }

        public SimpleConversation(string targetUsername)
        {
            TargetUsername = targetUsername;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
