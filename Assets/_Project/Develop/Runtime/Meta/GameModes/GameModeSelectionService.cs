using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using Assets._Project.Develop.Runtime.Utilites.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Gameplay.TypeMode;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta
{
    public class GameModeSelectionService
    {
        private readonly SceneSwitcherService _sceneSwitcherService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly ConfigsProviderService _configsProviderService;

        public GameModeSelectionService(
            SceneSwitcherService sceneSwitcherService,
            ICoroutinesPerformer coroutinesPerformer,
            ConfigsProviderService configsProviderService)
        {
            _sceneSwitcherService = sceneSwitcherService;
            _coroutinesPerformer = coroutinesPerformer;
            _configsProviderService = configsProviderService;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var config = _configsProviderService.GetConfig<TypeNumbersGameModeConfig>();
                _coroutinesPerformer.StartPerform(SwitchToGameplay(config));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                var config = _configsProviderService.GetConfig<TypeCharsGameModeConfig>();
                _coroutinesPerformer.StartPerform(SwitchToGameplay(config));
            }
        }

        private IEnumerator SwitchToGameplay(TypeSymbolsGameMode config)
        {
            yield return _sceneSwitcherService.ProcessingSwitchTo(
                Scenes.Gameplay,
                new GameplayInputArgs(config)
            );
        }
    }
}
