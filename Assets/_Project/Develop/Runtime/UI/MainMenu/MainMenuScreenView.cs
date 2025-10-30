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
        [SerializeField] private Button _openLevelsMenuButton;

        public event Action OpenTestPopupButtonClicked;
        public event Action OpenLevelsMenuButtonClicked;

        private void OnEnable()
        {
            _openTestPopupButton.onClick.AddListener(OnOpenTestPopupClicked);
            _openLevelsMenuButton.onClick.AddListener(OnOpenLevelsMenuButtonClicked);
        }

        private void OnDisable()
        {
            _openTestPopupButton.onClick.RemoveListener(OnOpenTestPopupClicked);
            _openLevelsMenuButton.onClick.RemoveListener(OnOpenLevelsMenuButtonClicked);
        }

        private void OnOpenTestPopupClicked()
        {
            Debug.Log("Clicked");
            OpenTestPopupButtonClicked?.Invoke();
        }

        private void OnOpenLevelsMenuButtonClicked()
        {
            Debug.Log("Clicked");
            OpenLevelsMenuButtonClicked?.Invoke();
        }
    } 
}
