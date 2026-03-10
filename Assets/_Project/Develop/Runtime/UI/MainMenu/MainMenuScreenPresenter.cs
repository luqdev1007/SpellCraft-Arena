using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenPresenter : IPresenter
    {
        private readonly MainMenuScreenView _view;
        private readonly MainMenuPopupService _popupService;
        private readonly WalletService _wallet;
        private readonly GameStatsService _statsService;

        private List<IDisposable> _disposables = new();

        public MainMenuScreenPresenter(
            MainMenuScreenView view,
            MainMenuPopupService popupService,
            WalletService wallet,
            GameStatsService statsService)
        {
            _view = view;
            _popupService = popupService;
            _wallet = wallet;
            _statsService = statsService;
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
        }

        public void Dispose()
        {
            _view.StartGameButtonClicked -= OnStartGameButtonClicked;

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

        private void OnStartGameButtonClicked()
        {
            _popupService.OpenLevelsMenuPopup();
        }

        private void OnGoldChanged(int arg1, int newValue)
        {
            _view.SetGoldText(newValue.ToString());
        }
    }
}
