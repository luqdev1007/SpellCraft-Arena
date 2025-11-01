using Assets._Project.Develop.Runtime.UI.Core;

public class ChatPresenter : IPresenter
{
    private ChatView _chatView;

    public ChatPresenter(ChatView chatView)
    {
        _chatView = chatView;
    }

    public void Initialize()
    {
        _chatView.ChatButtonClicked += OnChatButtonClicked;
        _chatView.SendMessageButtonClicked += OnSendMessageButtonClicked;
    }

    public void Dispose()
    {
        _chatView.ChatButtonClicked -= OnChatButtonClicked;
        _chatView.SendMessageButtonClicked -= OnSendMessageButtonClicked;
    }

    private void OnSendMessageButtonClicked(string message)
    {
        _chatView.AddText("red", "LuQmu5", message);
    }

    private void OnChatButtonClicked()
    {
        if (_chatView.IsClosed)
            _chatView.ShowChat();
        else
            _chatView.CloseChat();
    }
}