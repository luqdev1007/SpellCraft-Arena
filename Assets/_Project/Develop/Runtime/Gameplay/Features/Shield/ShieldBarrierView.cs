using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Shield
{
    public class ShieldBarrierView : EntityView
    {
        [SerializeField] private GameObject _magicBarrier;

        private IDisposable _shieldActiveSub;

        protected override void OnEntityStartedWork(Entity entity)
        {
            ReactiveVariable<bool> isShieldActive = entity.IsShieldActive;

            _shieldActiveSub = isShieldActive.Subscribe(OnShieldActiveChanged);

            _magicBarrier.SetActive(isShieldActive.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _shieldActiveSub?.Dispose();
        }

        private void OnShieldActiveChanged(bool prev, bool isActive)
        {
            _magicBarrier.SetActive(isActive);
        }
    }
}
