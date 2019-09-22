using Chat.Client.Framework;
using System;

namespace Chat.Client.ViewModels.Dialogs
{
    public class OkCancelDialogViewModel : ViewModelBase, IModalDialog
    {
        private string _title;
        public string Title {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        private string _message;
        public string Message {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }

        public ModalResult ModalResult { get; set; } = ModalResult.Closed;

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        public OkCancelDialogViewModel(string title, string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Title = title ?? throw new ArgumentNullException(nameof(title));

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
