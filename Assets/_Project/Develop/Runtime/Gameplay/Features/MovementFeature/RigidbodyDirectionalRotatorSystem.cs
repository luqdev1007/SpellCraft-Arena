using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public class RigidbodyDirectionalRotatorSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<Vector3> _moveDirection;
        private Rigidbody _rigidbody;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _rigidbody = entity.Rigidbody;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_moveDirection.Value.sqrMagnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_moveDirection.Value);
                _rigidbody.MoveRotation(targetRotation);
            }
        }
    }
}
