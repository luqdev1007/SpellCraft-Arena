using Assets._Project.Develop.Runtime.Gameplay.Common;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Conditions;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature
{
    public class TeleportInRandomPositionSystem : IInitializableSystem, IUpdatableSystem
    {
        private ICompositeCondition _canTeleport;
        private ReactiveVariable<float> _currentEnergy;
        private ReactiveVariable<float> _amountOfEnergyForTeleport;
        private ReactiveVariable<float> _teleportCurrentCooldown;
        private ReactiveVariable<float> _teleportInitialCooldown;

        private ReactiveEvent _teleportEvent;

        private Transform _transform;

        public void OnInit(Entity entity)
        {
            _canTeleport = entity.CanTeleport;
            _currentEnergy = entity.EnergyCurrentValue;
            _amountOfEnergyForTeleport = entity.AmountOfEnergyForTeleport;
            _teleportCurrentCooldown = entity.TeleportCurrentCooldown;
            _teleportInitialCooldown = entity.TeleportInitialCooldown;
            _transform = entity.Transform;
            _teleportEvent = entity.TeleportRequest;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_canTeleport.Evaluate() == false)
                return;

            TeleportToRandomPoint(5);

            // Debug.Log(_currentEnergy.Value);
        }

        private void TeleportToRandomPoint(float teleportRadius)
        {
            Vector3 centerPoint = Vector3.zero; 
            Vector2 randomCircle = Random.insideUnitCircle * teleportRadius;
            Vector3 newPosition = centerPoint + new Vector3(randomCircle.x, _transform.position.y, randomCircle.y);

            _transform.position = newPosition;

            _currentEnergy.Value -= _amountOfEnergyForTeleport.Value;
            _teleportEvent.Invoke();

            // Debug.Log($"Объект телепортирован в случайную точку: {newPosition} в радиусе {teleportRadius} от центра сцены.");
        }
    }
}
