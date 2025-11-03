using Assets._Project.Develop.Runtime.Configs.Meta.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuView _view;
        private readonly MainMenuPopupService _popupService;
        private readonly WalletService _wallet;
        private readonly GameStatsService _statsService;

        private List<IDisposable> _disposables = new();
        private int _resetCost;

        public MainMenuScreenPresenter(
            MainMenuView view,
            MainMenuPopupService popupService,
            WalletService wallet,
            GameStatsService statsService,
            ConfigsProviderService configsProvider)
        {
            _view = view;
            _popupService = popupService;
            _wallet = wallet;
            _statsService = statsService;
            _resetCost = configsProvider.GetConfig<GameRewardsConfig>().ResetCost;
        }

        private void OnGoldChanged(int arg1, int newValue)
        {
            CheckResetPossibility(newValue);
            _view.SetGoldText(newValue.ToString());
        }

        private void CheckResetPossibility(int newValue)
        {
            if (newValue >= _resetCost)
            {
                _view.EnableResetButton();
            }
            else
            {
                _view.DisableResetButton();
            }
        }

        public void Initialize()
        {
            CheckResetPossibility(_wallet.GetCurrency(CurrencyTypes.Gold).Value);

            _view.SetGoldText(_wallet.GetCurrency(CurrencyTypes.Gold).Value.ToString());
            _view.SetLosesText(_statsService.Losses.Value.ToString());
            _view.SetWinsText(_statsService.Wins.Value.ToString());

            _disposables.Add(_wallet.GetCurrency(CurrencyTypes.Gold).Subscribe(OnGoldChanged));
            _disposables.Add(_statsService.Wins.Subscribe(OnWinsChanged));
            _disposables.Add(_statsService.Losses.Subscribe(OnLossesChanged));

            _view.StartGameButtonClicked += OnStartGameButtonClicked;
            _view.ResetStatsButtonClicked += OnResetStatsButtonClicked;
        }

        private void OnLossesChanged(int oldValue, int newValue)
        {
            _view.SetLosesText(newValue.ToString());
        }

        private void OnWinsChanged(int oldValue, int newValue)
        {
            _view.SetWinsText(newValue.ToString());
        }

        public void Dispose()
        {
            _view.StartGameButtonClicked -= OnStartGameButtonClicked;
            _view.ResetStatsButtonClicked -= OnResetStatsButtonClicked;

            foreach (var disposable in _disposables)
                disposable.Dispose();

            _disposables.Clear();
        }

        private void OnStartGameButtonClicked()
        {
            _popupService.OpenLevelsMenuPopup();
        }

        private void OnResetStatsButtonClicked()
        {
            _popupService.OpenConfirmPopup(ResetStats, "Вы уверены, что хотите сбросить всю статистику?");
        }

        private void ResetStats()
        {
            Debug.Log("stats reseted");
            _statsService.ResetStats();
            _wallet.Spend(CurrencyTypes.Gold, _resetCost);
        }
    }
}
