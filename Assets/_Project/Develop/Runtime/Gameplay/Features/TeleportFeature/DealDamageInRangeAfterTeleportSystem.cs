using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature
{
    public class DealDamageInRangeAfterTeleportSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _teleportEvent;
        private ReactiveVariable<float> _damage;
        private CapsuleCollider _body;

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
            _body = entity.BodyCollider;

            _teleportEventDisposable = _teleportEvent.Subscribe(OnTeleport);
        }

        public void OnDispose()
        {
            _teleportEventDisposable.Dispose();
        }

        private void OnTeleport()
        {
            Transform bodyTransform = _body.transform; 
            Vector3 center = _body.center;
            float height = _body.height;
            float radius = _body.radius;

            Vector3 point1 = bodyTransform.position + center + bodyTransform.up * (height / 2f - radius);
            Vector3 point2 = bodyTransform.position + center - bodyTransform.up * (height / 2f - radius);


            Collider[] colliders = Physics.OverlapCapsule(point1,
                point2,
                radius);

            Debug.Log("Colliders around: " + colliders.Length);

            foreach (Collider collider in colliders)
            {
                Entity contactEntity = _colllidersRegistryService.GetBy(collider);

                if (contactEntity != null && collider != _body)
                {
                    if (contactEntity.HasComponent<TakeDamageRequest>())
                        contactEntity.TakeDamageRequest.Invoke(_damage.Value);
                }
            }
        }
    }
}
