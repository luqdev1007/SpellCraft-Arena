using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuScreenView : MonoBehaviour, IView
    {
        [field: SerializeField] public IconTextListView WalletView { get; private set; }


        [SerializeField] private Button _openTestPopupButton;

        public event Action OpenTestPopupButtonClicked;

        private void OnEnable()
        {
            _openTestPopupButton.onClick.AddListener(OnOpenTestPopupClicked);
        }

        private void OnDisable()
        {
            _openTestPopupButton.onClick.RemoveListener(OnOpenTestPopupClicked);
        }

        private void OnOpenTestPopupClicked()
        {
            Debug.Log("Clicked");
            OpenTestPopupButtonClicked?.Invoke();
        }
    } 
}
