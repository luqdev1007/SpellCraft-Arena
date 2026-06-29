using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    [RequireComponent(typeof(Animator))]
    public class SpellCastView : EntityView
    {
        private static readonly int IsAttackingKey = Animator.StringToHash("IsAttacking");

        [SerializeField] private Animator _animator;

        private IDisposable _castingSub;

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
        }

        protected override void OnEntityStartedWork(Entity entity)
        {
            _castingSub = entity.IsCasting.Subscribe(OnCastingChanged);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);
            _castingSub?.Dispose();
        }

        private void OnCastingChanged(bool prev, bool isCasting)
        {
            _animator.SetBool(IsAttackingKey, isCasting);
        }
    }
}
