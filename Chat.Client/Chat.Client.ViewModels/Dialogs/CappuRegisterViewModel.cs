using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Shared.Models;
using System;
using Chat.Client.ViewModels.Providers;
using Microsoft.Win32;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Chat.Client.ViewModels.Dialogs
{
    public class CappuRegisterViewModel : ViewModelBase, IModalDialog<SimpleUser>
    {
        private readonly IRegisterSignalHelper _registerSignalHelper;

        private const string PROFILEPICTURESTOREPATH = @"Images\UserPictures\";

        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); RaiseCanExecuteChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); RaiseCanExecuteChanged(); }
        }

        private string _profilePicturePath;
        public string ProfilePicturePath
        {
            get => _profilePicturePath;
            set { _profilePicturePath = value; OnPropertyChanged(); }
        }

        public ModalResult ModalResult { get; set; } = ModalResult.Closed;

        public event EventHandler<string> RegisterFailed;
        public event Action<IDialog> RegisterCompleted;
        public event Action<IDialog> RegisterCanceled;


        public RelayCommand RegisterCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand SelectProfilePictureCommand { get; }

        public ProgressProvider ProgressProvider { get; }

        public CappuRegisterViewModel(IRegisterSignalHelper registerSignalHelper)
        {
            if (registerSignalHelper == null)
                throw new ArgumentNullException(nameof(registerSignalHelper), "Cannot create CappuRegisterViewModel. Given registerSignalHelper is null.");
            _registerSignalHelper = registerSignalHelper;

            ProfilePicturePath = @"/Chat.Client;component/Resources/user.png";

            RegisterCommand = new RelayCommand(Register, CanRegister);
            CancelCommand = new RelayCommand(Cancel);
            SelectProfilePictureCommand = new RelayCommand(SelectProfilePicture);

            ProgressProvider = new ProgressProvider();
        }

        private void SelectProfilePicture()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            openFileDialog.ShowDialog();
            //if(!File.Exists(PROFILEPICTURESTOREPATH + openFileDialog.SafeFileName))
            //    File.Copy(openFileDialog.FileName, PROFILEPICTURESTOREPATH + openFileDialog.SafeFileName);
            //ProfilePicturePath = openFileDialog.FileName;
            var uri = new Uri(openFileDialog.FileName);
            BitmapImage img = new BitmapImage(uri);

            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(img));
            using(MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
        }

        private void ConvertToByteArray()
        {

        }

        private bool CanRegister()
        {
            return !string.IsNullOrWhiteSpace(_username) &&
                   !string.IsNullOrWhiteSpace(_password);
        }

        private async void Register()
        {
            using (ProgressProvider.StartProgress())
            {
                try
                {
                    await _registerSignalHelper.Register(_username, _password);
                }
                catch (RequestFailedException e)
                {
                    RegisterFailed?.Invoke(this, e.Message);
                    return;
                }
            }

            RegisterCompleted?.Invoke(this);

            ModalResult = ModalResult.Ok;
        }

        private void Cancel()
        {
            RegisterCanceled?.Invoke(this);
        }

        private void RaiseCanExecuteChanged()
        {
            RegisterCommand.RaiseCanExecuteChanged();
        }

        public SimpleUser GetResult()
        {
            return new SimpleUser(_username);
        }
    }
}
