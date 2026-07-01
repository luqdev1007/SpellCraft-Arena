using Assets._Project.Develop.Runtime.Configs.Gameplay.Spells;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature;
using Assets._Project.Develop.Runtime.UI.Core;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.SpellPanel
{
    public class SpellPanelPresenter : IPresenter
    {
        private const float ShieldActiveAlpha = 1f;
        private const float ShieldInactiveAlpha = 0.65f;
        private const float ShieldFadeDuration = 0.18f;

        private readonly SpellPanelView _view;
        private readonly Entity _heroEntity;
        private readonly SpellsConfigsContainer _spellsContainer;
        private readonly Sprite[] _aspectIcons;

        private readonly List<Aspect> _selectedAspects = new(3);
        private OrbsDisplayView _orbsDisplay;
        private IDisposable _castingSubscription;
        private IDisposable _blinkCooldownSubscription;
        private IDisposable _shieldActiveSubscription;

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
                _view.TeleportButton.onClick.AddListener(OnTeleportClicked);

            if (_view.ShieldButton != null)
                _view.ShieldButton.onClick.AddListener(OnShieldButtonClicked);

            _castingSubscription = _heroEntity.IsCasting.Subscribe(OnCastingChanged);
            _blinkCooldownSubscription = _heroEntity.BlinkCooldownCurrentTime.Subscribe(OnBlinkCooldownTimeChanged);
            UpdateBlinkCooldownFill();

            _shieldActiveSubscription = _heroEntity.IsShieldActive.Subscribe(OnShieldActiveChanged);
            SetShieldVisualActive(_heroEntity.IsShieldActive.Value, instant: true);

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

            if (_view.TeleportButton != null)
                _view.TeleportButton.onClick.RemoveListener(OnTeleportClicked);

            _blinkCooldownSubscription?.Dispose();

            if (_view.ShieldButton != null)
                _view.ShieldButton.onClick.RemoveListener(OnShieldButtonClicked);

            _shieldActiveSubscription?.Dispose();

            _view.ShieldFrameImage?.DOKill();
            _view.ShieldIconImage?.DOKill();
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
                Debug.Log($"[SpellPanel] Комбинация {combo} не найдена в SpellsConfigsContainer");
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

        private void OnTeleportClicked()
        {
            _heroEntity.BlinkRequest.Invoke();
        }

        private void OnBlinkCooldownTimeChanged(float previous, float current)
        {
            UpdateBlinkCooldownFill();
        }

        private void UpdateBlinkCooldownFill()
        {
            if (_view.BlinkCooldownFillImage == null)
                return;

            float initial = _heroEntity.BlinkCooldownInitialTime.Value;

            _view.BlinkCooldownFillImage.fillAmount = _heroEntity.InBlinkCooldown.Value && initial > 0f
                ? Mathf.Clamp01(_heroEntity.BlinkCooldownCurrentTime.Value / initial)
                : 0f;
        }

        private void OnShieldButtonClicked()
        {
            _heroEntity.ShieldToggleRequest.Invoke();
        }

        private void OnShieldActiveChanged(bool prev, bool isActive)
        {
            SetShieldVisualActive(isActive, instant: false);
        }

        private void SetShieldVisualActive(bool active, bool instant)
        {
            float targetAlpha = active ? ShieldActiveAlpha : ShieldInactiveAlpha;

            ApplyShieldFade(_view.ShieldFrameImage, targetAlpha, instant, active);
            ApplyShieldFade(_view.ShieldIconImage, targetAlpha, instant, active);
        }

        private void ApplyShieldFade(Image image, float targetAlpha, bool instant, bool active)
        {
            if (image == null)
                return;

            image.DOKill();

            if (instant)
            {
                Color color = image.color;
                color.a = targetAlpha;
                image.color = color;
                return;
            }

            image.DOFade(targetAlpha, ShieldFadeDuration)
                .SetUpdate(true)
                .SetEase(active ? Ease.OutQuad : Ease.InQuad);
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