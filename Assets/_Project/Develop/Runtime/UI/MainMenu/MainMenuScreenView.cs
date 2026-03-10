using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreenView : MonoBehaviour, IView
{
    public event Action StartGameButtonClicked;

    [SerializeField] private IconTextView _goldView;
    [SerializeField] private IconTextView _diamondView;
    [SerializeField] private IconTextView _winsView;
    [SerializeField] private IconTextView _losesView;

    [SerializeField] private Button _startGameButton;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
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

    private void OnStartGameButtonClicked()
    {
        StartGameButtonClicked?.Invoke();
    }
}
