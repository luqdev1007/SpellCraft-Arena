using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.ManaDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.SpellPanel;
using Assets._Project.Develop.Runtime.UI.Gameplay.Stages;
using Assets._Project.Develop.Runtime.UI.Wallet;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenPresenter : IPresenter
    {
        private readonly GameplayScreenView _screen;

        private readonly GameplayPresentersFactory _gameplayPresentersFactory;
        private readonly ProjectPresentersFactory _projectPresentersFactory;

        private readonly List<IPresenter> _childPresenters = new();

        private EntitiesHealthDisplayPresenter _entitiesHealthDisplayPresenter;

        private readonly MainHeroHolderService _mainHeroHolderService;
        private IDisposable _mainHeroHolderServiceDisposable;
        private CurrencyPresenter _mainHeroCoinsPresenter;
        private ManaBarPresenter _manaBarPresenter;
        private SpellPanelPresenter _spellPanelPresenter;

        private Entity _heroEntity;
        private Camera _camera;

        public GameplayScreenPresenter(
            GameplayScreenView screen,
            GameplayPresentersFactory gameplayPresentersFactory,
            MainHeroHolderService mainHeroHolderService,
            ProjectPresentersFactory projectPresentersFactory)
        {
            _screen = screen;
            _gameplayPresentersFactory = gameplayPresentersFactory;
            _mainHeroHolderService = mainHeroHolderService;
            _projectPresentersFactory = projectPresentersFactory;
        }

        public void Initialize()
        {
            _camera = Camera.main;

            CreateStageNumber();
            CreateEntitiesHealthDisplay();

            _mainHeroHolderServiceDisposable = _mainHeroHolderService.HeroRegistred.Subscribe(OnHeroRegistred);

            foreach (IPresenter presenter in _childPresenters)
                presenter.Initialize();
        }

        public void Dispose()
        {
            _mainHeroHolderServiceDisposable.Dispose();
            _mainHeroCoinsPresenter?.Dispose();
            _manaBarPresenter?.Dispose();
            _spellPanelPresenter?.Dispose();

            foreach (IPresenter presenter in _childPresenters)
                presenter.Dispose();

            _childPresenters.Clear();
        }

        public void LateUpdate()
        {
            _entitiesHealthDisplayPresenter.LateUpdate();
            UpdateManaBarPosition();
        }

        private void OnHeroRegistred(Entity entity)
        {
            _heroEntity = entity;

            _mainHeroCoinsPresenter = _projectPresentersFactory.CreateCurrencyPresenter(_screen.CoinsView, entity.Coins, Meta.Features.Wallet.CurrencyTypes.Gold);
            _mainHeroCoinsPresenter.Initialize();

            if (_screen.ManaBarView != null)
            {
                _manaBarPresenter = _gameplayPresentersFactory.CreateManaBarPresenter(_screen.ManaBarView, entity);
                _manaBarPresenter.Initialize();
            }

            if (_screen.SpellPanelView != null)
            {
                _spellPanelPresenter = _gameplayPresentersFactory.CreateSpellPanelPresenter(_screen.SpellPanelView, entity);
                _spellPanelPresenter.Initialize();
            }
        }

        private void UpdateManaBarPosition()
        {
            if (_screen.ManaBarView == null || _heroEntity == null || _camera == null)
                return;

            if (_heroEntity.TryGetHealthBarPoint(out Transform barPoint) == false)
                return;

            Vector3 screenPos = _camera.WorldToScreenPoint(barPoint.position);
            screenPos.y -= 30f;
            _screen.ManaBarView.transform.position = screenPos;
        }

        private void CreateStageNumber()
        {
            StagePresenter stagePresenter = _gameplayPresentersFactory.CreateStagePresenter(_screen.StageNumberView);

            _childPresenters.Add(stagePresenter);
        }

        private void CreateEntitiesHealthDisplay()
        {
            _entitiesHealthDisplayPresenter = _gameplayPresentersFactory.CreateEntitiesHealthDisplayPresenter(_screen.EntitiesHealthDisplay);

            _childPresenters.Add(_entitiesHealthDisplayPresenter);
        }
    }
}
