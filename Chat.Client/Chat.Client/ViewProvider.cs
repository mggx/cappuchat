using Chat.Client.Dialogs;
using Chat.Client.Framework;
using Chat.Client.ViewModels;
using Chat.Client.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Chat.Client.Presenters;
using Chat.Client.Windows;
using MahApps.Metro.Controls;

namespace Chat.Client
{
    public class ViewProvider : IViewProvider
    {
        private readonly Dictionary<IDialog, Window> _windowCache = new Dictionary<IDialog, Window>();

        public void Show(IDialog dialog)
        {
            if (dialog == null)
                throw new ArgumentNullException(nameof(dialog), "Cannot show. Given dialog is null.");

            if (_windowCache.ContainsKey(dialog))
            {
                _windowCache[dialog].Show();
                return;
            }

            Window window = GetWindowByDialog(dialog);

            window.DataContext = dialog;
            window.Show();
        }

        public void ShowDialog(IModalDialog modalDialog)
        {
            if (modalDialog == null)
                throw new ArgumentNullException(nameof(modalDialog), "Cannot showDialog. Given modalDialog is null.");

            Application.Current.Dispatcher.Invoke(() =>
            {
                Window window = GetWindowByDialog(modalDialog);
                window.ShowDialog();
            });
        }

        private Window GetWindowByDialog(IDialog dialog)
        {
            Window window = null;

            switch (dialog)
            {
                case OkCancelDialogViewModel _:
                    window = new OkCancelDialogWindow();
                    break;
                case CappuMainPresenter _:
                    window = new MainWindow();
                    break;
                case CappuChatViewModel _:
                    window = new PrivateMessageWindow();
                    break;
            }

            if (window == null)
                throw new InvalidOperationException($"Couldnt find window for dialog {dialog}");

            _windowCache.Add(dialog, window);
            window.Closed += WindowOnClosed;

            Window ownerWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (ownerWindow != null)
            {
                window.Owner = ownerWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            window.DataContext = dialog;

            return window;
        }

        private void MainWindowOnClosed(object sender, EventArgs e)
        {
        }

        private void WindowOnClosed(object sender, EventArgs e)
        {
            Window window = sender as Window;
            if (window == null)
                return;

            window.Closed -= WindowOnClosed;

            IDialog windowDataContext = window.DataContext as IDialog;
            if (windowDataContext == null)
                return;

            _windowCache.Remove(windowDataContext);

            if (_windowCache.Count == 0)
                Application.Current.Shutdown();
        }

        public void Hide(IDialog dialog)
        {
            if (!_windowCache.ContainsKey(dialog))
                return;
            _windowCache[dialog].Hide();
        }

        public void Close(IDialog dialog)
        {
            if (!_windowCache.ContainsKey(dialog))
                return;

            _windowCache[dialog].Close();
            _windowCache.Remove(dialog);
        }

        public void ShowMessage(string title, string message)
        {
            Application.Current.Invoke(() =>
            {
                OkCancelDialogViewModel viewModel = new OkCancelDialogViewModel(title, message);
                ShowDialog(viewModel);
            });
        }

        public void Focus(IDialog dialog)
        {
            if (_windowCache.ContainsKey(dialog))
            {
                _windowCache[dialog].WindowState = WindowState.Normal;
                _windowCache[dialog].Focus();
            }
        }
    }
}
