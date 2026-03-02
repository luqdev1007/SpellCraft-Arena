using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Minigames;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _view;
        private readonly MainMenuPopupService _popupService;
        private readonly WalletService _wallet;
        private readonly GameStatsService _statsService;
        private readonly ResetWinLoseStatsService _resetDataService;

        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        private List<IDisposable> _disposables = new();

        public MainMenuScreenPresenter(
            MainMenuScreenView view,
            MainMenuPopupService popupService,
            WalletService wallet,
            GameStatsService statsService,
            ResetWinLoseStatsService resetDataService,
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer)
        {
            _view = view;
            _popupService = popupService;
            _wallet = wallet;
            _statsService = statsService;
            _resetDataService = resetDataService;
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public void Initialize()
        {
            _view.SetGoldText(_wallet.GetCurrency(CurrencyTypes.Gold).Value.ToString());
            _view.SetLosesText(_statsService.Losses.Value.ToString());
            _view.SetWinsText(_statsService.Wins.Value.ToString());

            _disposables.Add(_wallet.GetCurrency(CurrencyTypes.Gold).Subscribe(OnGoldChanged));
            _disposables.Add(_statsService.Wins.Subscribe(OnWinsChanged));
            _disposables.Add(_statsService.Losses.Subscribe(OnLossesChanged));

            _view.StartGameButtonClicked += OnStartGameButtonClicked;
            _view.ResetStatsButtonClicked += OnResetStatsButtonClicked;
            _view.OpenChatButtonClicked += OnOpenChatButtonClicked;

            CheckResetPossibility();
        }

        public void Dispose()
        {
            _view.StartGameButtonClicked -= OnStartGameButtonClicked;
            _view.ResetStatsButtonClicked -= OnResetStatsButtonClicked;
            _view.OpenChatButtonClicked -= OnOpenChatButtonClicked;

            foreach (var disposable in _disposables)
                disposable.Dispose();

            _disposables.Clear();
        }

        private void OnLossesChanged(int oldValue, int newValue)
        {
            _view.SetLosesText(newValue.ToString());
        }

        private void OnWinsChanged(int oldValue, int newValue)
        {
            _view.SetWinsText(newValue.ToString());
        }

        private void OnOpenChatButtonClicked()
        {
            _popupService.OpenChatPopup();
        }

        private void OnStartGameButtonClicked()
        {
            _popupService.OpenLevelsMenuPopup();
        }

        private void OnResetStatsButtonClicked()
        {
            _popupService.OpenConfirmPopup(ResetStats, $"Вы уверены, что хотите сбросить всю статистику?\n" +
                $"Это будет стоить Вам {_resetDataService.ResetCost} {_resetDataService.ResetCurrency}");
        }

        private void ResetStats()
        {
            _resetDataService.TryResetData();
        }

        private void OnGoldChanged(int arg1, int newValue)
        {
            CheckResetPossibility();
            _view.SetGoldText(newValue.ToString());
        }

        private void CheckResetPossibility()
        {
            if (_resetDataService.CanReset)
            {
                _view.EnableResetButton();
            }
            else
            {
                _view.DisableResetButton();
            }
        }
    }
}
