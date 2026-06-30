using DG.Tweening;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    public class OrbIdleView : MonoBehaviour
    {
        private const float BobAmplitude = 0.06f;
        private const float BobDuration = 0.9f;
        private const float RotateSpeed = 80f;

        private Vector3 _baseLocalPos;
        private Tween _bobTween;
        private bool _stopped;

        public void SetBaseLocalPosition(Vector3 localPos)
        {
            _baseLocalPos = localPos;
            transform.localPosition = localPos;
            StartBob();
        }

        public void StopIdle()
        {
            _stopped = true;
            _bobTween?.Kill();
        }

        private void StartBob()
        {
            _bobTween?.Kill();
            _bobTween = transform
                .DOLocalMoveY(_baseLocalPos.y + BobAmplitude, BobDuration * 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void Update()
        {
            if (_stopped) return;
            transform.Rotate(Vector3.up, RotateSpeed * Time.deltaTime, Space.Self);
        }

        private void OnDestroy()
        {
            _bobTween?.Kill();
        }
    }
}
