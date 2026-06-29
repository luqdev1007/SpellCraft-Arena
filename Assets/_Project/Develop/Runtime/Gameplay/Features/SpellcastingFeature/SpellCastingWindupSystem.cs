using Assets._Project.Develop.Runtime.Configs.Gameplay.Spells;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class SpellCastingWindupSystem : IInitializableSystem, IUpdatableSystem, IDisposableSystem
    {
        private readonly EntitiesFactory _entitiesFactory;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        private Entity _entity;
        private ReactiveEvent _castRequest;
        private ReactiveVariable<bool> _isCasting;
        private ReactiveVariable<float> _windupCurrentTime;
        private ReactiveVariable<float> _currentMana;
        private ReactiveVariable<float> _maxMana;

        private IDisposable _castRequestSub;

        public SpellCastingWindupSystem(EntitiesFactory entitiesFactory, EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesFactory = entitiesFactory;
            _entitiesLifeContext = entitiesLifeContext;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _castRequest = entity.CastRequest;
            _isCasting = entity.IsCasting;
            _windupCurrentTime = entity.CastWindupCurrentTime;
            _currentMana = entity.CurrentMana;
            _maxMana = entity.MaxMana;

            _castRequestSub = _castRequest.Subscribe(OnCastRequested);
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isCasting.Value == false)
                return;

            SpellConfig config = _entity.ActiveSpellConfig;

            if (config == null)
            {
                _isCasting.Value = false;
                return;
            }

            _windupCurrentTime.Value += deltaTime;

            if (_windupCurrentTime.Value >= config.CastTime)
                ExecuteCast(config);
        }

        public void OnDispose()
        {
            _castRequestSub?.Dispose();
        }

        private void OnCastRequested()
        {
            if (_isCasting.Value)
                return;

            SpellConfig config = _entity.ActiveSpellConfig;

            if (config == null)
                return;

            if (_currentMana.Value < config.ManaCost)
            {
                Debug.Log($"[SpellCast] Not enough mana ({_currentMana.Value:0}/{config.ManaCost:0}) for {config.Name}");
                return;
            }

            _isCasting.Value = true;
            _windupCurrentTime.Value = 0f;
        }

        private void ExecuteCast(SpellConfig config)
        {
            _currentMana.Value = Mathf.Max(0f, _currentMana.Value - config.ManaCost);
            _isCasting.Value = false;
            _windupCurrentTime.Value = 0f;

            Debug.Log($"[SpellCast] ExecuteCast: {config.Name}, type={config.CastType}, dmg={config.Damage}");

            switch (config.CastType)
            {
                case SpellCastType.Projectile:
                    CastProjectile(config);
                    break;

                case SpellCastType.AreaOfEffect:
                    CastAreaOfEffect(config);
                    break;

                default:
                    string aspects = config.Aspects != null ? string.Join("+", config.Aspects) : "unknown";
                    Debug.Log($"[SpellCast] {config.Name} [{aspects}] — not implemented yet");
                    break;
            }
        }

        private void CastProjectile(SpellConfig config)
        {
            Debug.Log($"[SpellCast] CastProjectile start");

            if (_entity.TryGetShootPoint(out Transform shootPoint) == false)
                shootPoint = _entity.Transform;

            Vector3 dir = shootPoint.forward;
            Debug.Log($"[SpellCast] Spawning fireball at {shootPoint.position}, dir={dir}");
            _entitiesFactory.CreateFireballProjectile(shootPoint.position, dir, config.Damage, _entity);
            Debug.Log($"[SpellCast] Fireball spawned OK");
        }

        private void CastAreaOfEffect(SpellConfig config)
        {
            Vector3 spawnPos = _entity.Transform.position + _entity.Transform.forward * 3f;

            if (!string.IsNullOrEmpty(config.PrefabPath))
            {
                GameObject vfxPrefab = Resources.Load<GameObject>(config.PrefabPath);
                if (vfxPrefab != null)
                    Object.Instantiate(vfxPrefab, spawnPos, _entity.Transform.rotation);
            }

            IReadOnlyList<Entity> entities = _entitiesLifeContext.Entities;

            for (int i = 0; i < entities.Count; i++)
            {
                Entity target = entities[i];

                if (target == _entity)
                    continue;

                if (target.TryGetComponent(out CurrentHealth _) == false)
                    continue;

                float dist = Vector3.Distance(target.Transform.position, spawnPos);

                if (dist <= config.AoeRadius)
                    EntitiesHelper.TryTakeDamageFrom(_entity, target, config.Damage);
            }
        }
    }
}
