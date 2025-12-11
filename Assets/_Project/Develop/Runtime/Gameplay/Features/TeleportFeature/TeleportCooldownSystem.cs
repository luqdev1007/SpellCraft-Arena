using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.TeleportFeature
{
    public class TeleportCooldownSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _initialCooldown;
        private ReactiveVariable<float> _currentCooldown;
        private ReactiveVariable<bool> _isCooldownReady;

        public void OnInit(Entity entity)
        {
            _initialCooldown = entity.TeleportInitialCooldown;
            _currentCooldown = entity.TeleportCurrentCooldown;
            _isCooldownReady = entity.IsTeleportCooldownReady;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_currentCooldown.Value <= 0)
            {
                _isCooldownReady.Value = true;
                return;
            }

            // Debug.Log("Teleport cooldown: " + _currentCooldown.Value);

            if (_currentCooldown.Value > 0)
            {
                _currentCooldown.Value -= deltaTime;
                _isCooldownReady.Value = false;
            }
        }
    }
}
