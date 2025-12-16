using Assets._Project.Develop.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
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
        private Entity _ghostEntity;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _brainsFactory = _container.Resolve<BrainsFactory>();
        }

        public void Run()
        {
            _ghostEntity = _entitiesFactory.CreateGhost(Vector3.up + Vector3.forward * 5);

            // _entitiesFactory.CreateMagicOrb(Vector3.up + Vector3.forward * 5);

            _heroEntity = _entitiesFactory.CreateHero(Vector3.zero + Vector3.up);
            _heroEntity.AddCurrentTarget();
            _brainsFactory.CreateMainHeroBrain(_heroEntity, new NearestDamagableTargetSelector(_heroEntity));

            _camera.Target.TrackingTarget = _heroEntity.Transform;

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            if (Input.GetKeyDown(KeyCode.G))
            {
                _brainsFactory.CreateGhostBrain(_ghostEntity);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _heroEntity.StartAttackRequest.Invoke();
            }
        }
    }
}
