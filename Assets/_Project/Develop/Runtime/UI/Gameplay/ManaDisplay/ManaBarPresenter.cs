using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.ManaDisplay
{
    public class ManaBarPresenter : IPresenter
    {
        private readonly BarWithText _bar;
        private readonly Entity _entity;

        private ReactiveVariable<float> _currentMana;
        private ReactiveVariable<float> _maxMana;

        private readonly List<IDisposable> _disposables = new();

        public ManaBarPresenter(BarWithText bar, Entity entity)
        {
            _bar = bar;
            _entity = entity;
        }

        public void Initialize()
        {
            _currentMana = _entity.CurrentMana;
            _maxMana = _entity.MaxMana;

            _disposables.Add(_currentMana.Subscribe((prev, next) => UpdateBar()));
            _disposables.Add(_maxMana.Subscribe((prev, next) => UpdateBar()));

            _bar.SetFillerColor(new Color(0.15f, 0.45f, 1f));
            UpdateBar();
        }

        public void Dispose()
        {
            foreach (IDisposable d in _disposables)
                d.Dispose();

            _disposables.Clear();
        }

        private void UpdateBar()
        {
            float max = _maxMana.Value;

            if (max <= 0f)
            {
                _bar.UpdateSlider(0f);
                _bar.UpdateText("0");
                return;
            }

            _bar.UpdateSlider(_currentMana.Value / max);
            _bar.UpdateText(Mathf.FloorToInt(_currentMana.Value).ToString());
        }
    }
}
