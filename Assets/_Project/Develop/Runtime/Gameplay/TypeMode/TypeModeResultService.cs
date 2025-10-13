using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
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

        public void ContinueAfterResult(bool isVictory, GameplayInputArgs inputArgs)
        {
            var sceneSwitcher = _container.Resolve<SceneSwitcherService>();
            var coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            if (isVictory)
                coroutinesPerformer.StartPerform(sceneSwitcher.ProcessingSwitchTo(Scenes.MainMenu));
            else
                coroutinesPerformer.StartPerform(sceneSwitcher.ProcessingSwitchTo(Scenes.Gameplay, inputArgs));
        }
    }
}
