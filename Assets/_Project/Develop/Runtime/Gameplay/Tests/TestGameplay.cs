using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Tests
{
    public class TestGameplay : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private Transform _portalSpawnPoint;

        private DIContainer _container;

        private EntitiesFactory _entitiesFactory;
        private BrainsFactory _brainsFactory;

        private Entity _heroEntity;
        private IInputService _playerInput;

        private Entity _ghostEntity;
        private Entity _orbEntity;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;

            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _brainsFactory = _container.Resolve<BrainsFactory>();

            _playerInput = container.Resolve<IInputService>();
        }

        public void Run()
        {
            _ghostEntity = _entitiesFactory.CreateGhost(Vector3.up + Vector3.forward * 5);
            _brainsFactory.CreateGhostBrain(_ghostEntity);

            // _orbEntity = _entitiesFactory.CreateMagicOrb(Vector3.up + Vector3.forward * 5);
            // _brainsFactory.CreateMagicOrbBrain(_orbEntity);

            _heroEntity = _entitiesFactory.CreateHero(Vector3.zero);

            _camera.Target.TrackingTarget = _heroEntity.Transform;

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            /*
            if (Input.GetKeyDown(KeyCode.H))
            {
                _orbEntity.CurrentHealth.Value = _orbEntity.MaxHealth.Value;
            }
            */

            _heroEntity.MoveDirection.Value = _playerInput.MoveDirection;
            _heroEntity.RotationDirection.Value = _playerInput.MoveDirection == Vector3.zero? _playerInput.RotateDirection : _playerInput.MoveDirection;

            if (_playerInput.IsAttackKeyPressed)
                _heroEntity.StartAttackRequest.Invoke();
        }
    }
}
