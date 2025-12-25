using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using Assets._Project.Develop.Runtime.Utilites.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class ChooseLowestHealthTargetTeleportState : State, IUpdatableState
    {
        private ReactiveVariable<float> _currentEnergy;
        private ReactiveVariable<float> _maxEnergy;
        private ICompositeCondition _canTeleport;
        private ReactiveVariable<float> _maxTeleportRange;
        private TeleportSystem _teleportSystem;
        private LowestHealthTargetSelector _targetSelector;
        private EntitiesLifeContext _entitiesLifeContext;
        private Transform _transform;

        private float _minEnergyPercentForTeleport;

        public ChooseLowestHealthTargetTeleportState(
            Entity entity,
            TeleportSystem teleportSystem,
            LowestHealthTargetSelector targetSelector,
            float minEnergyPercentForTeleport,
            EntitiesLifeContext entitiesLifeContext)
        {
            _canTeleport = entity.CanTeleport;
            _currentEnergy = entity.EnergyCurrentValue;
            _maxEnergy = entity.EnergyMaxValue;
            _maxTeleportRange = entity.MaxTeleportRange;
            _teleportSystem = teleportSystem;
            _targetSelector = targetSelector;
            _minEnergyPercentForTeleport = minEnergyPercentForTeleport;
            _entitiesLifeContext = entitiesLifeContext;
            _transform = entity.Transform;
        }

        public void Update(float deltaTime)
        {
            if (_canTeleport.Evaluate() == false || IsEnoughPercentOfEnergy() == false)
                return;

            TeleportToLowestHealthTarget();
        }

        private void TeleportToLowestHealthTarget()
        {
            Entity entity = _targetSelector.SelectTargetFrom(_entitiesLifeContext.Entities);

            if (entity == null)
                return;

            if (Vector3.Distance(entity.Transform.position, _transform.position) > _maxTeleportRange.Value * 3)
                return;

            _teleportSystem.TeleportTo(entity.Transform.position + entity.Transform.forward * -2);
        }

        private bool IsEnoughPercentOfEnergy() => _currentEnergy.Value / _maxEnergy.Value >= _minEnergyPercentForTeleport;
    }
}
