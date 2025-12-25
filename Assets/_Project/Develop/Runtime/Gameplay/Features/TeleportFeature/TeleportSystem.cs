using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature
{
    public class TeleportSystem
    {
        private ReactiveVariable<float> _currentEnergy;
        private ReactiveVariable<float> _amountOfEnergyForTeleport;

        private ReactiveEvent _teleportEvent;

        private Transform _transform;

        public TeleportSystem(Entity entity)
        {
            _currentEnergy = entity.EnergyCurrentValue;
            _amountOfEnergyForTeleport = entity.AmountOfEnergyForTeleport;
            _transform = entity.Transform;
            _teleportEvent = entity.TeleportRequest;
        }

        public void TeleportTo(Vector3 position)
        {
            _transform.position = position;
            _currentEnergy.Value -= _amountOfEnergyForTeleport.Value;
            _teleportEvent.Invoke();
        }
    }
}
