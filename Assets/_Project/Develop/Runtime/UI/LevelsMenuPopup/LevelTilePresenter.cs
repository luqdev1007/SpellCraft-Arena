using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilites.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilites.SceneManagement;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.LevelsMenuPopup
{
    public class LevelTilePresenter : ISubscribePresenter
    {
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinesPerformer _coroutinesPerformer;
        private readonly GameplayInputArgs _inputArgs;
        private readonly LevelTileView _view;

        public LevelTilePresenter(
            SceneSwitcherService sceneSwitcher, 
            ICoroutinesPerformer coroutinesPerformer, 
            GameplayInputArgs inputArgs, 
            LevelTileView view)
        {
            _sceneSwitcher = sceneSwitcher;
            _coroutinesPerformer = coroutinesPerformer;
            _inputArgs = inputArgs;
            _view = view;
        }

        public LevelTileView View => _view;

        public void Initialize()
        {
            _view.Init(_inputArgs.LevelName, _inputArgs.LevelIcon);
        }

        public void Dispose()
        {
            _view.Clicked -= OnViewClicked;
        }


        public void Subscribe()
        {
            _view.Clicked += OnViewClicked;
        }

        public void Unsubscribe()
        {
            _view.Clicked -= OnViewClicked;
        }

        private void OnViewClicked()
        {
            _coroutinesPerformer
                .StartPerform(_sceneSwitcher.ProcessingSwitchTo(Scenes.Gameplay, _inputArgs));
        }
    }
}
