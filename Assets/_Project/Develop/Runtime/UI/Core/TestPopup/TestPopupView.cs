using TMPro;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Core.TestPopup
{
    public class TestPopupView : PopupViewBase
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(string value) => _text.text = value;
    }
}
