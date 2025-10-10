using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeHandler
    {
        private readonly DIContainer _container;
        private readonly GameplayInputArgs _inputArgs;
        private readonly string[] _allowedSymbols;

        private string _combination;

        public TypeModeHandler(DIContainer container, GameplayInputArgs inputArgs)
        {
            _container = container;
            _inputArgs = inputArgs;
            _allowedSymbols = _inputArgs.TypeSymbolsGameMode.Symbols;
        }

        public IEnumerator ProcessingStartGame()
        {
            bool isGameCompleted = false;
            bool isVictory = false;
            int currentSymbolIndex = 0;

            GenerateCombination();
            Debug.Log("Target combination: " + _combination);

            while (isGameCompleted == false)
            {
                yield return new WaitUntil(() => Input.anyKeyDown);

                string pressedKey = Input.inputString.ToUpper();

                if (string.IsNullOrEmpty(pressedKey))
                    continue;

                Debug.Log($"Pressed key: {pressedKey}");

                if (pressedKey == _combination[currentSymbolIndex].ToString().ToUpper())
                {
                    currentSymbolIndex++;
                    Debug.Log($"Correct! Progress: {currentSymbolIndex}/{_combination.Length}");
                }
                else
                {
                    isGameCompleted = true;
                    isVictory = false;

                    Debug.Log("Wrong key! Defeat...");
                }

                if (currentSymbolIndex >= _combination.Length)
                {
                    isGameCompleted = true;
                    isVictory = true;

                    Debug.Log("Combination completed! Victory!");
                }
            }

            yield return new WaitForEndOfFrame();

            Debug.Log("Press 'Space' to continue");

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            if (isVictory)
                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessingSwitchTo(Scenes.MainMenu));
            else
                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessingSwitchTo(Scenes.Gameplay, _inputArgs));

        }

        private void GenerateCombination(int length = 5)
        {
            _combination = "";

            for (int i = 0; i < length; i++)
            {
                string randomSymbol = _allowedSymbols[Random.Range(0, _allowedSymbols.Length)];
                _combination += randomSymbol;
            }
        }
    }
}
