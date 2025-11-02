using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeHandler
    {
        private readonly GameplayInputArgs _inputArgs;
        private readonly TypeModeCombinationGeneratorService _generator;
        private readonly ChatPresenter _chatPresenter;
        private readonly TypeModeResultService _resultService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        private string _combination;
        private bool _isGameStarted;

        public TypeModeHandler(GameplayInputArgs inputArgs, 
            TypeModeCombinationGeneratorService generator, 
            ChatPresenter chatPresenter,
            TypeModeResultService resultService, 
            ICoroutinesPerformer coroutinesPerformer
            )
        {
            _inputArgs = inputArgs;
            _generator = generator;
            _chatPresenter = chatPresenter;
            _resultService = resultService;
            _coroutinesPerformer = coroutinesPerformer;
        }

        public void StartGame()
        {
            _combination = _generator.GenerateCombination(_inputArgs.AllowedSymbols);

            Debug.Log($"Target combination: {_combination}");
            _chatPresenter.SendMessageInChat("purple", "Admin", $"Target combination: {_combination}");

            _chatPresenter.MessageSent += OnMessageSent;
            _isGameStarted = true;
        }

        private void OnMessageSent(string value)
        {
            if (_isGameStarted)
            {
                if (value == _combination)
                {
                    _resultService.ContinueAfterVictory();
                }
                else
                {
                    _resultService.ContinueAfterLose(_inputArgs);
                }
            }
        }
    }
}
