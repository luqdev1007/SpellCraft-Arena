using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class EndOfBattlePresenter : IPresenter
    {
        private readonly EndOfBattleView _view;
        private readonly WalletService _wallet;
        private readonly GameStatsService _statsService;
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameResultService _gameResultService;
        private readonly TypeModeHandler _typeModeHandler;

        public EndOfBattlePresenter(
            EndOfBattleView view,
            WalletService wallet,
            GameStatsService statsService,
            SceneSwitcherService sceneSwitcher,
            ICoroutinesPerformer coroutinesPerformer,
            GameResultService gameResultService,
            TypeModeHandler typeModeHandler)
        {
            _view = view;
            _wallet = wallet;
            _statsService = statsService;
            _sceneSwitcher = sceneSwitcher;
            _coroutinesPerformer = coroutinesPerformer;
            _gameResultService = gameResultService;
            _typeModeHandler = typeModeHandler;
        }

        public void Initialize()
        {
            _view.ExitButtonClicked += OnExitButtonClicked;
            _view.NextButtonClicked += OnNextButtonClicked;
            _view.RestartButtonClicked += OnRestartButtonClicked;

            _gameResultService.VictoryRegistred += OnVictory;
            _gameResultService.DefeatRegistred += OnDefeat;

            Hide();
        }

        public void Dispose()
        {
            _view.ExitButtonClicked -= OnExitButtonClicked;
            _view.NextButtonClicked -= OnNextButtonClicked;
            _view.RestartButtonClicked -= OnRestartButtonClicked;

            _gameResultService.VictoryRegistred -= OnVictory;
            _gameResultService.DefeatRegistred -= OnDefeat;
        }

        private void OnDefeat()
        {
            Show("Defeat...");
            ActivateRestartButton();
        }

        private void OnVictory()
        {
            Show("Victory!");
            ActivateNextButton();
        }

        public void Show(string headerText)
        {
            _view.SetGoldText(_wallet.GetCurrency(CurrencyTypes.Gold).Value.ToString());
            _view.SetLosesText(_statsService.Losses.Value.ToString());
            _view.SetWinsText(_statsService.Wins.Value.ToString());
            _view.SetHeaderText(headerText);

            _view.Show();
        }

        public void Hide()
        {
            _view.Hide();
        }

        public void ActivateNextButton()
        {
            _view.NextButton.gameObject.SetActive(true);
            _view.RestartButton.gameObject.SetActive(false);
        }

        public void ActivateRestartButton()
        {
            _view.RestartButton.gameObject.SetActive(true);
            _view.NextButton.gameObject.SetActive(false);
        }

        private void OnRestartButtonClicked()
        {
            Hide();
            _typeModeHandler.StartGame();
        }

        private void OnNextButtonClicked()
        {
            _coroutinesPerformer.StartPerform(_sceneSwitcher.ProcessingSwitchTo(Scenes.MainMenu)); 
        }

        private void OnExitButtonClicked()
        {
            _coroutinesPerformer.StartPerform(_sceneSwitcher.ProcessingSwitchTo(Scenes.MainMenu));
        }
    }
}