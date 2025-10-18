using Assets._Project.Develop.Infrastructure;
using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataManagment;
using Assets._Project.Develop.Runtime.Utilites.DataManagment.Serializers;
using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using System.Collections.Generic;
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
        }
    }
}
