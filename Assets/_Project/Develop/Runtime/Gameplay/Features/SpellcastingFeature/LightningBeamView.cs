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
        private const float FadeDuration = 0.12f;
        private const float BeamWidth = 0.15f;

        private static Material _materialTemplate;

        private LineRenderer _lineRenderer;
        private float _elapsed;

        public void Init(Vector3 from, Vector3 to, float duration)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.alignment = LineAlignment.View;
            _lineRenderer.textureMode = LineTextureMode.Tile;
            _lineRenderer.widthMultiplier = BeamWidth;
            _lineRenderer.material = new Material(GetOrCreateMaterialTemplate());

            BuildZigzagPoints(from, to);

            DOTween.To(() => 1f, SetAlpha, 0f, FadeDuration)
                .SetDelay(Mathf.Max(0f, duration - FadeDuration))
                .SetUpdate(true);

            Destroy(gameObject, duration + 0.05f);
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;

            if (_lineRenderer.material != null)
                _lineRenderer.material.mainTextureOffset = new Vector2(_elapsed * ScrollSpeed, 0f);
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

        private static Material GetOrCreateMaterialTemplate()
        {
            if (_materialTemplate != null)
                return _materialTemplate;

            _materialTemplate = new Material(Shader.Find("Sprites/Default"))
            {
                color = new Color(0.6f, 0.85f, 1f, 1f)
            };

            Texture2D texture = Resources.Load<Texture2D>("Textures/VFX/T_VFX_Zap_Lightning_01_Opti");

            if (texture != null)
                _materialTemplate.mainTexture = texture;

            return _materialTemplate;
        }
    }
}
