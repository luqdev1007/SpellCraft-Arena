using Assets._Project.Develop.Runtime.Configs.Gameplay.Blink;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilites;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.BlinkFeature
{
    public class BlinkSystem : IInitializableSystem, IDisposableSystem
    {
        private const float MinTravelDistance = 0.05f;
        private const float SweepHeightOffset = 1f;

        private readonly BlinkConfig _config;

        private Entity _entity;
        private ReactiveEvent _blinkRequest;
        private ReactiveEvent _blinkExecutedEvent;
        private ReactiveVariable<bool> _isCasting;
        private ReactiveVariable<bool> _inBlinkCooldown;
        private ReactiveVariable<Vector3> _rotationDirection;
        private ReactiveVariable<float> _currentMana;

        private IDisposable _blinkRequestSub;

        public BlinkSystem(BlinkConfig config)
        {
            _config = config;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _blinkRequest = entity.BlinkRequest;
            _blinkExecutedEvent = entity.BlinkExecutedEvent;
            _isCasting = entity.IsCasting;
            _inBlinkCooldown = entity.InBlinkCooldown;
            _rotationDirection = entity.RotationDirection;
            _currentMana = entity.CurrentMana;

            _blinkRequestSub = _blinkRequest.Subscribe(OnBlinkRequested);
        }

        public void OnDispose()
        {
            _blinkRequestSub?.Dispose();
        }

        // Gated here (not in PlayerInputMovementState) so the mobile UI button,
        // which invokes BlinkRequest directly, is covered by the same checks as desktop input.
        private void OnBlinkRequested()
        {
            if (_isCasting.Value)
            {
                Debug.Log("[Blink] blocked by cast windup");
                return;
            }

            if (_inBlinkCooldown.Value)
            {
                Debug.Log("[Blink] on cooldown");
                return;
            }

            Vector3 direction = _rotationDirection.Value;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.0001f)
                direction = _entity.Transform.forward;

            direction.y = 0f;
            direction.Normalize();

            Vector3 heroPosition = _entity.Transform.position;
            Vector3 sweepOrigin = heroPosition + Vector3.up * SweepHeightOffset;

            float travel = _config.Distance;

            if (Physics.SphereCast(sweepOrigin, _config.SphereCastRadius, direction, out RaycastHit hit, _config.Distance, LayersAPI.LayerMaskEnviroment))
                travel = Mathf.Max(0f, hit.distance - _config.WallBuffer);

            if (travel <= MinTravelDistance)
            {
                Debug.Log("[Blink] blocked by wall");
                return;
            }

            if (_currentMana.Value < _config.ManaCost)
            {
                Debug.Log($"[Blink] not enough mana ({_currentMana.Value:0}/{_config.ManaCost:0})");
                return;
            }

            _currentMana.Value = Mathf.Max(0f, _currentMana.Value - _config.ManaCost);

            SpawnVfx(heroPosition);

            Vector3 destination = heroPosition + direction * travel;
            _entity.Rigidbody.position = destination;
            _entity.Rigidbody.linearVelocity = Vector3.zero;

            SpawnVfx(destination);

            _blinkExecutedEvent.Invoke();
        }

        private void SpawnVfx(Vector3 position)
        {
            if (string.IsNullOrEmpty(_config.VfxPrefabPath))
                return;

            GameObject vfxPrefab = Resources.Load<GameObject>(_config.VfxPrefabPath);

            if (vfxPrefab == null)
                return;

            GameObject vfx = Object.Instantiate(vfxPrefab, position, Quaternion.identity);
            Object.Destroy(vfx, _config.VfxLifetime);
        }
    }
}
