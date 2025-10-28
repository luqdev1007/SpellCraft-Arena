using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Core.TestPopup
{
    public class TestPopupView : PopupViewBase
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(string value) => _text.text = value;

        protected override void ModifyShowAnimation(Sequence animation)
        {
            base.ModifyShowAnimation(animation);

            animation
                .Append(_text.transform
                    .DOScale(1, 0.2f)
                    .From(0)
                    .SetEase(Ease.OutSine));
        }
    }
}
