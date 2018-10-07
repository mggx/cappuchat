using System;
using System.Threading.Tasks;
using Chat.Client.Framework;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.ViewModels;
using Chat.Shared.Models;

namespace Chat.Client.Presenters
{
    public class CappuVotePresenter : ViewModelBase
    {
        private readonly ISignalHelperFacade _signalHelperFacade;
        private readonly IViewProvider _viewProvider;
        private SimpleUser _user;

        public CappuVoteViewModel CappuVoteViewModel { get; private set; }

        public CappuVotePresenter(ISignalHelperFacade signalHelperFacade, IViewProvider viewProvider)
        {
            if (signalHelperFacade == null)
                throw new ArgumentNullException(nameof(signalHelperFacade), "Cannot create CappuVotePresenter. Given signalHelperFacade is null.");
            _signalHelperFacade = signalHelperFacade;

            if (viewProvider == null)
                throw new ArgumentNullException(nameof(viewProvider), "Cannot create CappuVotePresenter. Given viewProvider is null.");
            _viewProvider = viewProvider;

            Initialize();
        }

        private void Initialize()
        {
            InitializeCappuVoteViewModel();
        }

        private void InitializeCappuVoteViewModel()
        {
            CappuVoteViewModel = new CappuVoteViewModel(_signalHelperFacade, _viewProvider);
        }

        public async Task Load(SimpleUser user)
        {
            _user = user;
            await CappuVoteViewModel.Load(user);
        }
    }
}
