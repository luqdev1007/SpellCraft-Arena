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
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameStatsService _gameStatsService;
        private readonly PlayerDataProvider _playerDataProvider;

        public TypeModeResultService(SceneSwitcherService sceneSwitcher, 
            ICoroutinesPerformer coroutinesPerformer, 
            GameStatsService statsService, 
            PlayerDataProvider playerDataProvider)
        {
            _sceneSwitcherService = sceneSwitcher;
            _coroutinesPerformer = coroutinesPerformer;
            _gameStatsService = statsService;
            _playerDataProvider = playerDataProvider;
        }

        public void HandleResult(bool isVictory)
        {
            Debug.Log(isVictory ? "Victory!" : "Defeat!");
            Debug.Log("Press 'Space' to continue");
        }

        public IEnumerator ContinueAfterResult(bool isVictory, GameplayInputArgs inputArgs)
        {
            if (isVictory)
            {
                _gameStatsService.RegisterVictory();
                yield return _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
                _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessingSwitchTo(Scenes.MainMenu));
            }
            else
            {
                _gameStatsService.RegisterDefeat();
                yield return _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
                _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessingSwitchTo(Scenes.Gameplay, inputArgs));
            }
        }
    }
}
