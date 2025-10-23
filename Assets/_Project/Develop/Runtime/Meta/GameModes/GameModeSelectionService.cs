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
                string allowedSymbols = _configsProviderService.GetConfig<TypeModeConfig>().GetValueFor(TypeModeSymbols.Digits);
                _coroutinesPerformer.StartPerform(SwitchToGameplay(allowedSymbols));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                string allowedSymbols = _configsProviderService.GetConfig<TypeModeConfig>().GetValueFor(TypeModeSymbols.Letters);
                _coroutinesPerformer.StartPerform(SwitchToGameplay(allowedSymbols));
            }
        }

        private IEnumerator SwitchToGameplay(string allowedSymbols)
        {
            yield return _sceneSwitcherService.ProcessingSwitchTo(
                Scenes.Gameplay,
                new GameplayInputArgs(allowedSymbols)
            );
        }
    }
}
