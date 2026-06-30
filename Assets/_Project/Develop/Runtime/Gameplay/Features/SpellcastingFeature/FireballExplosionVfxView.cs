using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class FireballExplosionVfxView : EntityView
    {
        [SerializeField] private GameObject _explosionVfxPrefab;

        private IReadOnlyVariable<bool> _isDead;
        private IDisposable _isDeadDisposable;

        protected override void OnEntityStartedWork(Entity entity)
        {
            _isDead = entity.IsDead;
            _isDeadDisposable = _isDead.Subscribe(OnIsDeadChanged);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);
            _isDeadDisposable?.Dispose();
        }

        private void OnIsDeadChanged(bool oldIsDead, bool isDead)
        {
            if (!isDead || _explosionVfxPrefab == null)
                return;

            // Spawn explosion VFX at fireball position
            GameObject vfx = Object.Instantiate(_explosionVfxPrefab, transform.position, Quaternion.identity);
            Object.Destroy(vfx, 4f);
        }
    }
}
