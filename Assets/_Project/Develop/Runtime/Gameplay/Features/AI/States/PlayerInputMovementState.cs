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
        private readonly ReactiveVariable<bool> _isCasting;
        private readonly ReactiveEvent _castRequest;
        private readonly ReactiveEvent _blinkRequest;
        private readonly Transform _entityTransform;

        public PlayerInputMovementState(Entity entity, IInputService inputService)
        {
            _inputService = inputService;

            _movementDirection = entity.MoveDirection;
            _rotationDirection = entity.RotationDirection;
            _isCasting = entity.IsCasting;
            _castRequest = entity.CastRequest;
            _blinkRequest = entity.BlinkRequest;
            _entityTransform = entity.Transform;
        }

        public void Update(float deltaTime)
        {
            Vector3 moveDir = _inputService.MoveDirection;
            _movementDirection.Value = moveDir;

            if (_isCasting.Value)
            {
                // During cast windup: rotate toward mouse for aiming
                Vector3 mouseWorldPos = _inputService.RotateDirection;
                Vector3 heroFlat = new Vector3(_entityTransform.position.x, 0f, _entityTransform.position.z);
                Vector3 aimDir = mouseWorldPos - heroFlat;
                if (aimDir.sqrMagnitude > 0.01f)
                    _rotationDirection.Value = aimDir.normalized;
            }
            else if (moveDir != Vector3.zero)
            {
                // Outside cast: rotate only toward movement direction
                _rotationDirection.Value = moveDir;
            }

            if (_inputService.IsCastRequested)
                _castRequest.Invoke();

            if (_inputService.IsBlinkRequested)
                _blinkRequest.Invoke();
        }

        public override void Exit()
        {
            base.Exit();

            _movementDirection.Value = Vector3.zero;
        }
    }
}
