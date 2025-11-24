using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayScreenView : MonoBehaviour, IView
    {
        public event Action OpenChatButtonClicked;

        [SerializeField] private Button _openChatButton;

        private void OnEnable()
        {
            _openChatButton.onClick.AddListener(OnOpenChatButtonClicked);
        }

        private void OnDisable()
        {
            _openChatButton.onClick.RemoveListener(OnOpenChatButtonClicked);
        }

        private void OnOpenChatButtonClicked()
        {
            OpenChatButtonClicked?.Invoke();
        }
    }
}
