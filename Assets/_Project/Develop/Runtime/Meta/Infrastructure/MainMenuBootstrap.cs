using Assets._Project.Develop.Infrastructure;
using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Main menu scene init");

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Start main menu scene");
        }

        // delete later
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
                TypeNumbersGameModeConfig gameMode = _container.Resolve<ConfigsProviderService>().GetConfig<TypeNumbersGameModeConfig>();

                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessingSwitchTo(Scenes.Gameplay, new GameplayInputArgs(gameMode)));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
                TypeCharsGameModeConfig gameMode = _container.Resolve<ConfigsProviderService>().GetConfig<TypeCharsGameModeConfig>();

                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessingSwitchTo(Scenes.Gameplay, new GameplayInputArgs(gameMode)));
            }
        }
    }
}
