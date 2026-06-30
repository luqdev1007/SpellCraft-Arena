using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.BlinkFeature
{
    public class BlinkCooldownTimerSystem : IInitializableSystem, IDisposableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _currentTime;
        private ReactiveVariable<float> _initialTime;
        private ReactiveVariable<bool> _inBlinkCooldown;

        private ReactiveEvent _blinkExecutedEvent;

        private IDisposable _blinkExecutedEventDisposable;

        public void OnInit(Entity entity)
        {
            _currentTime = entity.BlinkCooldownCurrentTime;
            _initialTime = entity.BlinkCooldownInitialTime;
            _inBlinkCooldown = entity.InBlinkCooldown;
            _blinkExecutedEvent = entity.BlinkExecutedEvent;

            _blinkExecutedEventDisposable = _blinkExecutedEvent.Subscribe(OnBlinkExecuted);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inBlinkCooldown.Value == false)
                return;

            _currentTime.Value -= deltaTime;

            if (CooldownIsOver())
                _inBlinkCooldown.Value = false;
        }

        public void OnDispose()
        {
            _blinkExecutedEventDisposable.Dispose();
        }

        private void OnBlinkExecuted()
        {
            _currentTime.Value = _initialTime.Value;
            _inBlinkCooldown.Value = true;
        }

        private bool CooldownIsOver() => _currentTime.Value <= 0;
    }
}
