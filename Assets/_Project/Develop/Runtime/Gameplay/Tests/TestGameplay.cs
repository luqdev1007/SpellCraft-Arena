using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
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
        private Entity _heroEntity;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = _container.Resolve<EntitiesFactory>();
        }

        public void Run()
        {
            // _entitiesFactory.CreateGhost(Vector3.up + Vector3.forward * 5);
            _entitiesFactory.CreateMagicOrb(Vector3.up + Vector3.forward * 5);

            _heroEntity = _entitiesFactory.CreateHero(Vector3.zero + Vector3.up);
            _camera.Target.TrackingTarget = _heroEntity.Rigidbody.transform;

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _heroEntity.StartAttackRequest.Invoke();
            }

            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));

            _heroEntity.MoveDirection.Value = input;
            _heroEntity.RotationDirection.Value = input;
        }
    }
}
