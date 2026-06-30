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
        [SerializeField] private float _orbHeight = 2.3f;
        [SerializeField] private float _arcRadius = 0.85f;
        [SerializeField] private float _arcDegrees = 110f;

        [Header("Cast flash VFX (assign in inspector)")]
        [SerializeField] private GameObject _castVfxPrefab;

        [Header("Orb Prefabs (by Aspect enum order: Blood, Fire, Light, Death, Nature, Ice, Magic)")]
        [SerializeField] private GameObject[] _orbPrefabsByAspect = new GameObject[7];

        private readonly List<GameObject> _activeOrbs = new();
        private IDisposable _castingSub; // Not used anymore

        protected override void OnEntityStartedWork(Entity entity)
        {
            // Cast animation now triggered from SpellPanelPresenter on 3rd aspect, not on cast button
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
                orb.transform.DOScale(Vector3.one, 0.22f).SetEase(Ease.OutBack);

                OrbIdleView idle = orb.AddComponent<OrbIdleView>();
                idle.SetBaseLocalPosition(GetOrbLocalPosition(i, aspects.Count));

                _activeOrbs.Add(orb);
            }
        }

        private Transform GetOrbRoot() => _orbsRoot != null ? _orbsRoot : transform;

        private Vector3 GetOrbLocalPosition(int index, int total)
        {
            if (total == 1)
                return new Vector3(0f, _orbHeight, 0f);

            float startAngle = -_arcDegrees * 0.5f;
            float step = _arcDegrees / (total - 1);
            float angleDeg = startAngle + index * step;
            float rad = angleDeg * Mathf.Deg2Rad;

            float x = Mathf.Sin(rad) * _arcRadius;
            float yOffset = (Mathf.Cos(rad) - 1f) * _arcRadius * 0.25f;

            return new Vector3(x, _orbHeight + yOffset, 0f);
        }



        public void PlayCastAnimation()
        {
            Transform root = GetOrbRoot();
            Vector3 worldTarget = root.TransformPoint(new Vector3(0f, _orbHeight, 0f));

            List<GameObject> orbs = new List<GameObject>(_activeOrbs);
            _activeOrbs.Clear();

            int total = orbs.Count;
            int done = 0;

            foreach (GameObject orb in orbs)
            {
                if (orb == null)
                {
                    done++;
                    if (done == total) SpawnCastFlash(worldTarget);
                    continue;
                }

                OrbIdleView idle = orb.GetComponent<OrbIdleView>();
                idle?.StopIdle();

                orb.transform
                    .DOMove(worldTarget, 0.28f)
                    .SetEase(Ease.InCubic)
                    .OnComplete(() =>
                    {
                        if (orb != null) Object.Destroy(orb);
                        done++;
                        if (done == total) SpawnCastFlash(worldTarget);
                    });
            }
        }

        private void SpawnCastFlash(Vector3 worldPos)
        {
            if (_castVfxPrefab == null)
                return;

            GameObject vfx = Object.Instantiate(_castVfxPrefab, worldPos, Quaternion.identity);
            Object.Destroy(vfx, 3f);
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


