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
        private readonly LevelsProgressionService _levelService;
        private readonly SceneSwitcherService _sceneSwitcher;
        private readonly ICoroutinesPerformer _coroutinesPerformer;

        private readonly int _levelNumber;

        private readonly LevelTileView _view;

        public LevelTilePresenter(
            LevelsProgressionService levelService, 
            SceneSwitcherService sceneSwitcher, 
            ICoroutinesPerformer coroutinesPerformer, 
            int levelNumber, 
            LevelTileView view)
        {
            _levelService = levelService;
            _sceneSwitcher = sceneSwitcher;
            _coroutinesPerformer = coroutinesPerformer;
            _levelNumber = levelNumber;
            _view = view;
        }

        public LevelTileView View => _view;

        public void Initialize()
        {
            _view.SetLevel(_levelNumber.ToString());

            if (_levelService.CanPlay(_levelNumber))
            {
                if (_levelService.IsLevelCompleted(_levelNumber))
                    _view.SetComplete();
                else
                    _view.SetActive();
            }
            else
            {
                _view.SetBlock();
            }
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
            if (_levelService.CanPlay(_levelNumber) == false)
            {
                Debug.Log("Level is blocked");
                return;
            }

            _coroutinesPerformer
                .StartPerform(_sceneSwitcher.ProcessingSwitchTo(Scenes.Gameplay, new GameplayInputArgs(levelNumber: _levelNumber)));
        }
    }
}
