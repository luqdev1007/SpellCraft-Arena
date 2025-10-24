using Assets._Project.Develop.Infrastructure;
using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap
    {
        private WalletService _walletService;

        private DIContainer _container;
        private GameplayInputArgs _inputArgs;
        private TypeModeHandler _typeModeHandler;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)} type");

            _inputArgs = gameplayInputArgs;

            GameplayContextRegistrations.Process(_container, _inputArgs);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Gameplay scene init");

            _walletService = _container.Resolve<WalletService>();

            _typeModeHandler = _container.Resolve<TypeModeHandler>();
            _typeModeHandler.StartGame();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Start gameplay scene");
        }

        private void Update()
        {
            _typeModeHandler?.Update();

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
        }
    }
}
