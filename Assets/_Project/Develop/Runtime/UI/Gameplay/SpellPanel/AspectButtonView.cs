using Assets._Project.Develop.Runtime.Configs.Gameplay.Spells;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.SpellPanel
{
    public class AspectButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectionFrame;
        [SerializeField] private Button _button;

        private Aspect _aspect;
        private float _pressTime;
        private bool _isPressed;
        private bool _isSelected;

        public Aspect Aspect => _aspect;
        public bool IsSelected => _isSelected;

        public event Action<Aspect> Clicked;
        public event Action<Aspect> LongPressed;

        public void Setup(Aspect aspect, Sprite icon)
        {
            _aspect = aspect;

            if (icon != null && _icon != null)
                _icon.sprite = icon;

            if (_selectionFrame != null)
            {
                _selectionFrame.enabled = true;
                Color c = _selectionFrame.color;
                c.a = 0f;
                _selectionFrame.color = c;
            }

            SetSelected(false);
        }

        public void SetSelected(bool selected)
        {
            _isSelected = selected;

            if (_selectionFrame == null)
                return;

            _selectionFrame.enabled = true;
            _selectionFrame.DOKill();
            _selectionFrame.DOFade(selected ? 1f : 0f, 0.18f)
                .SetUpdate(true)
                .SetEase(selected ? Ease.OutQuad : Ease.InQuad);

            if (selected)
            {
                _selectionFrame.transform.DOKill();
                _selectionFrame.transform
                    .DOPunchScale(Vector3.one * 0.12f, 0.18f, 4, 0.4f)
                    .SetUpdate(true);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
            _pressTime = 0f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isPressed == false)
                return;

            _isPressed = false;

            if (_pressTime < 0.35f)
                Clicked?.Invoke(_aspect);
            else
                LongPressed?.Invoke(_aspect);
        }

        private void Update()
        {
            if (_isPressed)
                _pressTime += Time.deltaTime;
        }
    }
}
