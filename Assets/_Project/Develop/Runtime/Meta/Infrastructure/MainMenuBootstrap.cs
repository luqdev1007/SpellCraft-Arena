using Assets._Project.Develop.Infrastructure;
using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataManagment;
using Assets._Project.Develop.Runtime.Utilites.DataManagment.Serializers;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;

        private WalletService _walletService;

        private PlayerData _playerData;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Main menu scene init");

            _walletService = _container.Resolve<WalletService>();

            _playerData = new PlayerData();
            _playerData.WalletData = new Dictionary<CurrencyTypes, int>()
            {
                {CurrencyTypes.Gold, 10 },
                {CurrencyTypes.Diamond, 150 },
            };

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Start main menu scene");
        }

        // delete later
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessingSwitchTo(Scenes.Gameplay, new GameplayInputArgs(2)));
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _walletService.Add(CurrencyTypes.Gold, 10);
                Debug.Log($"{CurrencyTypes.Gold} left: {_walletService.GetCurrency(CurrencyTypes.Gold).Value}");
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (_walletService.IsEnough(CurrencyTypes.Gold, 10) == false)
                    return;

                _walletService.Spend(CurrencyTypes.Gold, 10);
                Debug.Log($"{CurrencyTypes.Gold} left: {_walletService.GetCurrency(CurrencyTypes.Gold).Value}");
            }
        }
    }
}
