using System;
using System.Collections.Generic;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.ViewModels;
using Chat.Shared.Models;

namespace Chat.Client.Presenters
{
    public class CappuChatPresenter : ViewModelBase, IDialog
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;

        private readonly Dictionary<string, CappuChatViewModel> _privateMessageCache = new Dictionary<string, CappuChatViewModel>();

        private SimpleUser User { get; set; }

        private CappuChatViewModel _currentChatViewModel;
        public CappuChatViewModel CurrentChatViewModel
        {
            get { return _currentChatViewModel; }
            set { _currentChatViewModel = value; OnPropertyChanged(); }
        }

        public RelayCommand<SimpleUser> OpenChatCommand { get; }

        public CappuChatPresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuChatPresenter. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider), "Cannot create CappuChatPresenter. Given viewProvider is null.");
            _viewProvider = viewProvider;

            OpenChatCommand = new RelayCommand<SimpleUser>(OpenChat);

            Initialize();
        }

        private void OpenChat(SimpleUser user)
        {
            if (user.Username == User.Username)
                return;
            CurrentChatViewModel = new CappuChatViewModel(_signalHelperFacade, User, user);
        }

        private void Initialize()
        {
        }

        public void Load(SimpleUser user)
        {
            User = user;
        }

        public void Reset()
        {
            User = null; 

            foreach (var pair in _privateMessageCache)
            {
                pair.Value.Dispose();
            }

            _privateMessageCache.Clear();
        }
    }
}
