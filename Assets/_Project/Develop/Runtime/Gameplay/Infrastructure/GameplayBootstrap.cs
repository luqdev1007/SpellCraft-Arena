using Assets._Project.Develop.Infrastructure;
using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Tests;
using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private GameplayInputArgs _inputArgs;
        private TypeModeHandler _typeModeHandler;

        [SerializeField] private TestGameplay _testGameplay; // delete later

        private EntitiesLifeContext _entitiesLifeContext;

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

            _typeModeHandler = _container.Resolve<TypeModeHandler>();
            _typeModeHandler.Init();

            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
            _testGameplay.Initialize(_container);

            yield break;
        }

        public override void Run()
        {
            _typeModeHandler.StartGame();
            Debug.Log($"Start gameplay scene");

            _testGameplay.Run();
        }

        private void Update()
        {
            _entitiesLifeContext?.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _typeModeHandler.Dispose();
        }
    }
}
