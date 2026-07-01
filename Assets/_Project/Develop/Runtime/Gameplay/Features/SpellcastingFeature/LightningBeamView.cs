using DG.Tweening;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    [RequireComponent(typeof(LineRenderer))]
    public class LightningBeamView : MonoBehaviour
    {
        private const int SegmentCount = 6;
        private const float ZigzagAmplitude = 0.25f;
        private const float ScrollSpeed = 3f;
        private const float FadeDuration = 0.15f;

        private LineRenderer _lineRenderer;
        private float _elapsed;
        private bool _loggedRenderStats;

        public void Init(Vector3 from, Vector3 to, float duration)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = true;

            BuildZigzagPoints(from, to);

            Debug.Log($"[CLDebug] Init: duration={duration} parent={(transform.parent != null ? transform.parent.name : "null")} goPos={transform.position}");
            Debug.Log($"[CLDebug] Init: firstPoint={_lineRenderer.GetPosition(0)} lastPoint={_lineRenderer.GetPosition(SegmentCount)}");

            DOTween.To(() => 1f, SetAlpha, 0f, FadeDuration)
                .SetDelay(Mathf.Max(0f, duration - FadeDuration));

            Destroy(gameObject, duration + 0.05f);
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;

            if (_lineRenderer.material != null)
                _lineRenderer.material.mainTextureOffset = new Vector2(_elapsed * ScrollSpeed, 0f);

            if (_loggedRenderStats == false)
            {
                _loggedRenderStats = true;
                Debug.Log($"[CLDebug] RenderStats: isVisible={_lineRenderer.isVisible} bounds={_lineRenderer.bounds}");
            }
        }

        private void OnDestroy()
        {
            if (_lineRenderer != null && _lineRenderer.material != null)
                Destroy(_lineRenderer.material);
        }

        private void BuildZigzagPoints(Vector3 from, Vector3 to)
        {
            _lineRenderer.positionCount = SegmentCount + 1;

            Vector3 direction = to - from;
            Vector3 sideOffset = Vector3.Cross(direction.normalized, Vector3.up);

            if (sideOffset.sqrMagnitude < 0.001f)
                sideOffset = Vector3.right;

            for (int i = 0; i <= SegmentCount; i++)
            {
                float t = i / (float)SegmentCount;
                Vector3 point = Vector3.Lerp(from, to, t);

                if (i > 0 && i < SegmentCount)
                    point += sideOffset * Random.Range(-ZigzagAmplitude, ZigzagAmplitude);

                _lineRenderer.SetPosition(i, point);
            }
        }

        private void SetAlpha(float alpha)
        {
            if (_lineRenderer == null)
                return;

            Color start = _lineRenderer.startColor;
            Color end = _lineRenderer.endColor;
            start.a = alpha;
            end.a = alpha;
            _lineRenderer.startColor = start;
            _lineRenderer.endColor = end;
        }
    }
}
