using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Core
{
    public abstract class PopupViewBase : MonoBehaviour, IShowableView
    {
        [SerializeField] private CanvasGroup _mainGroup;
        [SerializeField] private Transform _body;
        [SerializeField] private Image _anticlicker;

        private Tween _currentAnimation;

        public event Action CloseRequest;

        private void Awake()
        {
            _mainGroup.alpha = 0;
        }

        public Tween Show()
        {
            KillCurrentAnimation();

            OnPreShow();

            // anim
            _mainGroup.alpha = 1;

            Sequence animation = DOTween.Sequence();

            animation
                .Append(_anticlicker
                .DOFade(endValue: 0.75f, duration: 0.2f)
                .From(0))
                .Join(_body
                .DOScale(endValue: 1, duration: 0.5f)
                .From(0)
                .SetEase(Ease.OutBack));

            ModifyShowAnimation(animation);

            animation.OnComplete(() => OnPostShow());

            return _currentAnimation = animation.SetUpdate(true).Play();
        }

        public Tween Hide()
        {
            KillCurrentAnimation();

            OnPreHide();

            // anim
            _mainGroup.alpha = 0;

            Sequence animation = DOTween.Sequence();
            ModifyHideAnimation(animation);

            animation.OnComplete(() => OnPostHide());

            return _currentAnimation = animation.SetUpdate(true).Play();
        }

        public void OnCloseButtonClicked() => CloseRequest?.Invoke();

        protected virtual void ModifyShowAnimation(Sequence animation)
        {

        }

        protected virtual void ModifyHideAnimation(Sequence animation)
        {

        }

        protected virtual void OnPostHide() { }

        protected virtual void OnPreHide() { }

        protected virtual void OnPostShow() { }

        protected virtual void OnPreShow() { }

        private void OnDestroy()
        {
            KillCurrentAnimation();
        }

        private void KillCurrentAnimation()
        {
            if (_currentAnimation != null)
                _currentAnimation.Kill();
        }
    }
}
