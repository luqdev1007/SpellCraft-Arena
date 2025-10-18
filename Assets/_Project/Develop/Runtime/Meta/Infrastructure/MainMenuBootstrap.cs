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

            _playerDataProvider = _container.Resolve<PlayerDataProvider>();
            _coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Start main menu scene");
            Debug.Log("A - add 10 gold, S - spend 10 gold, F2 - save progress, 1 - digits game mode, 2 - letters game mode");
            Debug.Log("3 - show stats, 4 - show gold, 5 - reset stats");
        }

        private void Update()
        {
            _gameModeSelectionService?.Update();

            if (Input.GetKeyDown(KeyCode.A))
            {
                _walletService.Add(CurrencyTypes.Gold, 10);
                Debug.Log($"{CurrencyTypes.Gold} left: {_walletService.GetCurrency(CurrencyTypes.Gold).Value}");
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (_walletService.IsEnough(CurrencyTypes.Gold, 10) == false)
                    return;

                _walletService.Spend(CurrencyTypes.Gold, 10);
                Debug.Log($"{CurrencyTypes.Gold} left: {_walletService.GetCurrency(CurrencyTypes.Gold).Value}");
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
                Debug.Log("Data is saved");
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                var stats = _container.Resolve<GameStatsService>();
                Debug.Log($"Wins: {stats.GetWins()}, Losses: {stats.GetLosses()}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                var wallet = _container.Resolve<WalletService>();
                Debug.Log($"Gold: {wallet.GetCurrency(CurrencyTypes.Gold).Value}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                WalletService wallet = _container.Resolve<WalletService>();
                GameRewardsConfig rewardsConfig = _container.Resolve<ConfigsProviderService>().GetConfig<GameRewardsConfig>();

                if (wallet.IsEnough(CurrencyTypes.Gold, rewardsConfig.ResetCost))
                {
                    wallet.Spend(CurrencyTypes.Gold, rewardsConfig.ResetCost);
                    PlayerDataProvider playerDataProvider = _container.Resolve<PlayerDataProvider>();
                    playerDataProvider.CurrentData.Wins = 0;
                    playerDataProvider.CurrentData.Losses = 0;
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
