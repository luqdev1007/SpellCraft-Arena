using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class FireballExplosionVfxSystem : IInitializableSystem, IDisposableSystem
    {
        private GameObject _explosionVfxPrefab;
        private ReactiveVariable<bool> _isDead;
        private Transform _transform;
        private IDisposable _deathDisposable;

        public void OnInit(Entity entity)
        {
            if (entity.TryGetComponent(out FireballExplosionVfx vfxComponent))
            {
                _explosionVfxPrefab = vfxComponent.ExplosionVfxPrefab;
            }

            _isDead = entity.IsDead;
            _transform = entity.Transform;

            _deathDisposable = _isDead.Subscribe(OnDeathChanged);
        }

        private void OnDeathChanged(bool arg1, bool isDead)
        {
            if (!isDead)
                return;

            if (_explosionVfxPrefab == null)
                return;

            // Spawn explosion VFX at fireball position and auto-destroy after 4s
            GameObject vfx = Object.Instantiate(_explosionVfxPrefab, _transform.position, Quaternion.identity);
            Object.Destroy(vfx, 4f);
        }

        public void OnDispose()
        {
            _deathDisposable?.Dispose();
        }
    }
}
