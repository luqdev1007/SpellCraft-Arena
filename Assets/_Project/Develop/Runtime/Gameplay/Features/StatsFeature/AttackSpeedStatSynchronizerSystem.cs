using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StatsFeature
{
    public class AttackSpeedStatSynchronizerSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _attackProcessTime;
        private ReactiveVariable<float> _attackDelayTime;
        private ReactiveVariable<float> _attackCooldown;
        private Dictionary<StatTypes, float> _modifiedStats;

        private float _baseAttackProcessTime;
        private float _baseAttackDelayTime;
        private float _baseAttackCooldown;

        public void OnInit(Entity entity)
        {
            _attackProcessTime = entity.AttackProcessInitialTime;
            _attackDelayTime = entity.AttackDelayTime;
            _attackCooldown = entity.AttackCooldownInitialTime;
            _modifiedStats = entity.ModifiedStats;
            _baseAttackProcessTime = entity.BaseStats[StatTypes.AttackProcessTime];
            _baseAttackDelayTime = entity.BaseStats[StatTypes.AttackDelayTime];
            _baseAttackCooldown = entity.BaseStats[StatTypes.AttackCooldown];
        }

        public void OnUpdate(float deltaTime)
        {
            float attackSpeedMultiplier = _modifiedStats[StatTypes.AttackSpeedMultiplier];

            if (attackSpeedMultiplier <= 0f)
                attackSpeedMultiplier = 0.01f;

            _attackProcessTime.Value = _baseAttackProcessTime / attackSpeedMultiplier;
            _attackDelayTime.Value = _baseAttackDelayTime / attackSpeedMultiplier;
            _attackCooldown.Value = _baseAttackCooldown / attackSpeedMultiplier;
        }
    }
}