using Assets._Project.Develop.Runtime.Configs.Gameplay.Spells;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature;
using Assets._Project.Develop.Runtime.UI.Core;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.SpellPanel
{
    public class SpellPanelPresenter : IPresenter
    {
        private readonly SpellPanelView _view;
        private readonly Entity _heroEntity;
        private readonly SpellsConfigsContainer _spellsContainer;
        private readonly Sprite[] _aspectIcons;

        private readonly List<Aspect> _selectedAspects = new(3);
        private OrbsDisplayView _orbsDisplay;
        private IDisposable _castingSubscription;

        public SpellPanelPresenter(
            SpellPanelView view,
            Entity heroEntity,
            SpellsConfigsContainer spellsContainer,
            Sprite[] aspectIcons)
        {
            _view = view;
            _heroEntity = heroEntity;
            _spellsContainer = spellsContainer;
            _aspectIcons = aspectIcons;
        }

        public void Initialize()
        {
            Aspect[] allAspects = (Aspect[])Enum.GetValues(typeof(Aspect));

            for (int i = 0; i < _view.AspectButtons.Length && i < allAspects.Length; i++)
            {
                Aspect aspect = allAspects[i];
                Sprite icon = i < _aspectIcons.Length ? _aspectIcons[i] : null;

                _view.AspectButtons[i].Setup(aspect, icon);
                _view.AspectButtons[i].Clicked += OnAspectClicked;
                _view.AspectButtons[i].LongPressed += OnAspectLongPressed;
            }

            _view.CastButton.onClick.AddListener(OnCastClicked);

            if (_view.TeleportButton != null)
                _view.TeleportButton.onClick.AddListener(() => Debug.Log("[UI] РўРµР»РµРїРѕСЂС‚ вЂ” РЅРµ СЂРµР°Р»РёР·РѕРІР°РЅРѕ"));

            if (_view.ShieldButton != null)
                _view.ShieldButton.onClick.AddListener(() => Debug.Log("[UI] Р©РёС‚ вЂ” РЅРµ СЂРµР°Р»РёР·РѕРІР°РЅРѕ"));

            _castingSubscription = _heroEntity.IsCasting.Subscribe(OnCastingChanged);

            _orbsDisplay = _heroEntity.Transform.GetComponentInChildren<OrbsDisplayView>();

            if (_heroEntity.ActiveSpellConfig == null && _spellsContainer.DefaultSpell != null)
                _heroEntity.ActiveSpellConfigC.Value = _spellsContainer.DefaultSpell;

            SpellConfig initial = _heroEntity.ActiveSpellConfig;
            if (initial != null && _view.ActiveSpellIcon != null && initial.Icon != null)
                _view.ActiveSpellIcon.sprite = initial.Icon;

            UpdateCastButtonInteractable();
        }

        public void Dispose()
        {
            foreach (AspectButtonView btn in _view.AspectButtons)
            {
                if (btn == null) continue;
                btn.Clicked -= OnAspectClicked;
                btn.LongPressed -= OnAspectLongPressed;
            }

            _view.CastButton.onClick.RemoveListener(OnCastClicked);
            _castingSubscription?.Dispose();
        }

        private void OnAspectClicked(Aspect aspect)
        {
            if (_selectedAspects.Count >= 3)
                return;

            if (_selectedAspects.Contains(aspect))
                return;

            _selectedAspects.Add(aspect);
            SetButtonSelected(aspect, true);
            _orbsDisplay?.ShowAspects(_selectedAspects);

            if (_selectedAspects.Count == 3)
                OnComboComplete();
        }

        private void OnAspectLongPressed(Aspect aspect)
        {
            if (_selectedAspects.Count >= 3)
                return;

            if (_selectedAspects.Contains(aspect) == false)
                return;

            _selectedAspects.Remove(aspect);
            SetButtonSelected(aspect, false);
            _orbsDisplay?.ShowAspects(_selectedAspects);
        }

        private void OnComboComplete()
        {
            SpellConfig matched = _spellsContainer.FindByAspects(_selectedAspects);

            if (matched != null)
            {
                _heroEntity.ActiveSpellConfigC.Value = matched;

                if (_view.ActiveSpellIcon != null && matched.Icon != null)
                    _view.ActiveSpellIcon.sprite = matched.Icon;
            }
            else
            {
                string combo = string.Join("+", _selectedAspects);
                Debug.Log($"[SpellPanel] РљРѕРјР±РёРЅР°С†РёСЏ {combo} РЅРµ РЅР°Р№РґРµРЅР° РІ SpellsConfigsContainer");
            }

            ClearSelection();
        }

        private void OnCastClicked()
        {
            _heroEntity.CastRequest.Invoke();
        }

        private void OnCastingChanged(bool prev, bool isCasting)
        {
            UpdateCastButtonInteractable();
        }

        private void UpdateCastButtonInteractable()
        {
            if (_view.CastButton != null)
                _view.CastButton.interactable = !_heroEntity.IsCasting.Value;
        }

        private void SetButtonSelected(Aspect aspect, bool selected)
        {
            foreach (AspectButtonView btn in _view.AspectButtons)
            {
                if (btn == null || btn.Aspect != aspect) continue;

                btn.SetSelected(selected);

                btn.transform
                    .DOPunchScale(Vector3.one * 0.15f, 0.2f, 5, 0.5f)
                    .SetUpdate(true);

                break;
            }
        }

        private void ClearSelection()
        {
            foreach (Aspect aspect in _selectedAspects)
                SetButtonSelectedSilent(aspect, false);

            _selectedAspects.Clear();
        }

        private void SetButtonSelectedSilent(Aspect aspect, bool selected)
        {
            foreach (AspectButtonView btn in _view.AspectButtons)
            {
                if (btn != null && btn.Aspect == aspect)
                {
                    btn.SetSelected(selected);
                    break;
                }
            }
        }
    }
}

