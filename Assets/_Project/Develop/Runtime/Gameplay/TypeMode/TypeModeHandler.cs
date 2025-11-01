using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeHandler
    {
        private readonly GameplayInputArgs _inputArgs;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly TypeModeCombinationGeneratorService _generator;
        private readonly TypeModeInputService _inputService;
        private readonly TypeModeResultService _resultService;
        private readonly ChatPresenter _chatPresenter;
        private string _combination;
        private int _currentIndex;
        private bool _isGameActive;
        private bool _isWaitingForContinue;
        private bool _isVictory;

        public TypeModeHandler(GameplayInputArgs inputArgs, 
            ICoroutinesPerformer coroutinesPerformer, 
            TypeModeCombinationGeneratorService generator, 
            TypeModeInputService inputService, 
            TypeModeResultService resultService,
            ChatPresenter chatPresenter)
        {
            _inputArgs = inputArgs;
            _coroutinesPerformer = coroutinesPerformer;
            _generator = generator;
            _inputService = inputService;
            _resultService = resultService;
            _chatPresenter = chatPresenter;
        }

        public void StartGame()
        {
            _combination = _generator.GenerateCombination(_inputArgs.AllowedSymbols);
            _currentIndex = 0;
            _isGameActive = true;
            _isWaitingForContinue = false;
            _isVictory = false;

            Debug.Log($"Target combination: {_combination}");
            _chatPresenter.SendMessageInChat("purple", "Admin", $"Target combination: {_combination}");
        }

        public void Update()
        {
            if (_isGameActive)
            {
                HandleGameplayInput();
            }
            else if (_isWaitingForContinue && _inputService.IsContinuePressed())
            {
                _coroutinesPerformer.StartPerform(_isVictory? _resultService.ContinueAfterVictory(_inputArgs) : _resultService.ContinueAfterLose(_inputArgs));
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
