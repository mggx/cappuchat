using System;
using Chat.Client.Framework;

namespace Chat.Client.ViewModels.Dialogs
{
    public class OkCancelDialogViewModel : ViewModelBase, IModalDialog
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        public ModalResult ModalResult { get; set; } = ModalResult.Closed;

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public OkCancelDialogViewModel(string title, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message), "Cannot create OkCancelDialogViewModel. Given message is invalid.");
            Message = message;

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException(nameof(title), "Cannot create OkCancelDialogViewModel. Given title is invalid.");
            Title = title;

            OkCommand = new RelayCommand(Ok);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Ok()
        {
            ModalResult = ModalResult.Ok;
        }

        private void Cancel()
        {
            ModalResult = ModalResult.Aborted;
        }
    }
}
