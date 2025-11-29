using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public class TransformDirectionalRotatorSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<Vector3> _moveDirection;
        private Transform _transform;

        public void OnInit(Entity entity)
        {
            _moveDirection = entity.MoveDirection;
            _transform = entity.Transform;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_moveDirection.Value.sqrMagnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_moveDirection.Value);
                _transform.rotation = targetRotation;
            }
        }
    }
}
