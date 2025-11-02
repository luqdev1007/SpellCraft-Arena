using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuView _view;
        private readonly ProjectPresentersFactory _projectPresentersFactory;
        private readonly MainMenuPopupService _popupService;
        private readonly WalletService _wallet;
        private readonly GameStatsService _statsService;

        public MainMenuScreenPresenter(
            MainMenuView view,
            ProjectPresentersFactory projectPresentersFactory,
            MainMenuPopupService popupService,
            WalletService wallet,
            GameStatsService statsService)
        {
            _view = view;
            _projectPresentersFactory = projectPresentersFactory;
            _popupService = popupService;
            _wallet = wallet;
            _statsService = statsService;

            _view.SetGoldText(_wallet.GetCurrency(CurrencyTypes.Gold).Value.ToString());
            _view.SetLosesText(_statsService.Losses.ToString());
            _view.SetWinsText(_statsService.Wins.ToString());
        }

        public void Initialize()
        {
            _view.StartGameButtonClicked += OnStartGameButtonClicked;
            _view.ResetStatsButtonClicked += OnResetStatsButtonClicked;
        }

        public void Dispose()
        {
            _view.StartGameButtonClicked -= OnStartGameButtonClicked;
            _view.ResetStatsButtonClicked -= OnResetStatsButtonClicked;
        }

        private void OnStartGameButtonClicked()
        {
            Debug.Log("Start");
        }

        private void OnResetStatsButtonClicked()
        {
            Debug.Log("Reset");
        }
    }
}
