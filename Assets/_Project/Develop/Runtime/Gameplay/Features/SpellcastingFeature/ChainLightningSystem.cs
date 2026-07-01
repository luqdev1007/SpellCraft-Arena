using Assets._Project.Develop.Runtime.Configs.Gameplay.Spells;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class ChainLightningSystem : IInitializableSystem, IUpdatableSystem
    {
        private const float BeamDuration = 0.15f;
        private const float ImpactDuration = 1.5f;

        private readonly EntitiesLifeContext _entitiesLifeContext;

        private Entity _entity;
        private ReactiveVariable<bool> _active;
        private ReactiveVariable<float> _jumpTimer;

        public ChainLightningSystem(EntitiesLifeContext entitiesLifeContext)
        {
            _entitiesLifeContext = entitiesLifeContext;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _active = entity.ChainLightningActive;
            _jumpTimer = entity.ChainLightningJumpTimer;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_active.Value == false)
                return;

            ChainLightningState state = _entity.ChainLightningStateC;
            SpellConfig config = state.Config;

            _jumpTimer.Value += deltaTime;

            if (_jumpTimer.Value < config.JumpDelay)
                return;

            _jumpTimer.Value = 0f;

            if (state.JumpsRemaining <= 0)
            {
                _active.Value = false;
                return;
            }

            ICollection<Entity> excluded = config.CanHitSameTarget
                ? new HashSet<Entity> { state.CurrentTarget }
                : new HashSet<Entity>(state.HitTargets);

            Vector3 origin = state.CurrentTarget.Transform.position;

            if (ChainLightningTargeting.TryFindNextTarget(_entity, _entitiesLifeContext, origin, config.BounceRange, excluded, out Entity nextTarget) == false)
            {
                _active.Value = false;
                return;
            }

            state.CurrentDamage *= config.DamageFalloff;
            state.JumpsRemaining--;

            EntitiesHelper.TryTakeDamageFrom(_entity, nextTarget, state.CurrentDamage);

            SpawnBeamAndImpact(config, origin, nextTarget.Transform.position);

            state.HitTargets.Add(nextTarget);
            state.CurrentTarget = nextTarget;
        }

        public static void SpawnBeamAndImpact(SpellConfig config, Vector3 from, Vector3 to)
        {
            GameObject beamGameObject = new GameObject("LightningBeam");
            LightningBeamView beam = beamGameObject.AddComponent<LightningBeamView>();
            beam.Init(from, to, BeamDuration);

            if (string.IsNullOrEmpty(config.PrefabPath))
                return;

            GameObject impactPrefab = Resources.Load<GameObject>(config.PrefabPath);

            if (impactPrefab == null)
                return;

            GameObject impact = Object.Instantiate(impactPrefab, to, Quaternion.identity);
            Object.Destroy(impact, ImpactDuration);
        }
    }
}
