using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeHandler
    {
        private readonly DIContainer _container;
        private readonly GameplayInputArgs _inputArgs;
        private readonly string[] _allowedSymbols;

        private string _combination;
        private int _currentIndex;
        private bool _isGameActive;
        private bool _isVictory;
        private bool _isWaitingForContinue;

        public TypeModeHandler(DIContainer container, GameplayInputArgs inputArgs)
        {
            _container = container;
            _inputArgs = inputArgs;
            _allowedSymbols = _inputArgs.TypeSymbolsGameMode.Symbols;
        }

        public void StartGame()
        {
            GenerateCombination();
            _currentIndex = 0;
            _isGameActive = true;
            _isVictory = false;
            _isWaitingForContinue = false;

            Debug.Log($"Target combination: {_combination}");
        }

        public void Update()
        {
            if (!_isGameActive && !_isWaitingForContinue)
                return;

            if (_isGameActive)
            {
                if (Input.anyKeyDown)
                    HandleInput();
            }
            else if (_isWaitingForContinue)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    ContinueAfterResult();
            }
        }

        private void HandleInput()
        {
            string pressedKey = Input.inputString.ToUpper();

            if (string.IsNullOrEmpty(pressedKey))
                return;

            Debug.Log($"Pressed key: {pressedKey}");

            if (pressedKey == _combination[_currentIndex].ToString().ToUpper())
            {
                _currentIndex++;
                Debug.Log($"Correct! Progress: {_currentIndex}/{_combination.Length}");

                if (_currentIndex >= _combination.Length)
                    EndGame(true);
            }
            else
            {
                EndGame(false);
            }
        }

        private void EndGame(bool victory)
        {
            _isGameActive = false;
            _isVictory = victory;
            _isWaitingForContinue = true;

            Debug.Log(victory ? "Combination completed! Victory!" : "Wrong key! Defeat...");
            Debug.Log("Press 'Space' to continue");
        }

        private void ContinueAfterResult()
        {
            _isWaitingForContinue = false;

            SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
            ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            if (_isVictory)
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
