using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilites.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    [RequireComponent(typeof(Animator))]
    public class SpellCastView : EntityView
    {
        private static readonly int IsAttackingKey = Animator.StringToHash("IsAttacking");
        private static readonly int SpeedKey = Animator.StringToHash("AttackAnimationSpeedMultiplier");

        [SerializeField] private Animator _animator;

        private ReactiveVariable<float> _initialTime;
        private IDisposable _castingSub;
        private IDisposable _speedSub;

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
        }

        protected override void OnEntityStartedWork(Entity entity)
        {
            _initialTime = entity.AttackProcessInitialTime;
            _castingSub = entity.IsCasting.Subscribe(OnCastingChanged);
            _speedSub = entity.AttackProcessModifiedTime.Subscribe(OnModifiedTimeChanged);
            OnModifiedTimeChanged(0f, entity.AttackProcessModifiedTime.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);
            _castingSub?.Dispose();
            _speedSub?.Dispose();
        }

        private void OnCastingChanged(bool prev, bool isCasting)
        {
            _animator.SetBool(IsAttackingKey, isCasting);
        }

        private void OnModifiedTimeChanged(float prev, float modifiedTime)
        {
            if (modifiedTime > 0f)
                _animator.SetFloat(SpeedKey, _initialTime.Value / modifiedTime);
        }
    }
}
