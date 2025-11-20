using System;

public class ChatService
{
    public event Action<string> MessageSent;

    public void SendMessage(ChatPopupView view, string message, string color, string userName)
    {
        view.AddText(color, userName, message);

        MessageSent?.Invoke(message);
    }
}