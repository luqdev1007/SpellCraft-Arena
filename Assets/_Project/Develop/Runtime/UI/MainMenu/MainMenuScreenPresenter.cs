using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private MainMenuScreenView _screen;

        private ProjectPresentersFactory _projectPresentersFactory;

        private readonly MainMenuPopupService _popupService;

        private readonly List<IPresenter> _childPresenters = new();

        public MainMenuScreenPresenter(
            MainMenuScreenView screen,
            ProjectPresentersFactory projectPresentersFactory,
            MainMenuPopupService popupService)
        {
            _screen = screen;
            _projectPresentersFactory = projectPresentersFactory;
            _popupService = popupService;
        }

        public void Initialize()
        {
            _screen.OpenTestPopupButtonClicked += OnOpenTestPopupButtonClicked;
            _screen.OpenLevelsMenuButtonClicked += OnOpenLevelsMenuButtonClicked;

            CreateWallet();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void Dispose()
        {
            _screen.OpenTestPopupButtonClicked -= OnOpenTestPopupButtonClicked;
            _screen.OpenLevelsMenuButtonClicked -= OnOpenLevelsMenuButtonClicked;

            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        private void OnOpenTestPopupButtonClicked()
        {
            _popupService.OpenTestPopup(() => Debug.Log("Pop up closed"));
        }

        private void OnOpenLevelsMenuButtonClicked()
        {
            _popupService.OpenLevelsMenuPopup();
        }

        private void CreateWallet()
        {
            WalletPresenter walletPresenter = _projectPresentersFactory.CreateWalletPresenter(_screen.WalletView);

            _childPresenters.Add(walletPresenter);
        }
    }
}
