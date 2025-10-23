using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Stats;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.DataProviders;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeHandler
    {
        private readonly DIContainer _container;
        private readonly GameplayInputArgs _inputArgs;

        private readonly TypeModeCombinationGeneratorService _generator;
        private readonly TypeModeInputService _inputService;
        private readonly TypeModeResultService _resultService;

        private string _combination;
        private int _currentIndex;
        private bool _isGameActive;
        private bool _isWaitingForContinue;
        private bool _isVictory;

        public TypeModeHandler(DIContainer container, GameplayInputArgs inputArgs)
        {
            _container = container;
            _inputArgs = inputArgs;

            _generator = new TypeModeCombinationGeneratorService(_inputArgs.AllowedSymbols);
            _inputService = new TypeModeInputService();
            _resultService = new TypeModeResultService(_container);
        }

        public void StartGame()
        {
            _combination = _generator.GenerateCombination();
            _currentIndex = 0;
            _isGameActive = true;
            _isWaitingForContinue = false;
            _isVictory = false;

            Debug.Log($"Target combination: {_combination}");
        }

        public void Update()
        {
            if (_isGameActive)
            {
                HandleGameplayInput();
            }
            else if (_isWaitingForContinue && _inputService.IsContinuePressed())
            {
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
                coroutinesPerformer.StartPerform(_resultService.ContinueAfterResult(_isVictory, _inputArgs));
                _isWaitingForContinue = false;
            }
        }


        private void HandleGameplayInput()
        {
            if (_inputService.TryGetPressedSymbol(out string pressed))
            {
                Debug.Log($"Pressed: {pressed}");

                if (pressed == _combination[_currentIndex].ToString().ToUpper())
                {
                    _currentIndex++;
                    Debug.Log($"Correct! {_currentIndex}/{_combination.Length}");

                    if (_currentIndex >= _combination.Length)
                        EndGame(true);
                }
                else
                {
                    EndGame(false);
                }
            }
        }

        private void EndGame(bool victory)
        {
            _isGameActive = false;
            _isWaitingForContinue = true;
            _isVictory = victory;

            _resultService.HandleResult(victory);
        }
    }
}
