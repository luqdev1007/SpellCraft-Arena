using Assets._Project.Develop.Infrastructure;
using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Configs.Meta.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private GameModeSelectionService _gameModeSelectionService;

        private WalletService _walletService;
        private GameStatsService _gameStatsService;
        private GameRewardsConfig _gameRewardsConfig;

        private ICoroutinesPerformer _coroutinesPerformer;
        private PlayerDataProvider _playerDataProvider;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Main menu scene init");

            _gameModeSelectionService = _container.Resolve<GameModeSelectionService>();

            _walletService = _container.Resolve<WalletService>();

            _gameStatsService = _container.Resolve<GameStatsService>();

            _gameRewardsConfig = _container.Resolve<ConfigsProviderService>().GetConfig<GameRewardsConfig>();

            _playerDataProvider = _container.Resolve<PlayerDataProvider>();
            _coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            // Wallet presenter?

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Start main menu scene");
            Debug.Log("A - add 10 gold & diamonds, S - spend 10 gold, F2 - save progress, 1 - digits game mode, 2 - letters game mode");
            Debug.Log("3 - show stats, 4 - reset stats");
        }

        private void Update()
        {
            _gameModeSelectionService?.Update();

            if (Input.GetKeyDown(KeyCode.A))
            {
                _walletService.Add(CurrencyTypes.Gold, 10);
                _walletService.Add(CurrencyTypes.Diamond, 10);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (_walletService.IsEnough(CurrencyTypes.Gold, 10) == false)
                    return;

                _walletService.Spend(CurrencyTypes.Gold, 10);
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
                Debug.Log("Data is saved");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log($"Wins: {_gameStatsService.Wins}, Losses: {_gameStatsService.Losses}");
            }


            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (_walletService.IsEnough(CurrencyTypes.Gold, _gameRewardsConfig.ResetCost))
                {
                    _walletService.Spend(CurrencyTypes.Gold, _gameRewardsConfig.ResetCost);
                    _gameStatsService.Reset();
                    Debug.Log("Progress reset!");
                }
                else
                {
                    Debug.Log("Not enough gold to reset progress!");
                }
            }
        }
    }
}
