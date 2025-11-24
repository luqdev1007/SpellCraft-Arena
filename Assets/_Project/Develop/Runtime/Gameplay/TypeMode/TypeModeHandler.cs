using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeHandler : IDisposable
    {
        private readonly GameplayInputArgs _inputArgs;
        private readonly TypeModeCombinationGeneratorService _generator;
        private readonly GameResultService _resultService;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly ChatService _chatService;
        private readonly GameplayPopupService _gameplayPopupService;

        private string _combination;
        private bool _isGame = false;

        public TypeModeHandler(GameplayInputArgs inputArgs,
            TypeModeCombinationGeneratorService generator,
            GameResultService resultService,
            ICoroutinesPerformer coroutinesPerformer,
            ChatService chatService,
            GameplayPopupService gameplayPopupService)
        {
            _inputArgs = inputArgs;
            _generator = generator;
            _resultService = resultService;
            _coroutinesPerformer = coroutinesPerformer;
            _chatService = chatService;
            _gameplayPopupService = gameplayPopupService;
        }


        public void Init()
        {
            _chatService.MessageAddedInHistory += OnMessageAddedInHistory;
        }

        public void Dispose()
        {
            _chatService.MessageAddedInHistory -= OnMessageAddedInHistory;
        }

        public void StartGame()
        {
            _combination = _generator.GenerateCombination(_inputArgs.AllowedSymbols);

            Debug.Log($"Target combination: {_combination}");
            _chatService.AddMessage($"Target combination: {_combination}", "Admin");
            _isGame = true;
        }

        private void OnMessageAddedInHistory(string value)
        {
            if (_isGame)
                _coroutinesPerformer.StartPerform(HandleEndGame(_combination == value.PartAfter(':')));
        }

        private IEnumerator HandleEndGame(bool isVictory)
        {
            _isGame = false;

            if (isVictory)
            {
                yield return _resultService.RegisterVictory();
                _chatService.AddMessage("Ты выиграл!", "Admin");
                EndOfBattlePresenter endOfBattlePopup = _gameplayPopupService.OpenEndOfBattlePopup();
                endOfBattlePopup.ShowVictory();
            }
            else
            {
                yield return _resultService.RegisterDefeat();
                _chatService.AddMessage("Ты проиграл...", "Admin");
                EndOfBattlePresenter endOfBattlePopup = _gameplayPopupService.OpenEndOfBattlePopup();
                endOfBattlePopup.ShowDefeat();
            }
        }
    }
}
