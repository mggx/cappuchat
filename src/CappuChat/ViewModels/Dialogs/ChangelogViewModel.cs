using System;
using Chat.Client.Framework;

namespace Chat.Client.ViewModels.Dialogs
{
    public class ChangelogViewModel : ViewModelBase, IModalDialog
    {
        private string _changelog;
        public string Changelog
        {
            get { return _changelog; }
            set { _changelog = value; OnPropertyChanged(); }
        }

        public RelayCommand OkCommand { get; set; }

        public ModalResult ModalResult { get; set; }

        public event EventHandler ChangelogClose;

        public ChangelogViewModel(string changelog)
        {
            if (string.IsNullOrWhiteSpace(changelog))
                throw new ArgumentNullException(CappuChat.Properties.Errors.EmptyChangelogIsANoNo);
            Changelog = changelog;

            OkCommand = new RelayCommand(Ok);
        }

        private void Ok()
        {
            ChangelogClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
