using Assets._Project.Develop.Runtime.UI.Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatPopupView : PopupViewBase
{
    [SerializeField] private Button _chatButton;
    [SerializeField] private Button _sendMessageButton;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TMP_Text _chatMessagesContent;
    [SerializeField] private ScrollRect _scrollRect;

    public event Action ChatButtonClicked;
    public event Action<string> SendMessageButtonClicked;

    private void OnEnable()
    {
        _chatButton.onClick.AddListener(OnChatButtonClicked);
        _sendMessageButton.onClick.AddListener(OnSendMessageButtonClicked);
    }

    private void OnDisable()
    {
        _chatButton.onClick.RemoveListener(OnChatButtonClicked);
        _sendMessageButton.onClick.RemoveListener(OnSendMessageButtonClicked);
    }

    public void OnSendMessageButtonClicked()
    {
        string messageContent = _inputField.text;

        if (messageContent.Length == 0)
            return;

        SendMessageButtonClicked?.Invoke(messageContent);
        _inputField.text = "";

        Canvas.ForceUpdateCanvases();
        _scrollRect.verticalNormalizedPosition = 0f;
    }

    public void AddText(string userColor, string userName, string value)
    {
        _chatMessagesContent.text += "\n" + $"<color=\"{userColor}\">{userName}</color>: " + value;
    }

    private void OnChatButtonClicked()
    {
        ChatButtonClicked?.Invoke();
    }
}