﻿using CappuChat;
using CappuChat.Configuration;
using Chat.Client.CustomNotifications.Extensions;
using Chat.Client.Dialogs;
using Chat.Client.Framework;
using Chat.Client.Presenters;
using Chat.Client.ViewModels.Dialogs;
using Chat.Models;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Chat.Client
{
    public sealed class ViewProvider : IViewProvider, IDisposable
    {
        private readonly Dictionary<IDialog, Window> _windowCache = new Dictionary<IDialog, Window>();
        private readonly Notifier _notifier;

        public ViewProvider()
        {
            _notifier = new Notifier(configuration =>
            {
                configuration.PositionProvider = new PrimaryScreenPositionProvider(
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);
                configuration.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                configuration.Dispatcher = Application.Current.Dispatcher;
            });
        }

        public void Show(IDialog dialog)
        {
            if (dialog == null)
                throw new ArgumentNullException(nameof(dialog));

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
                throw new ArgumentNullException(nameof(modalDialog));

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
                default:
                    throw new InvalidOperationException($"Couldnt find window for dialog {dialog}");
            }

            _windowCache.Add(dialog, window);
            window.Closed += WindowOnClosed;

            Window ownerWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            if (ownerWindow != null)
            {
                window.Owner = ownerWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            window.DataContext = dialog;

            return window;
        }

        private void WindowOnClosed(object sender, EventArgs e)
        {
            if (!(sender is Window window))
                return;

            window.Closed -= WindowOnClosed;

            if (!(window.DataContext is IDialog windowDataContext))
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

        public void ShowToastNotification(string message, NotificationType notificationType, bool force = false)
        {
            var configurationController = new ConfigurationController<NotificationConfiguration>();
            var notificationConfiguration = configurationController.ReadConfiguration(new NotificationConfiguration());

            if (!notificationConfiguration.ShowPushNotifications && !force)
                return;

            switch (notificationType)
            {
                case NotificationType.Information:
                    _notifier.ShowInformation(message);
                    break;
                case NotificationType.Success:
                    _notifier.ShowSuccess(message);
                    break;
                case NotificationType.Warning:
                    _notifier.ShowWarning(message);
                    break;
                case NotificationType.Error:
                    _notifier.ShowError(message);
                    break;
                case NotificationType.Dark:
                    _notifier.ShowDarkMessage(message, string.Empty);
                    break;
            }
        }

        public void ShowToastNotification(string message, string buttonContent, NotificationType notificationType, ICommand command = null)
        {
            var configurationController = new ConfigurationController<NotificationConfiguration>();
            var notificationConfiguration = configurationController.ReadConfiguration(new NotificationConfiguration());

            if (!notificationConfiguration.ShowPushNotifications)
                return;

            if (notificationType == NotificationType.Dark)
                _notifier.ShowDarkMessage(message, buttonContent, command);
        }

        public void BringToFront(IDialog dialog)
        {
            if (_windowCache.ContainsKey(dialog))
            {
                _windowCache[dialog].WindowState = WindowState.Normal;
                _windowCache[dialog].Focus();
            }
        }

        public void BringToFront()
        {
            var window = Application.Current.MainWindow;
            if (window == null)
                return;

            window.WindowState = WindowState.Normal;
            window.Focus();
        }

        public void FlashWindow(bool checkFocus = true)
        {
            var window = Application.Current.MainWindow;
            if (window == null)
                return;

            WindowInteropHelper wih = new WindowInteropHelper(window);
            if (!window.IsFocused)
                _ = NativeMethods.FlashWindow(wih.Handle, true);
        }

        public bool IsMainWindowFocused()
        {
            var window = Application.Current.MainWindow;
            if (window == null)
                return false;
            return window.IsFocused;
        }

        #region IDisposable Support
        private bool alreadyDisposed = false; // To detect redundant calls

        public void Dispose(bool disposing)
        {
            if (!alreadyDisposed)
            {
                if (disposing)
                {
                    _notifier.Dispose();
                }

                alreadyDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
