using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Shield
{
    public class ShieldToggleSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _shieldToggleRequest;
        private ReactiveVariable<bool> _isShieldActive;

        private IDisposable _shieldToggleRequestSub;

        public void OnInit(Entity entity)
        {
            _shieldToggleRequest = entity.ShieldToggleRequest;
            _isShieldActive = entity.IsShieldActive;

            _shieldToggleRequestSub = _shieldToggleRequest.Subscribe(OnShieldToggleRequested);
        }

        public void OnDispose()
        {
            _shieldToggleRequestSub?.Dispose();
        }

        private void OnShieldToggleRequested()
        {
            _isShieldActive.Value = !_isShieldActive.Value;
        }
    }
}
