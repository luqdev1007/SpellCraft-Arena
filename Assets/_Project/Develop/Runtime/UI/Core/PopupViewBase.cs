using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Core
{
    public abstract class PopupViewBase : MonoBehaviour, IShowableView
    {
        [SerializeField] private CanvasGroup _mainGroup;

        public event Action CloseRequest;

        private void Awake()
        {
            _mainGroup.alpha = 0;
        }

        public void Hide()
        {
            OnPreHide();

            // anim
            _mainGroup.alpha = 0;

            OnPostHide();
        }

        public void Show()
        {
            OnPreShow();

            // anim
            _mainGroup.alpha = 1;

            OnPostShow();
        }

        public void OnCloseButtonClicked() => CloseRequest?.Invoke();

        protected virtual void OnPostHide() { }

        protected virtual void OnPreHide() { }

        protected virtual void OnPostShow() { }

        protected virtual void OnPreShow() { }
    }
}
