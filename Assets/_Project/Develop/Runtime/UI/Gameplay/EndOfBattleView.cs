using Assets._Project.Develop.Runtime.UI.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class EndOfBattleView : MonoBehaviour, IView
    {
        [SerializeField] private CanvasGroup _mainGroup;
        [SerializeField] private IconTextView _goldView;
        [SerializeField] private IconTextView _winsView;
        [SerializeField] private IconTextView _losesView;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _headerText;

        [field: SerializeField] public Button NextButton { get; private set; }
        [field: SerializeField] public Button RestartButton { get; private set; }

        public event Action NextButtonClicked;
        public event Action ExitButtonClicked;
        public event Action RestartButtonClicked;

        private void OnEnable()
        {
            NextButton.onClick.AddListener(OnNextButtonClicked);
            RestartButton.onClick.AddListener(OnRestartButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }

        private void OnDisable()
        {
            NextButton.onClick.RemoveListener(OnNextButtonClicked);
            RestartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        public void Show()
        {
            _mainGroup.alpha = 1;
            _mainGroup.blocksRaycasts = true;
            _mainGroup.interactable = true;
        }

        public void Hide()
        {
            _mainGroup.alpha = 0;
            _mainGroup.blocksRaycasts = false;
            _mainGroup.interactable = false;
        }

        public void SetHeaderText(string value)
        {
            _headerText.text = value;
        }

        public void SetWinsText(string value)
        {
            _winsView.SetText(value);
        }

        public void SetLosesText(string value)
        {
            _losesView.SetText(value);
        }

        public void SetGoldText(string value)
        {
            _goldView.SetText(value);
        }

        private void OnNextButtonClicked()
        {
            NextButtonClicked?.Invoke();
        }

        private void OnExitButtonClicked()
        {
            ExitButtonClicked?.Invoke();
        }

        private void OnRestartButtonClicked()
        {
            RestartButtonClicked?.Invoke();
        }
    }
}