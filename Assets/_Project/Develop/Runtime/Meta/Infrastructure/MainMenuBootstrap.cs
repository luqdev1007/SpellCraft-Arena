using Assets._Project.Develop.Infrastructure;
using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private GameModeSelectionService _gameModeSelectionService;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Main menu scene init");

            _gameModeSelectionService = _container.Resolve<GameModeSelectionService>();

            yield break;
        }

        private void Update()
        {
            if (_gameModeSelectionService != null)
                _gameModeSelectionService.Update();
        }

        public override void Run()
        {
            Debug.Log("Start main menu scene");
        }
    }
}
