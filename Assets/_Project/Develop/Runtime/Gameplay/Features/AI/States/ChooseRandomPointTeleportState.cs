using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using Assets._Project.Develop.Runtime.Utilites.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class ChooseRandomPointTeleportState : State, IUpdatableState
    {
        private ICompositeCondition _canTeleport;
        private TeleportSystem _teleportSystem;
        private Transform _transform;
        private ReactiveVariable<float> _maxTeleportRange;

        public ChooseRandomPointTeleportState(Entity entity, TeleportSystem teleportSystem)
        {
            _canTeleport = entity.CanTeleport;
            _transform = entity.Transform;
            _maxTeleportRange = entity.MaxTeleportRange;
            _teleportSystem = teleportSystem;
        }

        public void Update(float deltaTime)
        {
            if (_canTeleport.Evaluate() == false)
                return;

            TeleportToRandomPoint(_maxTeleportRange.Value);
        }

        private void TeleportToRandomPoint(float range)
        {
            Vector3 centerPoint = Vector3.zero;
            Vector2 randomCircle = Random.insideUnitCircle * range;
            Vector3 newPosition = centerPoint + new Vector3(randomCircle.x, _transform.position.y, randomCircle.y);

            _teleportSystem.TeleportTo(newPosition);
        }
    }
}
