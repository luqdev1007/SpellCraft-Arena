using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.TypeMode
{
    public class TypeModeHandler : IDisposable
    {
        private readonly GameplayInputArgs _inputArgs;
        private readonly TypeModeCombinationGeneratorService _generator;
        private readonly ChatPresenter _chatPresenter;
        private readonly GameResultService _resultService;
        private readonly EndOfBattlePresenter _endOfBattlePresenter;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        private string _combination;
        private bool _isGame = false;

        public TypeModeHandler(GameplayInputArgs inputArgs,
            TypeModeCombinationGeneratorService generator,
            ChatPresenter chatPresenter,
            GameResultService resultService,
            EndOfBattlePresenter endOfBattlePresenter,
            ICoroutinesPerformer coroutinesPerformer)
        {
            _inputArgs = inputArgs;
            _generator = generator;
            _chatPresenter = chatPresenter;
            _resultService = resultService;
            _endOfBattlePresenter = endOfBattlePresenter;
            _coroutinesPerformer = coroutinesPerformer;
        }


        public void Init()
        {
            _chatPresenter.MessageSent += OnMessageSent;
            _endOfBattlePresenter.RestartRequested += StartGame;
        }

        public void Dispose()
        {
            _chatPresenter.MessageSent -= OnMessageSent;
            _endOfBattlePresenter.RestartRequested -= StartGame;
        }

        public void StartGame()
        {
            _endOfBattlePresenter.Hide();
            _combination = _generator.GenerateCombination(_inputArgs.AllowedSymbols);

            Debug.Log($"Target combination: {_combination}");
            _chatPresenter.SendMessageInChat("purple", "Admin", $"Target combination: {_combination}");
            _isGame = true;
        }

        private void OnMessageSent(string value)
        {
            if (_isGame)
                _coroutinesPerformer.StartPerform(HandleEndGame(_combination == value));
        }

        private IEnumerator HandleEndGame(bool isVictory)
        {
            _isGame = false;

            if (isVictory)
            {
                yield return _resultService.RegisterVictory();
                _endOfBattlePresenter.Show("Победа!");
                _endOfBattlePresenter.ActivateNextButton();
            }
            else
            {
                yield return _resultService.RegisterDefeat();
                _endOfBattlePresenter.Show("Поражение...");
                _endOfBattlePresenter.ActivateRestartButton();
            }
        }
    }
}
