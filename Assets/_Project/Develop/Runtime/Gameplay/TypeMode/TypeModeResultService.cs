using Assets._Project.Develop.Runtime.Configs.Meta.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using UnityEngine;


namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeResultService
    {
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameStatsService _gameStatsService;
        private readonly PlayerDataProvider _playerDataProvider;
        private readonly WalletService _walletService;
        private readonly ConfigsProviderService _configsProviderService;

        public TypeModeResultService(SceneSwitcherService sceneSwitcher,
            ICoroutinesPerformer coroutinesPerformer,
            GameStatsService statsService,
            PlayerDataProvider playerDataProvider,
            WalletService walletService,
            ConfigsProviderService configsProviderService)
        {
            _sceneSwitcherService = sceneSwitcher;
            _coroutinesPerformer = coroutinesPerformer;
            _gameStatsService = statsService;
            _playerDataProvider = playerDataProvider;
            _walletService = walletService;
            _configsProviderService = configsProviderService;
        }

        public void HandleResult(bool isVictory)
        {
            Debug.Log(isVictory ? "Victory!" : "Defeat!");
            Debug.Log("Press 'Space' to continue");
        }

        public IEnumerator ContinueAfterVictory()
        {
            _gameStatsService.RegisterVictory();
            _walletService.Add(CurrencyTypes.Gold, _configsProviderService.GetConfig<GameRewardsConfig>().RewardForWin);

            yield return _coroutinesPerformer.StartPerform(_playerDataProvider.Save());

            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessingSwitchTo(Scenes.MainMenu));
        }

        public IEnumerator ContinueAfterLose(GameplayInputArgs inputArgs)
        {
            _gameStatsService.RegisterDefeat();

            if (_walletService.IsEnough(CurrencyTypes.Gold, _configsProviderService.GetConfig<GameRewardsConfig>().PenaltyForLose))
                _walletService.Spend(CurrencyTypes.Gold, _configsProviderService.GetConfig<GameRewardsConfig>().PenaltyForLose);
            else
                _walletService.Spend(CurrencyTypes.Gold, _walletService.GetCurrency(CurrencyTypes.Gold).Value);

            yield return _coroutinesPerformer.StartPerform(_playerDataProvider.Save());

            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessingSwitchTo(Scenes.Gameplay, inputArgs));
        }
    }
}
