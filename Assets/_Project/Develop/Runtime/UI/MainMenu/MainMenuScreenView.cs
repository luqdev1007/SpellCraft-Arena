using Assets._Project.Develop.Runtime.UI.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreenView : MonoBehaviour, IView
{
    public event Action StartGameButtonClicked;
    public event Action ResetStatsButtonClicked;
    public event Action OpenChatButtonClicked;

    public event Action TESTMINIGAMEBTNCLICKED;

    [SerializeField] private IconTextView _goldView;
    [SerializeField] private IconTextView _winsView;
    [SerializeField] private IconTextView _losesView;

    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _resetStatsButton;
    [SerializeField] private Button _openChatButton;
    [SerializeField] private Button TESTMINIGAMEBTN;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        _resetStatsButton.onClick.AddListener(OnResetStatsButtonClicked);
        _openChatButton.onClick.AddListener(OnOpenChatButtonClicked);

        TESTMINIGAMEBTN.onClick.AddListener(MINIGAMEBUTTONPRESSED);
    }

    private void OnDisable()
    {
        _startGameButton.onClick.RemoveListener(OnStartGameButtonClicked);
        _resetStatsButton.onClick.RemoveListener(OnResetStatsButtonClicked);
        _openChatButton.onClick.RemoveListener(OnOpenChatButtonClicked);

        TESTMINIGAMEBTN.onClick.RemoveListener(MINIGAMEBUTTONPRESSED);
    }

    private void MINIGAMEBUTTONPRESSED()
    {
        TESTMINIGAMEBTNCLICKED?.Invoke();
    }

    public void EnableResetButton()
    {
        _resetStatsButton.interactable = true;
    }

    public void DisableResetButton()
    {
        _resetStatsButton.interactable = false;
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

    private void OnOpenChatButtonClicked()
    {
        OpenChatButtonClicked?.Invoke();
    }

    private void OnResetStatsButtonClicked()
    {
        ResetStatsButtonClicked?.Invoke();
    }

    private void OnStartGameButtonClicked()
    {
        StartGameButtonClicked?.Invoke();
    }
}
