using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.Shield;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage
{
    public class ApplyDamageSystem : IInitializableSystem, IDisposableSystem
    {
        private Entity _entity;

        private ReactiveEvent<float> _damageRequest;
        private ReactiveEvent<float> _damageEvent;

        private ReactiveVariable<float> _health;

        private ICompositeCondition _canApplyDamage;

        private IDisposable _requestDisposable;

        private string _entityName;

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _entityName = entity.Transform.gameObject.name;

            _damageRequest = entity.TakeDamageRequest;
            _damageEvent = entity.TakeDamageEvent;

            _health = entity.CurrentHealth;

            _canApplyDamage = entity.CanApplyDamage;

            _requestDisposable = _damageRequest.Subscribe(OnDamageRequest);
        }

        public void OnDispose()
        {
            _requestDisposable.Dispose();
        }

        private void OnDamageRequest(float damage)
        {
            if (damage < 0)
                throw new ArgumentOutOfRangeException($"{nameof(damage)} can't be less than 0");

            if (_canApplyDamage.Evaluate() == false)
                return;

            float passthrough = ShieldAbsorptionResolver.Resolve(_entity, damage);

            _health.Value = MathF.Max(_health.Value - passthrough, 0);
            _damageEvent.Invoke(passthrough);

            // Debug.Log($"{_entityName} получил урон, у него осталось {_health.Value} ед. здоровья");
        }
    }
}
