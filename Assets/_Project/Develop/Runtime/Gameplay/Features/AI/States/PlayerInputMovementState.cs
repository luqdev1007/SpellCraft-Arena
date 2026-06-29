using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using Assets._Project.Develop.Runtime.Utilites.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class PlayerInputMovementState : State, IUpdatableState
    {
        private readonly IInputService _inputService;

        private readonly ReactiveVariable<Vector3> _movementDirection;
        private readonly ReactiveVariable<Vector3> _rotationDirection;
        private readonly ReactiveEvent _castRequest;

        public PlayerInputMovementState(Entity entity, IInputService inputService)
        {
            _inputService = inputService;

            _movementDirection = entity.MoveDirection;
            _rotationDirection = entity.RotationDirection;
            _castRequest = entity.CastRequest;
        }

        public void Update(float deltaTime)
        {
            _movementDirection.Value = _inputService.MoveDirection;
            _rotationDirection.Value = _inputService.RotateDirection;

            if (_inputService.IsCastRequested)
                _castRequest.Invoke();
        }

        public override void Exit()
        {
            base.Exit();

            _movementDirection.Value = Vector3.zero;
        }
    }
}
