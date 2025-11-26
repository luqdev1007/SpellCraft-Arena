using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.Common;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Tests
{
    public class TestGameplay : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera _camera;

        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;

        private Entity _entity;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = _container.Resolve<EntitiesFactory>();
        }

        public void Run()
        {
            _entity = _entitiesFactory.CreateTestEntity(Vector3.zero);

            _camera.Target.TrackingTarget = _entity.CharacterController.transform;

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));

            _entity.MoveDirection.Value = input;
        }
    }
}
