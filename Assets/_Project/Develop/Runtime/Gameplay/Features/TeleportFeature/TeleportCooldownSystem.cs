using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature
{
    public class TeleportCooldownSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private ReactiveVariable<float> _initialCooldown;
        private ReactiveVariable<float> _currentCooldown;
        private ReactiveVariable<bool> _isCooldownReady;
        private ReactiveEvent _teleportRequest;

        private IDisposable _teleportRequestDisposable;

        public void OnInit(Entity entity)
        {
            _initialCooldown = entity.TeleportInitialCooldown;
            _currentCooldown = entity.TeleportCurrentCooldown;
            _isCooldownReady = entity.IsTeleportCooldownReady;
            _teleportRequest = entity.TeleportRequest;

            _teleportRequestDisposable = _teleportRequest.Subscribe(OnTeleportRequested);
        }

        private void OnTeleportRequested()
        {
            _currentCooldown.Value = _initialCooldown.Value;
            _isCooldownReady.Value = false;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_currentCooldown.Value <= 0)
            {
                _isCooldownReady.Value = true;
            }
            else
            {
                _currentCooldown.Value -= deltaTime;
            }
        }

        public void OnDispose()
        {
            _teleportRequestDisposable.Dispose();
        }
    }
}
