using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using UnityEngine;



namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeResultService
    {
        private readonly DIContainer _container;

        public TypeModeResultService(DIContainer container)
        {
            _container = container;
        }

        public void HandleResult(bool isVictory)
        {
            Debug.Log(isVictory ? "Victory!" : "Defeat!");
            Debug.Log("Press 'Space' to continue");
        }

        public IEnumerator ContinueAfterResult(bool isVictory, GameplayInputArgs inputArgs)
        {
            SceneSwitcherService sceneSwitcher = _container.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
            GameStatsService statsService = _container.Resolve<GameStatsService>();
            PlayerDataProvider playerDataProvider = _container.Resolve<PlayerDataProvider>();

            if (isVictory)
            {
                statsService.RegisterVictory();
                yield return coroutinesPerformer.StartPerform(playerDataProvider.Save());
                coroutinesPerformer.StartPerform(sceneSwitcher.ProcessingSwitchTo(Scenes.MainMenu));
            }
            else
            {
                statsService.RegisterDefeat();
                yield return coroutinesPerformer.StartPerform(playerDataProvider.Save());
                coroutinesPerformer.StartPerform(sceneSwitcher.ProcessingSwitchTo(Scenes.Gameplay, inputArgs));
            }
        }
    }
}
