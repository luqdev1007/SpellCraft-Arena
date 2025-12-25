using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilites;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature
{
    public class DealDamageInRangeAfterTeleportSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _teleportEvent;
        private ReactiveVariable<float> _damage;
        private ReactiveVariable<float> _range;
        private Transform _transform;
        private Collider _body;

        private readonly CollidersRegistryService _colllidersRegistryService;

        public DealDamageInRangeAfterTeleportSystem(CollidersRegistryService colllidersRegistryService)
        {
            _colllidersRegistryService = colllidersRegistryService;
        }

        private IDisposable _teleportEventDisposable;

        public void OnInit(Entity entity)
        {
            _teleportEvent = entity.TeleportRequest;
            _damage = entity.AttackDamage;
            _range = entity.AttackRange;
            _transform = entity.Transform;
            _body = entity.BodyCollider;
            Debug.Log(_body.gameObject.name);

            _teleportEventDisposable = _teleportEvent.Subscribe(OnTeleport);
        }

        public void OnDispose()
        {
            _teleportEventDisposable.Dispose();
        }

        private void OnTeleport()
        {

            Collider[] colliders = Physics.OverlapSphere(_transform.position, _range.Value);

            foreach (Collider collider in colliders)
            {
                Entity contactEntity = _colllidersRegistryService.GetBy(collider);

                if (contactEntity != null && collider != _body)
                {
                    Debug.Log(collider.gameObject.name);

                    if (contactEntity.HasComponent<TakeDamageRequest>())
                        contactEntity.TakeDamageRequest.Invoke(_damage.Value);
                }
            }
        }
    }
}
