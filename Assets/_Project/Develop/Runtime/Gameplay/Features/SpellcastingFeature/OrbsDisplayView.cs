using Assets._Project.Develop.Runtime.Configs.Gameplay.Spells;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class OrbsDisplayView : EntityView
    {
        [SerializeField] private Transform _orbsRoot;
        [SerializeField] private float _orbHeight = 2f;
        [SerializeField] private float _orbSpacing = 0.5f;

        [Header("Orb Prefabs (by Aspect enum order: Blood, Fire, Light, Death, Nature, Ice, Magic)")]
        [SerializeField] private GameObject[] _orbPrefabsByAspect = new GameObject[7];

        private readonly List<GameObject> _activeOrbs = new();
        private IDisposable _castingSub;
        private Entity _entity;

        protected override void OnEntityStartedWork(Entity entity)
        {
            _entity = entity;
            _castingSub = entity.IsCasting.Subscribe(OnCastingChanged);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);
            _castingSub?.Dispose();
            ClearOrbs(false);
        }

        public void ShowAspects(IReadOnlyList<Aspect> aspects)
        {
            ClearOrbs(true);

            for (int i = 0; i < aspects.Count; i++)
            {
                int idx = (int)aspects[i];
                if (idx < 0 || idx >= _orbPrefabsByAspect.Length) continue;
                if (_orbPrefabsByAspect[idx] == null) continue;

                GameObject orb = Object.Instantiate(_orbPrefabsByAspect[idx], GetOrbRoot());
                orb.transform.localPosition = GetOrbLocalPosition(i, aspects.Count);
                orb.transform.localScale = Vector3.zero;
                orb.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);

                _activeOrbs.Add(orb);
            }
        }

        private Transform GetOrbRoot()
        {
            return _orbsRoot != null ? _orbsRoot : transform;
        }

        private Vector3 GetOrbLocalPosition(int index, int total)
        {
            float totalWidth = (total - 1) * _orbSpacing;
            float x = -totalWidth / 2f + index * _orbSpacing;
            return new Vector3(x, _orbHeight, 0f);
        }

        private void OnCastingChanged(bool prev, bool isCasting)
        {
            if (isCasting)
                ClearOrbs(true);
        }

        private void ClearOrbs(bool animated)
        {
            foreach (GameObject orb in _activeOrbs)
            {
                if (orb == null) continue;

                if (animated)
                {
                    orb.transform
                        .DOScale(Vector3.zero, 0.15f)
                        .SetEase(Ease.InBack)
                        .OnComplete(() => { if (orb != null) Object.Destroy(orb); });
                }
                else
                {
                    Object.Destroy(orb);
                }
            }

            _activeOrbs.Clear();
        }
    }
}
