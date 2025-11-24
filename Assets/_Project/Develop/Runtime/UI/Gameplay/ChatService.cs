using System;

public class ChatService
{
    // история сообщений
    // реактивный список, а чат презентер подписан на список этот
    // очистка чата

    public event Action<string> MessageSent;

    public void SendMessage(ChatPopupView view, string message, string color, string userName)
    {
        view.AddText(color, userName, message);

        MessageSent?.Invoke(message);
    }
}