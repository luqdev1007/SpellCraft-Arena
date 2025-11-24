using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class EndOfBattlePresenter : PopupPresenterBase
    {
        private readonly EndOfBattleView _view;
        private readonly WalletService _wallet;
        private readonly GameStatsService _statsService;
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameResultService _gameResultService;
        private readonly TypeModeHandler _typeModeHandler;

        protected override PopupViewBase PopupView => _view;

        public EndOfBattlePresenter(
            EndOfBattleView view,
            WalletService wallet,
            GameStatsService statsService,
            SceneSwitcherService sceneSwitcher,
            ICoroutinesPerformer coroutinesPerformer,
            GameResultService gameResultService,
            TypeModeHandler typeModeHandler) : base(coroutinesPerformer)
        {
            _view = view;
            _wallet = wallet;
            _statsService = statsService;
            _sceneSwitcher = sceneSwitcher;
            _coroutinesPerformer = coroutinesPerformer;
            _gameResultService = gameResultService;
            _typeModeHandler = typeModeHandler;
        }

        public override void Initialize()
        {
            base.Initialize();

            _view.ExitButtonClicked += OnExitButtonClicked;
            _view.NextButtonClicked += OnNextButtonClicked;
            _view.RestartButtonClicked += OnRestartButtonClicked;

            _gameResultService.VictoryRegistred += OnVictory;
            _gameResultService.DefeatRegistred += OnDefeat;

            Hide();
        }

        public override void Dispose()
        {
            base.Dispose();

            _view.ExitButtonClicked -= OnExitButtonClicked;
            _view.NextButtonClicked -= OnNextButtonClicked;
            _view.RestartButtonClicked -= OnRestartButtonClicked;

            // не будет, когда открывать буду буду передавать сразу параметром победу или поражение показать
            // или вытаскивать из gameResultService результат
            _gameResultService.VictoryRegistred -= OnVictory;
            _gameResultService.DefeatRegistred -= OnDefeat;
        }

        private void OnDefeat()
        {
            UpdateView("Defeat...");
            ActivateRestartButton();
        }

        private void OnVictory()
        {
            UpdateView("Victory!");
            ActivateNextButton();
        }

        private void UpdateView(string header)
        {
            _view.SetHeaderText(header);
            _view.SetGoldText(_wallet.GetCurrency(CurrencyTypes.Gold).Value.ToString());
            _view.SetLosesText(_statsService.Losses.Value.ToString());
            _view.SetWinsText(_statsService.Wins.Value.ToString());

            Show();
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