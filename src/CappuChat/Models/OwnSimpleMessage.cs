using CappuChat;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Chat.Models
{
    [Serializable]
    public class OwnSimpleMessage : SimpleMessage, INotifyPropertyChanged
    {
        [JsonIgnore]
        public Stream ImageStream { get; set; }

        private bool _imageUploading;
        public bool ImageUploading
        {
            get { return _imageUploading; }
            set { _imageUploading = value; OnPropertyChanged(); }
        }

        public OwnSimpleMessage()
        {
        }

        public OwnSimpleMessage(SimpleMessage message) : base(message.Sender, message.Receiver, message.Message)
        {
            MessageSentDateTime = message.MessageSentDateTime;
            Base64ImageString = message.Base64ImageString;
        }

        public OwnSimpleMessage(SimpleUser sender, string message) : base(sender, message)
        {
        }

        public OwnSimpleMessage(SimpleUser sender, SimpleUser receiver, string message) : base(sender, receiver, message)
        {
        }

        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
